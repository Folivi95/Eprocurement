using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EGPS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/v1/generalPlans")]
    [ApiController]
    public class GeneralPlansController : ControllerBase
    {
        private readonly IGeneralPlanRepository _generalPlanRepository;
        private readonly IMapper _mapper;
        private readonly IMinistryRepository _ministryRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private const string USER_IP_ADDRESS = "User-IP-Address";

        public GeneralPlansController(
            IGeneralPlanRepository generalPlanRepository,
            IMapper mapper,
            IMinistryRepository ministryRepository,
            IUserActivityRepository userActivityRepository)
        {
            _generalPlanRepository = generalPlanRepository ?? throw new ArgumentNullException(nameof(generalPlanRepository));
            _mapper =  mapper ?? throw new ArgumentNullException(nameof(mapper));
            _ministryRepository = ministryRepository ?? throw new ArgumentNullException(nameof(ministryRepository));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
        }

        /// <summary>
        /// An endpoint to create a general plan
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponse<GeneralPlanDTO>), 200)]
        public async Task<IActionResult> CreateGeneralPlan(GeneralPlanForCreation generalPlanForCreation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your create general plan request failed",
                        errors  = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();

                var ministry = await _ministryRepository.GetByIdAsync(generalPlanForCreation.MinistryId);

                if (ministry == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Ministry with id {generalPlanForCreation.MinistryId} not found ",
                        errors  = new { }
                    });
                }

                var isMinsitryGeneralPlanExist =
                    await _generalPlanRepository.ExistsAsync(x => x.MinistryId == generalPlanForCreation.MinistryId);

                if (isMinsitryGeneralPlanExist)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"General plan already created for this minstry",
                        errors  = new { }
                    });
                }

                var generalPlan = _mapper.Map<GeneralPlan>(generalPlanForCreation);
                generalPlan.CreatedById = userClaims.UserId;

                await _generalPlanRepository.AddAsync(generalPlan);

                var userActivity = new UserActivity
                {
                    EventType   = "General Plan Creation",
                    UserId      = userClaims.UserId,
                    ObjectClass = "GeneralPlan",
                    ObjectId    = generalPlan.Id,
                    IpAddress   = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _generalPlanRepository.SaveChangesAsync();

                var generalPlanDto = _mapper.Map<GeneralPlanDTO>(generalPlan);
                generalPlanDto.Ministry = new MinistryPlan {Name = ministry.Name};

                return Ok(new SuccessResponse<GeneralPlanDTO>
                {
                    success = true,
                    message = "General Plan created successfully",
                    data    = generalPlanDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to get general plan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<GeneralPlanDTO>), 200)]
        public async Task<IActionResult> GetGeneralPlanById(Guid id)
        {
            try
            {
                var generalPlan = await _generalPlanRepository.GetGeneralPlanById(id);

                if (generalPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Genereal plan with id {id} not found ",
                        errors  = new { }
                    });
                }

                var generalPlanDto = _mapper.Map<GeneralPlanDTO>(generalPlan);

                return Ok(new SuccessResponse<GeneralPlanDTO>
                {
                    success = true,
                    message = "General Plan retrived successfully",
                    data    = generalPlanDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to get list of general plans. Status can be Approved, Pending and Needamendment
        /// </summary>
        /// <param name="generalPlanParameter"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetGeneralPlan")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<GeneralPlanResponse>>>), 200)]
        public async Task<IActionResult> GetGenrealPlan([FromQuery] GeneralPlanParameters generalPlanParameter)
        {
            try
            {
                var userClaims = User.UserClaims();
                var generalPlans = await _generalPlanRepository.GetGeneralPlan(generalPlanParameter);

                var prevLink = generalPlans.HasPrevious ? CreateResourceUri(generalPlanParameter, ResourceUriType.PreviousPage) : null;
                var nextLink = generalPlans.HasNext ? CreateResourceUri(generalPlanParameter, ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(generalPlanParameter, ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = generalPlans.TotalPages,
                    perPage = generalPlans.PageSize,
                    totalEntries = generalPlans.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<GeneralPlanResponse>>
                {
                    success = true,
                    message = "General plan retrieved successfully",
                    data = generalPlans,
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

        /// <summary>
        /// An endpoint to get list of procurment plans under a general plan. Status can be APPROVED, INREVIEW and DRAFT and stage can be ONGOING, COMPLETED and NOTSTARTED
        /// </summary>
        /// <param name="procurementParameter"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/procurementPlans", Name = "GetProcurmentPlan")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<ProcurementsResponse>>>), 200)]
        public async Task<IActionResult> GetProcurmentPlan(Guid id, [FromQuery] ProcurementPlanParameters procurementParameter)
        {
            try
            {
                var generalPlan = await _generalPlanRepository.ExistsAsync(x => x.Id == id);

                if (!generalPlan)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Genereal plan with id {id} not found ",
                        errors  = new { }
                    });
                }
                var userClaims = User.UserClaims();
                var procurementPlans = await _generalPlanRepository.GetProcurments(procurementParameter, id);

                var prevLink = procurementPlans.HasPrevious ? CreateProcurmentResourceUri(procurementParameter, ResourceUriType.PreviousPage) : null;
                var nextLink = procurementPlans.HasNext ? CreateProcurmentResourceUri(procurementParameter, ResourceUriType.NextPage) : null;
                var currentLink = CreateProcurmentResourceUri(procurementParameter, ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = procurementPlans.TotalPages,
                    perPage = procurementPlans.PageSize,
                    totalEntries = procurementPlans.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<ProcurementsResponse>>
                {
                    success = true,
                    message = "procurement plan retrieved successfully",
                    data = procurementPlans,
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

        /// <summary>
        /// Endpoint to summarize general plan data
        /// </summary>
        /// <returns></returns>
        [HttpGet("summary")]
        [ProducesResponseType(typeof(SuccessResponse<GeneralPlanSummaryDto>), 200)]
        public async Task<IActionResult> SummarizeGeneralPlanData([FromQuery] ProcurementPlanParameters procurementParameter)
        {
            try
            {
                var generalPlanSummary = await _generalPlanRepository.GetGeneralPlanSummary(procurementParameter);

                return Ok(new SuccessResponse<GeneralPlanSummaryDto>
                {
                    success = true,
                    message = "General plan summary retrieved successfully",
                    data    = generalPlanSummary
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to update a general plan
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<GeneralPlanDTO>), 200)]
        public async Task<IActionResult> UpdateGeneralPlan(Guid id, GeneralPlanForUpdate generalPlanForUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("id");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your general plan update request failed",
                        errors = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();

                var generalPlan = await _generalPlanRepository.GetGeneralPlanById(id);

                if (generalPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Genereal plan with id {id} not found ",
                        errors  = new { }
                    });
                }

                var isMinistryExist = await _ministryRepository.ExistsAsync(x => x.Id == generalPlanForUpdate.MinistryId);

                if (!isMinistryExist)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Ministry with id {generalPlanForUpdate.MinistryId} not found ",
                        errors = new { }
                    });
                }

                var isMinsitryGeneralPlanExist =
                    await _generalPlanRepository.ExistsAsync(x => x.MinistryId == generalPlanForUpdate.MinistryId);

                if (isMinsitryGeneralPlanExist && generalPlan.MinistryId != generalPlanForUpdate.MinistryId)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"General plan already created for this minstry",
                        errors = new { }
                    });
                }

                _mapper.Map(generalPlanForUpdate, generalPlan);

                _generalPlanRepository.Update(generalPlan);
                await _generalPlanRepository.SaveChangesAsync();

                var generalPlanDto = _mapper.Map<GeneralPlanDTO>(generalPlan);

                return Ok(new SuccessResponse<GeneralPlanDTO>
                {
                    success = true,
                    message = "General Plan updated successfully",
                    data = generalPlanDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An endpoint to approve a general plan 
        /// </summary>
        /// <param name="id"></param>
        [HttpPost("{id}/approve")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> ApproveGeneralPlan(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();

                var generalPlan = await _generalPlanRepository.GetByIdAsync(id);

                if (generalPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Genereal plan with id {id} not found ",
                        errors  = new { }
                    });
                }

                generalPlan.Status = EGeneralPlanStatus.APPROVED;
                generalPlan.UpdatedAt = DateTime.Now;
                _generalPlanRepository.Update(generalPlan);

                var userActivity = new UserActivity
                {
                    EventType = "General Plan approved",
                    UserId = userClaims.UserId,
                    ObjectClass = "General Plan",
                    ObjectId = id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);

                await _generalPlanRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "General Plan approved successfully",
                    data = new { }
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to summarize procurement plan data under a general plan
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/procurementPlanSummary")]
        [ProducesResponseType(typeof(SuccessResponse<ProcurementPlanSummaryDto>), 200)]
        public async Task<IActionResult> SummarizeProcurementPlanData(Guid id)
        {
            try
            {
                var generalPlan = await _generalPlanRepository.GetByIdAsync(id);

                if (generalPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Genereal plan with id {id} not found ",
                        errors  = new { }
                    });
                }
                var procurmentPlanSummary = await _generalPlanRepository.GetprocurementPlanSummary(id);

                return Ok(new SuccessResponse<ProcurementPlanSummaryDto>
                {
                    success = true,
                    message = "Procurement plan summary retrieved successfully",
                    data    = procurmentPlanSummary
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An endpoint to request for an amendment for a general plan 
        /// </summary>
        /// <param name="id"></param>
        [HttpPost("{id}/needAmendment")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> AmmendProcurementPlan(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();

                var generalPlan = await _generalPlanRepository.GetByIdAsync(id);
                if (generalPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"General Plan with id {id} not found",
                        errors = new { }
                    });
                }

                generalPlan.Status = EGeneralPlanStatus.NEEDAMENDMENT;
                generalPlan.UpdatedAt = DateTime.Now;
                _generalPlanRepository.Update(generalPlan);

                await _generalPlanRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "General Plan sent for amendmment successfully",
                    data = new { }
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        #region CreateResource
        private string CreateResourceUri(GeneralPlanParameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetGeneralPlan",
                        new
                        {
                            PageNumber = parameters.PageNumber - 1,
                            parameters.PageSize,
                            parameters.MinistryId,
                            parameters.Year,
                            parameters.Status,
                            parameters.Search
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetGeneralPlan",
                        new
                        {
                            PageNumber = parameters.PageNumber + 1,
                            parameters.PageSize,
                            parameters.MinistryId,
                            parameters.Year,
                            parameters.Status,
                            parameters.Search
                        });

                default:
                    return Url.Link("GetGeneralPlan",
                        new
                        {
                            parameters.PageNumber,
                            parameters.PageSize,
                            parameters.MinistryId,
                            parameters.Year,
                            parameters.Status,
                            parameters.Search
                        });
        
            }

        }
        #endregion

        #region CreateProcurmentResource
        private string CreateProcurmentResourceUri(ProcurementPlanParameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetProcurmentPlan",
                        new
                        {
                            PageNumber = parameters.PageNumber - 1,
                            parameters.PageSize,
                            parameters.MinistryId,
                            parameters.Stage,
                            parameters.Status,
                            parameters.Search
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetProcurmentPlan",
                        new
                        {
                            PageNumber = parameters.PageNumber + 1,
                            parameters.PageSize,
                            parameters.MinistryId,
                            parameters.Stage,
                            parameters.Status,
                            parameters.Search
                        });

                default:
                    return Url.Link("GetProcurmentPlan",
                        new
                        {
                            parameters.PageNumber,
                            parameters.PageSize,
                            parameters.MinistryId,
                            parameters.Stage,
                            parameters.Status,
                            parameters.Search
                        });

            }

        }
        #endregion
    }
}
