using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EGPS.Application.Helpers;
using AutoMapper;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EGPS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/v1/audits")]
    [ApiController]
    public class AuditsController : ControllerBase
    {
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IMapper _mapper;

        public AuditsController(IUserActivityRepository userActivityRepository, IMapper mapper)
        {
            _userActivityRepository =
                userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<UserActivityDTO>), 200)]
        public async Task<IActionResult> GetAuditDetails(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();

                var audit = await _userActivityRepository.SingleOrDefault(x => x.Id == id && x.AccountId == userClaims.AccountId);

                if (audit == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Audit with id {id} not found",
                        errors = new { }
                    });
                }

                var auditDto = _mapper.Map<UserActivityDTO>(audit);
                return Ok(new SuccessResponse<UserActivityDTO>
                {
                    success = true,
                    message = "Audit retrieved successfully",
                    data = auditDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpGet(Name = "GetAudits")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<UserActivitiesDTO>>>), 200)]
        public async Task<IActionResult> GetAudits([FromQuery] AuditParameters auditParameters)
        {
            try
            {
                var userClaims = User.UserClaims();
                if (auditParameters.StartDate.HasValue && auditParameters.EndDate.HasValue &&
                    auditParameters.EndDate < auditParameters.StartDate)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "EndDate cannot be less than startDate",
                        errors = new { }
                    });
                }
                var userActivities = await _userActivityRepository.GetAudits(auditParameters, userClaims.AccountId);

                var prevLink = userActivities.HasPrevious ? CreateResourceUri(auditParameters, ResourceUriType.PreviousPage)
                    : null;
                var nextLink = userActivities.HasNext
                    ? CreateResourceUri(auditParameters, ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(auditParameters, ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = userActivities.TotalPages,
                    perPage = userActivities.PageSize,
                    totalEntries = userActivities.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<UserActivitiesDTO>>
                {
                    success = true,
                    message = "Audits retrieved successfully",
                    data = userActivities,
                    meta = new Meta
                    {
                        pagination = pagination
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        #region CreateResource
        private string CreateResourceUri(AuditParameters auditParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetAudits",
                        new
                        {
                            PageNumber = auditParameters.PageNumber - 1,
                            auditParameters.PageSize,
                            auditParameters.Search,
                            auditParameters.StartDate,
                            auditParameters.EndDate
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetAudits",
                        new
                        {
                            PageNumber = auditParameters.PageNumber + 1,
                            auditParameters.PageSize,
                            auditParameters.Search,
                            auditParameters.StartDate,
                            auditParameters.EndDate
                        });

                default:
                    return Url.Link("GetAudits",
                        new
                        {
                            auditParameters.PageNumber,
                            auditParameters.PageSize,
                            auditParameters.Search,
                            auditParameters.StartDate,
                            auditParameters.EndDate
                        });
            }

        }
        #endregion

    }
}
