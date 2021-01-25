using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EGPS.WebAPI.Controllers
{
    [Route("api/v1/documentClasses")]
    [ApiController]
    [Authorize]
    public class DocumentClassesController : ControllerBase
    {
        private readonly IDocumentClassRepository _documentClassRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private const string USER_IP_ADDRESS = "User-IP-Address";
        public DocumentClassesController(IDocumentClassRepository documentClassRepository,
            IUserActivityRepository userActivityRepository,
            IHttpContextAccessor accessor,
            IMapper mapper)
        {
            _documentClassRepository = documentClassRepository ?? throw new ArgumentNullException(nameof(documentClassRepository));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponse<DocumentClassDTO>), 200)]
        public async Task<IActionResult> CreateDocumentClass(DocumentClassForCreationDTO documentClassForCreation)
        {
            try
            {
                var userClaims = User.UserClaims();

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your document class creation request failed",
                        errors = ModelState.Error()
                    });
                }

                var documentClassExists = _documentClassRepository.Exists(x =>
                    x.Title == documentClassForCreation.Title && x.AccountId == userClaims.AccountId);

                if (documentClassExists)
                {
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your document class creation request failed",
                        errors = new
                        {
                            title = new string[] { "Duplicate title" }
                        }
                    });
                }

                var documentClassEntity = new DocumentClass
                {
                    Title = documentClassForCreation.Title,
                    CreatedById = userClaims.UserId,
                    AccountId = userClaims.AccountId
                };

                await _documentClassRepository.AddAsync(documentClassEntity);
                await _documentClassRepository.SaveChangesAsync();

                var documentClassDTO = _mapper.Map<DocumentClassDTO>(documentClassEntity);

                var userActivity = new UserActivity
                {
                    EventType = "DocumentClass Created",
                    UserId = userClaims.UserId,
                    ObjectClass = "DOCUMENTCLASS",
                    ObjectId = documentClassEntity.Id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<DocumentClassDTO>
                {
                    success = true,
                    message = "Document class created successfully",
                    data = documentClassDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpGet(Name = "GetDocumentClasses")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<DocumentClassDTO>>>), 200)]
        public async Task<IActionResult> GetDocumentClass([FromQuery] DocumentClassParameters documentClassParameters)
        {
            try
            {
                var userClaims = User.UserClaims();
                var documentClasses = await _documentClassRepository.GetDocumentClasses(documentClassParameters, userClaims.AccountId);

                var prevLink = documentClasses.HasPrevious ? CreateResourceUri(documentClassParameters, ResourceUriType.PreviousPage) : null;
                var nextLink = documentClasses.HasNext ? CreateResourceUri(documentClassParameters, ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(documentClassParameters, ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = documentClasses.TotalPages,
                    perPage = documentClasses.PageSize,
                    totalEntries = documentClasses.TotalCount
                };

                var documentClassesDto = _mapper.Map<IEnumerable<DocumentClassDTO>>(documentClasses);

                return Ok(new PagedResponse<IEnumerable<DocumentClassDTO>>
                {
                    success = true,
                    message = "Document classes retrieved successfully",
                    data = documentClassesDto,
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<DocumentClassDTO>), 200)]
        public async Task<IActionResult> GetDocumentClassById(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();
                var documentClassEntity = await _documentClassRepository.SingleOrDefault(x =>
                    x.Id == id &&
                    x.AccountId == userClaims.AccountId);

                if (documentClassEntity == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Document class with id {id} not found",
                        errors = new { }
                    });
                }

                var documentClassDTO = _mapper.Map<DocumentClassDTO>(documentClassEntity);

                return Ok(new SuccessResponse<DocumentClassDTO>
                {
                    success = true,
                    message = "Document class retrieved successfully",
                    data = documentClassDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<DocumentClassDTO>), 200)]
        public async Task<IActionResult> UpdateDocumentClass(Guid id, DocumentClassForUpdateDTO documentClassForUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("documentClassId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Document class details update failed",
                        errors = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();
                var documentClassExists = _documentClassRepository.Exists(x =>
                    x.Title == documentClassForUpdate.Title &&
                    x.AccountId == userClaims.AccountId);

                if (documentClassExists)
                {
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your document class creation request failed",
                        errors = new
                        {
                            title = new string[] { "Duplicate title" }
                        }
                    });
                }

                var documentClassEntity = await _documentClassRepository.SingleOrDefault(x =>
                    x.Id == id &&
                    x.AccountId == userClaims.AccountId);

                if (documentClassEntity == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Document class with id {id} not found",
                        errors = new { }
                    });
                }

                documentClassEntity.Title = documentClassForUpdate.Title;
                documentClassEntity.UpdatedAt = DateTime.Now;
                _mapper.Map(documentClassForUpdate, documentClassEntity);
                _documentClassRepository.Update(documentClassEntity);
                await _documentClassRepository.SaveChangesAsync();

                var documentClassDTO = _mapper.Map<DocumentClassDTO>(documentClassEntity);

                var userActivity = new UserActivity
                {
                    EventType = "DocumentClass Updated",
                    UserId = documentClassEntity.CreatedById,
                    ObjectClass = "DOCUMENTCLASS",
                    ObjectId = documentClassEntity.Id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<DocumentClassDTO>
                {
                    success = true,
                    message = "Document class updated successfully",
                    data = documentClassDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteDocumentClass(Guid id)
        {
            var userClaims = User.UserClaims();

            var documentClass = await _documentClassRepository.SingleOrDefault(x =>
                x.AccountId == userClaims.AccountId &&
                x.Id == id);

            if (documentClass == null)
            {
                return NotFound(new ErrorResponse<object>
                {
                    success = false,
                    message = $"Document class with id {id} not found",
                    errors = new { }
                });
            }

            documentClass.Deleted = true;
            documentClass.DeletedAt = DateTime.Now;

            _documentClassRepository.Update(documentClass);
            await _documentClassRepository.SaveChangesAsync();

            var userActivity = new UserActivity
            {
                EventType = "Document class Deleted",
                UserId = userClaims.UserId,
                ObjectClass = "DOCUMENTCLASS",
                ObjectId = id,
                AccountId = userClaims.AccountId,
                IpAddress = Request.GetHeader(USER_IP_ADDRESS)
            };

            await _userActivityRepository.AddAsync(userActivity);
            await _userActivityRepository.SaveChangesAsync();

            return Ok(new SuccessResponse<object>
            {
                success = true,
                message = "Document class deleted successfully",
                data = new { }
            });
        }

        #region CreateResource
        private string CreateResourceUri(DocumentClassParameters docmentClassParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetDocumentClasses",
                        new
                        {
                            PageNumber = docmentClassParameters.PageNumber - 1,
                            docmentClassParameters.PageSize
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetDocumentClasses",
                        new
                        {
                            PageNumber = docmentClassParameters.PageNumber + 1,
                            docmentClassParameters.PageSize
                        });

                default:
                    return Url.Link("GetDocumentClasses",
                        new
                        {
                            docmentClassParameters.PageNumber,
                            docmentClassParameters.PageSize
                        });
            }

        }
        #endregion
    }
}
