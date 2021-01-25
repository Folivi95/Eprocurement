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
    [Route("api/v1/documentUpload")]
    [ApiController]
    [Authorize]
    public class DocumentUploadController : ControllerBase
    {
        private readonly IDocumentUploadService _documentUploadService;
        private readonly IProcurementPlanRepository _procurementPlanRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDocumentUploadRepository _documentUploadRepository;
        private readonly IMapper _mapper;
        private const string USER_IP_ADDRESS = "User-IP-Address";

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="documentUploadService"></param>
        /// <param name="procurementPlanRepository"></param>
        /// <param name="userActivityRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="userRepository"></param>
        /// <param name="documentUploadRepository"></param>

        public DocumentUploadController(IDocumentUploadService documentUploadService, IProcurementPlanRepository procurementPlanRepository, 
            IUserActivityRepository userActivityRepository,
            IMapper mapper,
            IUserRepository userRepository,
            IDocumentUploadRepository documentUploadRepository)
        {
            _documentUploadService = documentUploadService ?? throw new ArgumentNullException(nameof(documentUploadService));
            _procurementPlanRepository = procurementPlanRepository ?? throw new ArgumentNullException(nameof(procurementPlanRepository));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository)); 
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository)); 
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _documentUploadRepository = documentUploadRepository ?? throw new ArgumentNullException(nameof(documentUploadRepository));
        }

        /// <summary>
        /// POST endpoint to upload documents.
        /// Document Status: 1 = MANDATORY, 2 = SUPPORTING, 3 = REVIEW, 4 = PAYMENT, 5 = OTHER.
        /// Object Type: 1 = ADMIN, 2 = VENDOR
        /// </summary>
        /// <param name="documentUploadDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<DocumentDTO>>), 200)]
        public async Task<IActionResult> DocumentUpload ([FromForm] DocumentUploadDto documentUploadDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your document upload failed",
                        errors  = ModelState.Error().FilterError()
                    });
                }
                var userClaims = User.UserClaims();

                // upload document 
                var documentDto = new GenericProcurementPlanDocumentDto()
                {
                    UserId = userClaims.UserId,
                    Documents = documentUploadDTO.Documents,
                    ObjectId = documentUploadDTO.ObjectId,
                    Status = documentUploadDTO.Status,
                    ObjectType = documentUploadDTO.ObjectType
                };
            
                var documentUploaded = _documentUploadService.CreateGenericDocument(documentDto);
                await _documentUploadRepository.AddRangeAsync(documentUploaded);
                await _procurementPlanRepository.SaveChangesAsync();

                //Create, add and save new UserActivity
                var userActivity = new UserActivity
                {
                    EventType = $"Document Uploaded",
                    UserId = userClaims.UserId,
                    ObjectClass = documentUploadDTO.Status.ToString(),
                    ObjectId = documentUploadDTO.ObjectId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();


                var documentResponse = _mapper.Map<IEnumerable<DocumentDTO>>(documentUploaded);

                return Ok(new SuccessResponse<IEnumerable<DocumentDTO>>
                {
                    success = true,
                    message = "Document uploaded successfully",
                    data = documentResponse,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An ednpoint to get a paricular document with it's ID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<DocumentDTO>), 200)]
        public async Task<IActionResult> GetDocument(Guid id)
        {
            try
            {
                var procurementPlanDocument = await _documentUploadRepository.SingleOrDefault(d => d.Id == id);

                if (procurementPlanDocument == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Document with id {id} not found",
                        errors = new { }
                    });
                }

                var documentDTO = _mapper.Map<DocumentDTO>(procurementPlanDocument);


                return Ok(new SuccessResponse<DocumentDTO>
                {
                    success = true,
                    message = "Document retrieved successfully",
                    data = documentDTO
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Get all documents with an object ID.
        /// Object Type: 1 = ADMIN, 2 = VENDOR
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{objectId}/documents")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<DocumentDTO>>), 200)]
        public async Task<IActionResult> GetAllDocumentsFromObject(Guid objectId, int objectType)
        {
            try
            {
                var documents = await _documentUploadRepository.GetAllDocumentsWithObject(objectId, objectType);

                var documentsDTO = _mapper.Map<IEnumerable<DocumentDTO>>(documents);


                return Ok(new SuccessResponse<IEnumerable<DocumentDTO>>
                {
                    success = true,
                    message = "Document retrieved successfully",
                    data = documentsDTO
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Delete document by document Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<DocumentDTO>), 200)]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            try
            {
                var document = await _documentUploadRepository.SingleOrDefault(x => x.Id == id);
                if (document == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"document with id {id} not found",
                        errors = new { }
                    });
                }
                document.Deleted = true;
                _documentUploadRepository.Update(document);
                await _documentUploadRepository.SaveChangesAsync();
                
                var documentsDTO = _mapper.Map<object>(document);


                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Document deleted successfully",
                    data = new {}
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

    }

}