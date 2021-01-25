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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EGPS.WebAPI.Controllers
{
    /// <summary>
    /// Procurements controller
    /// </summary>

    [Route("api/v1/procurements")]
    [ApiController]
    [Authorize]
    public class ProcurementsController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IProcurementPlanRepository _procurementPlanRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IMinistryRepository _ministryRepository;
        private readonly IPhotoAcessor _photoAcessor;
        private readonly IProcurementService _procurementService;
        private readonly IVendorProfileRepository _vendorProfileRepository;
        private readonly IGeneralPlanRepository _generalPlanRepository;
        private readonly IVendorBidRepository _vendorBidRepository;
        private readonly IVendorProcurementRepository _vendorProcurementRepository;
        private readonly INotificationService _notificationService;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private const string USER_IP_ADDRESS = "User-IP-Address";

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userRepository"></param>
        /// <param name="procurementPlanRepository"></param>
        /// <param name="userActivityRepository"></param>
        /// <param name="ministryRepository"></param>
        /// <param name="photoAcessor"></param>
        /// <param name="procurementService"></param>
        /// <param name="vendorProfileRepository"></param>
        /// <param name="generalPlanRepository"></param>
        /// <param name="vendorBidRepository"></param>
        /// <param name="vendorProcurementRepository"></param>
        /// <param name="notificationService"></param>
        /// <param name="notificationRepository"></param>


        public ProcurementsController(IMapper mapper,
            IUserRepository userRepository,
            IProcurementPlanRepository procurementPlanRepository,
            IUserActivityRepository userActivityRepository,
            IMinistryRepository ministryRepository,
            IPhotoAcessor photoAcessor,
            IProcurementService procurementService,
            IVendorProfileRepository vendorProfileRepository,
            IGeneralPlanRepository generalPlanRepository,
            IVendorBidRepository vendorBidRepository,
            IVendorProcurementRepository vendorProcurementRepository,
            INotificationService notificationService,
            INotificationRepository notificationRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _procurementPlanRepository = procurementPlanRepository ?? throw new ArgumentNullException(nameof(procurementPlanRepository));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _ministryRepository = ministryRepository ?? throw new ArgumentNullException(nameof(ministryRepository));
            _photoAcessor = photoAcessor ?? throw new ArgumentNullException(nameof(photoAcessor));
            _procurementService = procurementService ?? throw new ArgumentNullException(nameof(procurementService));
            _vendorProfileRepository = vendorProfileRepository ?? throw new ArgumentNullException(nameof(vendorProfileRepository));
            _generalPlanRepository = generalPlanRepository ?? throw new ArgumentNullException(nameof(generalPlanRepository));
            _vendorBidRepository = vendorBidRepository ?? throw new ArgumentNullException(nameof(vendorBidRepository));
            _vendorProcurementRepository = vendorProcurementRepository ?? throw new ArgumentNullException(nameof(vendorProcurementRepository));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
        }



        /// <summary>
        /// Endpoint to create a procurement plan.
        /// </summary>
        /// <param name="generalPlanId"></param>
        /// <param name="procurementPlanModel"></param>
        /// <returns></returns>
        [HttpPost("{generalPlanId}")]
        [ProducesResponseType(typeof(SuccessResponse<ProcurementPlanDTO>), 200)]
        public async Task<IActionResult> CreateProcurementPlan(Guid generalPlanId, [FromBody]ProcurementPlanForCreationDTO procurementPlanModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("generalPlanId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your create procurement plan request failed",
                        errors = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();

                var generalPlan = await _generalPlanRepository.ExistsAsync(x => x.Id == generalPlanId);

                if (!generalPlan)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"General plan with id {generalPlanId} not found ",
                        errors = new { }
                    });
                }

                var user = await _userRepository.GetByIdAsync(userClaims.UserId);

                if (user == null || user.UserType != EUserType.STAFF)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {userClaims.UserId} not found or is not a staff",
                        errors = new { }
                    });
                }

                var ministry = await _ministryRepository.SingleOrDefault(m => m.Id == procurementPlanModel.MinistryId);

                if (ministry == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Ministry with id {procurementPlanModel.MinistryId} not found",
                        errors = new { }
                    });
                }

                var procurementMethod = await _procurementPlanRepository.GetProcurementMethod(procurementPlanModel.ProcurementMethodId);

                if (procurementMethod == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Procurement Method with id {procurementPlanModel.ProcurementMethodId} not found",
                        errors = new { }
                    });
                }

                var reviewMethod = await _procurementPlanRepository.GetReviewMethod(procurementPlanModel.ReviewMethodId);

                if (reviewMethod == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Review Method with id {procurementPlanModel.ReviewMethodId} not found",
                        errors = new { }
                    });
                }

                var qualificationMethod = await _procurementPlanRepository.GetQualificationMethod(procurementPlanModel.QualificationMethodId);

                if (qualificationMethod == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Qualification Method with id {procurementPlanModel.QualificationMethodId} not found",
                        errors = new { }
                    });
                }

                var procurementProcess = await _procurementPlanRepository.GetProcurementProcess(procurementPlanModel.ProcessTypeId);

                if (procurementProcess == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Procurement Process with id {procurementPlanModel.ProcessTypeId} not found",
                        errors = new { }
                    });
                }

                var procurementCategory = await _procurementPlanRepository.GetProcurementCategory(procurementPlanModel.ProcurementCategoryId);

                if (procurementCategory == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Procurement Category with id {procurementPlanModel.ProcurementCategoryId} not found",
                        errors = new { }
                    });
                }

                var procurementPlanDTO = new ProcurementPlanDTO();

                if (procurementPlanModel.Id != null)
                {
                    var updateProcurementPlan = await _procurementPlanRepository.GetByIdAsync(procurementPlanModel.Id.Value);

                    if (updateProcurementPlan == null)
                    {
                        return NotFound(new ErrorResponse<object>
                        {
                            success = false,
                            message = $"procurement plan with id {procurementPlanModel.Id} not found",
                            errors = new { }
                        });

                    }

                    var updateUserActivity = new UserActivity
                    {
                        EventType = "Procurement Plan Updated",
                        UserId = user.Id,
                        ObjectClass = "PROCUREMENT-PLAN",
                        ObjectId = updateProcurementPlan.Id,
                        IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                    };

                    await _userActivityRepository.AddAsync(updateUserActivity);

                    _mapper.Map(procurementPlanModel, updateProcurementPlan);
                    updateProcurementPlan.UpdatedAt = DateTime.UtcNow;
                    _procurementPlanRepository.Update(updateProcurementPlan);
                    await _procurementPlanRepository.SaveChangesAsync();
                    procurementPlanDTO = _mapper.Map<ProcurementPlanDTO>(updateProcurementPlan);

                    return Ok(new SuccessResponse<ProcurementPlanDTO>
                    {
                        success = true,
                        message = "Procurement Plan updated successfully",
                        data = procurementPlanDTO
                    });
                }

                var procurementPlan = _mapper.Map<ProcurementPlan>(procurementPlanModel);
                procurementPlan.GeneralPlanId = generalPlanId;
                procurementPlan.CreatedById = userClaims.UserId;

                await _procurementPlanRepository.AddAsync(procurementPlan);

                var userActivity = new UserActivity
                {
                    EventType = "Procurement Plan Creation",
                    UserId = user.Id,
                    ObjectClass = "PROCUREMENT-PLAN",
                    ObjectId = procurementPlan.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);

                await _procurementPlanRepository.SaveChangesAsync();
                procurementPlanDTO = _mapper.Map<ProcurementPlanDTO>(procurementPlan);


                return Ok(new SuccessResponse<ProcurementPlanDTO>
                {
                    success = true,
                    message = "Procurement Plan created successfully",
                    data = procurementPlanDTO
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// Endpoint to get a procurement plan 
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<ProcurementPlanToReturnDto>), 200)]
        public async Task<IActionResult> GetProcurementPlanById(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {userClaims.UserId} not found",
                        errors = new { }
                    });
                }

                var procurementPlan = await _procurementPlanRepository.GetProcurementPlan(id);

                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement plan with id {id} not found",
                        errors = new { }
                    });
                }

                var procurementPlanDto = _mapper.Map<ProcurementPlanToReturnDto>(procurementPlan);
                procurementPlanDto.Ministry = procurementPlan.Ministry.Name;
                procurementPlanDto.MinistryCode = procurementPlan.Ministry.Code;

                return Ok(new SuccessResponse<ProcurementPlanToReturnDto>
                {
                    success = true,
                    message = "Procurement plan retrieved successfully",
                    data = procurementPlanDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// Endpoint to soft delete a procurement plan document
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteProcurementPlan")]
        [ProducesResponseType(typeof(SuccessResponse<ProcurementPlanForDeletedDTO>), 200)]
        public async Task<IActionResult> DeleteProcurementPlan(Guid id)
        {
            try
            {
                //Get userClaims from token
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId &&
                                    u.AccountId == userClaims.AccountId
                                    && u.UserType == EUserType.STAFF);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {userClaims.UserId} not found or is not a Staff",
                        errors = new { }
                    });
                }

                var procurementPlan = await _procurementPlanRepository.FirstOrDefault(x => x.Id == id);

                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }

                procurementPlan.Deleted = true;
                procurementPlan.DeletedAt = DateTime.UtcNow;

                await _procurementPlanRepository.SaveChangesAsync();

                //Create, add and save new UserActivity
                var userActivity = new UserActivity
                {
                    EventType = $"Procurement Plan Deleted",
                    UserId = user.Id,
                    ObjectClass = "PROCUREMENT-PLAN",
                    ObjectId = procurementPlan.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                var procurementPlanDto = _mapper.Map<ProcurementPlanForDeletedDTO>(procurementPlan);

                return Ok(new SuccessResponse<ProcurementPlanForDeletedDTO>
                {
                    success = true,
                    message = "Procurement Plan deleted successfully",
                    data = procurementPlanDto
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to get all procurement plan type. Type is PROCUREMENTPLANNING and PROCUREMENTEXECUTION 
        /// </summary>
        /// <returns></returns>
        [HttpGet("procurementPlanType")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProcurementPlanTypeDTO>>), 200)]
        public async Task<IActionResult> ProcurementPlanType(string type)
        {
            try
            {
                var procurementPlanTypes = await _procurementPlanRepository.GetProcurementPlanTypes(type);
                var procurementPlanTypeDTO = _mapper.Map<IEnumerable<ProcurementPlanTypeDTO>>(procurementPlanTypes);

                return Ok(new SuccessResponse<IEnumerable<ProcurementPlanTypeDTO>>
                {
                    success = true,
                    message = "Successfully retrieved procurement plan type",
                    data = procurementPlanTypeDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }



        /// <summary>
        /// Endpoint to update a procurement plan document
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<ProcurementPlanDTO>), 200)]
        public async Task<IActionResult> UpdateProcurementPlan(Guid id, [FromBody]ProcurementPlanForUpdateDTO procurementPlanModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("id");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your update procurement plan document request failed",
                        errors = ModelState.Error()
                    });
                }


                //get user claims
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId && u.AccountId == userClaims.AccountId && u.UserType == EUserType.STAFF);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {userClaims.UserId} not found or is not staff",
                        errors = new { }
                    });
                }

                var procurementPlan = await _procurementPlanRepository.SingleOrDefault(p => p.Id == id);

                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }

                var generalPlan = await _generalPlanRepository.ExistsAsync(x => x.Id == procurementPlanModel.GeneralPlanId);

                if (!generalPlan)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"General plan with id {procurementPlanModel.GeneralPlanId} not found ",
                        errors = new { }
                    });
                }

                var ministry = await _ministryRepository.SingleOrDefault(m => m.Id == procurementPlanModel.MinistryId);

                if (ministry == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Ministry with id {procurementPlanModel.MinistryId} not found",
                        errors = new { }
                    });
                }

                var procurementMethod = await _procurementPlanRepository.GetProcurementMethod(procurementPlanModel.ProcurementMethodId);

                if (procurementMethod == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Procurement Method with id {procurementPlanModel.ProcurementMethodId} not found",
                        errors = new { }
                    });
                }

                var reviewMethod = await _procurementPlanRepository.GetReviewMethod(procurementPlanModel.ReviewMethodId);

                if (reviewMethod == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Review Method with id {procurementPlanModel.ReviewMethodId} not found",
                        errors = new { }
                    });
                }

                var qualificationMethod = await _procurementPlanRepository.GetQualificationMethod(procurementPlanModel.QualificationMethodId);

                if (qualificationMethod == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Qualification Method with id {procurementPlanModel.QualificationMethodId} not found",
                        errors = new { }
                    });
                }

                var procurementProcess = await _procurementPlanRepository.GetProcurementProcess(procurementPlanModel.ProcessTypeId);

                if (procurementProcess == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Procurement Process with id {procurementPlanModel.ProcessTypeId} not found",
                        errors = new { }
                    });
                }

                var procurementCategory = await _procurementPlanRepository.GetProcurementCategory(procurementPlanModel.ProcurementCategoryId);

                if (procurementCategory == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Procurement Category with id {procurementPlanModel.ProcurementCategoryId} not found",
                        errors = new { }
                    });
                }

                procurementPlan.MinistryId = ministry.Id;
                procurementPlan.Description = procurementPlanModel.Description;
                procurementPlan.ProcurementMethodId = procurementPlanModel.ProcurementMethodId;
                procurementPlan.EstimatedAmountInDollars = procurementPlanModel.EstimatedAmountInDollars;
                procurementPlan.EstimatedAmountInNaira = procurementPlanModel.EstimatedAmountInNaira;
                procurementPlan.ProcurementMethodId = procurementPlanModel.ProcurementMethodId;
                procurementPlan.QualificationMethodId = procurementPlanModel.QualificationMethodId;
                procurementPlan.ReviewMethodId = procurementPlanModel.ReviewMethodId;
                procurementPlan.ProcessTypeId = procurementPlanModel.ProcessTypeId;
                procurementPlan.PackageNumber = procurementPlanModel.PackageNumber;
                procurementPlan.UpdatedAt = DateTime.UtcNow;


                _procurementPlanRepository.Update(procurementPlan);
                await _procurementPlanRepository.SaveChangesAsync();

                //Create, add and save new UserActivity
                var userActivity = new UserActivity
                {
                    EventType = $"Procurement Plan Updated",
                    UserId = user.Id,
                    ObjectClass = "STAFF",
                    ObjectId = user.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                var procurementPlanDTO = _mapper.Map<ProcurementPlanDTO>(procurementPlan);

                return Ok(new SuccessResponse<ProcurementPlanDTO>
                {
                    success = true,
                    message = "Procurement Plan Updated successfully",
                    data = procurementPlanDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }



        /// <summary>
        /// An endpoint to create documents for a procurement plan activity
        /// </summary>
        /// <param name="procurementPlanActivityId"></param>
        /// <param name="procurementPlanDocumentCreation"></param>
        /// <returns></returns>
        [HttpPost("{procurementPlanActivityId}/procurementPlanDocument")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProcurementPlanDocumentDTO>>), 200)]
        public async Task<IActionResult> CreateProcurementPlanDocument(Guid procurementPlanActivityId, [FromForm]ProcurementPlanDocumentCreation procurementPlanDocumentCreation)
        {
            try
            {
                var userClaims = User.UserClaims();

                var documentExists =
                    await _procurementPlanRepository.IsProcurementPlanActivityDocumentExists(procurementPlanActivityId);

                if (!documentExists && procurementPlanDocumentCreation?.MandatoryDocument == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Mandatory documents must contain atleast a document",
                        errors = new { }
                    });
                }

                var procurementPlanDocuments = await _procurementService.CreateDocument(userClaims.UserId, procurementPlanDocumentCreation, procurementPlanActivityId);

                await _procurementPlanRepository.AddProcurementPlanDocument(procurementPlanDocuments);

                await _procurementPlanRepository.SaveChangesAsync();

                if (procurementPlanDocuments.Count() == 0)
                {
                    procurementPlanDocuments = await _procurementPlanRepository.GetProcurementPlanDocumentWithObjectType(procurementPlanActivityId, procurementPlanDocumentCreation.ObjectType);
                }

                var procurementPlanDocumentDto = _mapper.Map<IEnumerable<ProcurementPlanDocumentDTO>>(procurementPlanDocuments);

                return Ok(new SuccessResponse<IEnumerable<ProcurementPlanDocumentDTO>>
                {
                    success = true,
                    message = "Document added successfully",
                    data = procurementPlanDocumentDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }




        /// <summary>
        /// An endpoint to create a review for a procurement plan activity
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="reviewForCreation"></param>
        /// <returns></returns>
        [HttpPost("{objectId}/review")]
        [ProducesResponseType(typeof(SuccessResponse<ReviewResponse>), 200)]
        public async Task<IActionResult> CreateReview(Guid objectId, ReviewForCreation reviewForCreation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("objectId");
                    ModelState.Remove("Document");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your review creation request failed",
                        errors = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();

                var review = _mapper.Map<Review>(reviewForCreation);
                review.CreatedById = userClaims.UserId;
                review.ObjectId = objectId;

                await _procurementPlanRepository.AddProcurementPlanReview(review);
                await _procurementPlanRepository.SaveChangesAsync();

                var reviewById = await _procurementPlanRepository.GetProcurementReviewById(review.Id);

                var reviewResponse = _mapper.Map<ReviewResponse>(reviewById);

                return Ok(new SuccessResponse<ReviewResponse>
                {
                    success = true,
                    message = "Review created successfully",
                    data = reviewResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An endpoint to get the reviews for a procurement plan activity
        /// </summary>
        /// <returns></returns>
        [HttpGet("{objectId}/review")]
        [ProducesResponseType(typeof(SuccessResponse<ReviewResponse>), 200)]
        public async Task<IActionResult> GetProcurementPlanActivityReviews(Guid objectId)
        {
            try
            {
                var userClaims = User.UserClaims();

                var review = await _procurementPlanRepository.GetProcurementPlanActivityReview(objectId);

                var reviewResponse = _mapper.Map<IEnumerable<ReviewResponse>>(review);

                return Ok(new SuccessResponse<IEnumerable<ReviewResponse>>
                {
                    success = true,
                    message = "Reviews fetched successfully",
                    data = reviewResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }



        /// <summary>
        /// An endpoint to create a notice information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="noticeInformationForCreation"></param>
        /// <returns></returns>
        [HttpPost("{id}/noticeInformation")]
        [ProducesResponseType(typeof(SuccessResponse<NoticeInformationResponse>), 200)]
        public async Task<IActionResult> CreateNoticeInformation(Guid id, NoticeInformationForCreation noticeInformationForCreation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("procurementPlanActivityId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your  notice information creation request failed",
                        errors = ModelState.Error()
                    });
                }
                var userClaims = User.UserClaims();
                var procurementPlanExist = await _procurementPlanRepository.IsProcurementPlanExists(id);
                if (!procurementPlanExist)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }

                var NoticeInformationResponse = new NoticeInformationResponse();

                if (noticeInformationForCreation.Id != null)
                {
                    var noticeInformationToUpdate = await _procurementPlanRepository.GetNoticeInformationForProcurementPlan(id);
                    if (noticeInformationToUpdate == null)
                    {
                        return NotFound(new ErrorResponse<object>
                        {
                            success = false,
                            message = $"Notice Information with id {noticeInformationForCreation.Id} not found",
                            errors = new { }
                        });
                    }

                    _mapper.Map(noticeInformationForCreation, noticeInformationToUpdate);

                    _procurementPlanRepository.UpdateNoticeInformation(noticeInformationToUpdate);
                    NoticeInformationResponse = _mapper.Map<NoticeInformationResponse>(noticeInformationToUpdate);
                    await _procurementPlanRepository.SaveChangesAsync();

                    return Ok(new SuccessResponse<NoticeInformationResponse>
                    {
                        success = true,
                        message = "Notice information updated successfully",
                        data = NoticeInformationResponse
                    });
                }
                var noticeInfoExists = await _procurementPlanRepository.GetNoticeInformationForProcurementPlan(id);
                if (noticeInfoExists != null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {id} already has a notice information",
                        errors = new { }
                    });
                }
                var NoticeInformation = _mapper.Map<NoticeInformation>(noticeInformationForCreation);
                NoticeInformation.ProcurementPlanId = id;
                NoticeInformation.CreatedById = userClaims.UserId;
                await _procurementPlanRepository.AddNoticeInformation(NoticeInformation);
                NoticeInformationResponse = _mapper.Map<NoticeInformationResponse>(NoticeInformation);


                await _procurementPlanRepository.SaveChangesAsync();
                return Ok(new SuccessResponse<NoticeInformationResponse>
                {
                    success = true,
                    message = "Notice information created successfully",
                    data = NoticeInformationResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An endpoint to update a notice information 
        /// </summary>
        /// <param name="noticeInformationId"></param>
        /// <param name="noticeInformationForUpdateDto"></param>
        /// <returns></returns>
        [HttpPut("{noticeInformationId}/noticeInformation")]
        [ProducesResponseType(typeof(SuccessResponse<NoticeInformationResponse>), 200)]
        public async Task<IActionResult> UpdateNoticeInformation(Guid noticeInformationId, NoticeInformationForUpdate noticeInformationForUpdateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("noticeInformationId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your  notice information update request failed",
                        errors = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();

                var noticeInformationToUpdate = await _procurementPlanRepository.GetNoticeInformation(noticeInformationId);
                if (noticeInformationToUpdate == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"notice information with id {noticeInformationId} not found",
                        errors = new { }
                    });
                }

                // var NoticeInformationForUpdate = _mapper.Map<NoticeInformation>(noticeInformationForCreation);

                _mapper.Map(noticeInformationForUpdateDto, noticeInformationToUpdate);
                noticeInformationToUpdate.UpdatedAt = DateTime.UtcNow;

                _procurementPlanRepository.UpdateNoticeInformation(noticeInformationToUpdate);
                await _procurementPlanRepository.SaveChangesAsync();


                var NoticeInformationResponse = _mapper.Map<NoticeInformationResponse>(noticeInformationToUpdate);

                return Ok(new SuccessResponse<NoticeInformationResponse>
                {
                    success = true,
                    message = "Notice information updated successfully",
                    data = NoticeInformationResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to get procurement plan notice information
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}/noticeInformation")]
        [ProducesResponseType(typeof(SuccessResponse<NoticeInformationResponse>), 200)]
        public async Task<IActionResult> GetNoticeInformation(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();

                var procurementPlan = await _procurementPlanRepository.GetProcurementPlan(id);
                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }

                var noticeInformation = await _procurementPlanRepository.GetNoticeInformationForProcurementPlan(id);

                if (noticeInformation == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Notice information for procurement Plan Activity with id {id} not found",
                        errors = new { }
                    });
                }
                var noticeInformationResponse = _mapper.Map<NoticeInformationResponse>(noticeInformation);

                var draftBiddingProcuremntActivity = procurementPlan.ProcurementPlanActivities.SingleOrDefault(x => x.Title.ToLower() == "draft bidding document");

                if (draftBiddingProcuremntActivity != null)
                {
                    var documents = await _procurementPlanRepository.GetProcurementPlanDocument(draftBiddingProcuremntActivity.Id);
                    var documentDto = _mapper.Map<IEnumerable<ProcurementPlanDocumentDTO>>(documents);
                    noticeInformationResponse.Documents = documentDto;
                }

                var procurementPlanDTO = _mapper.Map<ProcurementPlanForNoticeInformationDTO>(procurementPlan);
                noticeInformationResponse.ProcurementPlan = procurementPlanDTO;

                return Ok(new SuccessResponse<NoticeInformationResponse>
                {
                    success = true,
                    message = "Notice information retrieved successfully",
                    data = noticeInformationResponse
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to approve a procurement plan activity. Object Type: 1 = ADMIN, 2 = VENDOR
        /// </summary>
        /// <param name="id"></param>
        /// <param name="procurementPlanActivityId"></param>
        [HttpPost("{id}/approve/{procurementPlanActivityId}")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> ApproveProcurementPlanActivity(Guid id, Guid procurementPlanActivityId)
        {
            try
            {
                var procurementPlan = await _procurementPlanRepository.GetProcurementPlan(id);
                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }

                var currentProcurementPlanActivity = procurementPlan.ProcurementPlanActivities.SingleOrDefault(x => x.Id == procurementPlanActivityId);

                if (currentProcurementPlanActivity == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan Activity with id {procurementPlanActivityId} not found",
                        errors = new { }
                    });
                }
                currentProcurementPlanActivity.ProcurementPlanActivityStatus = EProcurementPlanActivityStatus.APPROVED;
                currentProcurementPlanActivity.UpdatedAt = DateTime.Now;
                _procurementPlanRepository.UpdateProcurementPlanActivity(currentProcurementPlanActivity);
                var currentPlanActivityIndex = currentProcurementPlanActivity.Index;

                switch (currentProcurementPlanActivity.ProcurementPlanType)
                {
                    case EPprocurementPlanTask.PROCUREMENTPLANNING:
                        var noOfPlanActivitiesForProcurementPlanning = procurementPlan.ProcurementPlanActivities.Where(x => x.ProcurementPlanType == EPprocurementPlanTask.PROCUREMENTPLANNING).Count();
                        if (currentPlanActivityIndex < noOfPlanActivitiesForProcurementPlanning)
                        {
                            var nextPlanActivityIndex = currentPlanActivityIndex + 1;
                            var nextProcurementPlanActivity = procurementPlan.ProcurementPlanActivities.SingleOrDefault(x => x.Index == nextPlanActivityIndex && x.ProcurementPlanType == EPprocurementPlanTask.PROCUREMENTPLANNING);

                            nextProcurementPlanActivity.ProcurementPlanActivityStatus = EProcurementPlanActivityStatus.PENDING;
                            nextProcurementPlanActivity.UpdatedAt = DateTime.Now;
                            _procurementPlanRepository.UpdateProcurementPlanActivity(nextProcurementPlanActivity);
                        }
                        if (currentProcurementPlanActivity.Title.ToLower() == "bid invitation")
                        {
                            var bidExpiryDate = await _procurementPlanRepository.GetBidExpiryDate(currentProcurementPlanActivity.Id);
                            var interestedBids = await _procurementPlanRepository.GetInterestedVendorBids(procurementPlan.Id);
                            foreach (var bid in interestedBids)
                            {
                                bid.ExpiryDate = bidExpiryDate;
                                bid.Type = EVendorContractStatus.NOTSTARTED;
                                _vendorBidRepository.Update(bid);
                            }
                        }
                        break;
                    case EPprocurementPlanTask.PROCUREMENTEXECUTION:
                        var noOfPlanActivitiesForProcurementExcution = procurementPlan.ProcurementPlanActivities.Where(x => x.ProcurementPlanType == EPprocurementPlanTask.PROCUREMENTEXECUTION).Count();
                        if (currentPlanActivityIndex < noOfPlanActivitiesForProcurementExcution && currentProcurementPlanActivity.Title.ToLower() != "contract signing")
                        {
                            var nextPlanActivityIndex = currentPlanActivityIndex + 1;
                            var nextProcurementPlanActivity = procurementPlan.ProcurementPlanActivities.SingleOrDefault(x => x.Index == nextPlanActivityIndex && x.ProcurementPlanType == EPprocurementPlanTask.PROCUREMENTEXECUTION);

                            nextProcurementPlanActivity.ProcurementPlanActivityStatus = EProcurementPlanActivityStatus.PENDING;
                            nextProcurementPlanActivity.UpdatedAt = DateTime.Now;
                            _procurementPlanRepository.UpdateProcurementPlanActivity(nextProcurementPlanActivity);
                        }

                        if (currentProcurementPlanActivity.Title.ToLower() == "contract signing")
                        {
                            var contract = await _procurementPlanRepository.GetAcceptedContract(procurementPlan.Id);
                            contract.IsUploaded = true;
                        }
                        break;
                }

                await _procurementPlanRepository.SaveChangesAsync();
                var t = Task.Run(async () =>
                {
                    await _notificationRepository.LogProcurementNotification(procurementPlanActivityId);
                });
                t.Wait();


                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = $"Procurement Plan Activity approved succesfully",
                    data = new { }
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }



        /// <summary>
        /// An endpoint to get list of documents for a procurement plan activity. Object Type: 1 = ADMIN, 2 = VENDOR
        /// </summary>
        /// <param name="procurementPlanActivityId"></param>
        /// <param name="objectType"></param>
        [HttpGet("{procurementPlanActivityId}/procurementPlanDocument")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProcurementPlanDocumentDTO>>), 200)]
        public async Task<IActionResult> GetProcurementPlanDocument(Guid procurementPlanActivityId, EDocumentObjectType? objectType)
        {
            try
            {
                var userClaims = User.UserClaims();

                var procurementPlanActivityExist = await _procurementPlanRepository.IsProcurementPlanActivityExist(procurementPlanActivityId);
                if (!procurementPlanActivityExist)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan Activity with id {procurementPlanActivityId} not found",
                        errors = new { }
                    });
                }

                var procurementPlanDocument = await _procurementPlanRepository.GetProcurementPlanDocumentWithObjectType(procurementPlanActivityId, objectType);

                var procurementPlanDocumentDto = _mapper.Map<IEnumerable<ProcurementPlanDocumentDTO>>(procurementPlanDocument);

                return Ok(new SuccessResponse<IEnumerable<ProcurementPlanDocumentDTO>>
                {
                    success = true,
                    message = "Documents retrieved successfully",
                    data = procurementPlanDocumentDto
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }



        /// <summary>
        /// An endpoint to create documents for a contract award decision
        /// </summary>
        /// <param name="procurementPlanActivityId"></param>
        /// <param name="contractAwardDocumentCreation"></param>
        /// <returns></returns>
        [HttpPost("{procurementPlanActivityId}/contractAwardDecisionDocument")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProcurementPlanDocumentDTO>>), 200)]
        public async Task<IActionResult> CreateDocumentForContractAward(Guid procurementPlanActivityId, [FromForm]ContractAwardDocumentCreation contractAwardDocumentCreation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("procurementPlanActivityId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your contract award document request failed",
                        errors = ModelState.Error()
                    });
                }

                if (contractAwardDocumentCreation.EndDate != null)
                {
                    var isVerifyDate = _procurementService.VerifyDate(contractAwardDocumentCreation.IssuedDate, contractAwardDocumentCreation.EndDate.Value);

                    if (!isVerifyDate)
                    {
                        return BadRequest(new ErrorResponse<object>
                        {
                            success = false,
                            message = "EndDate cannot be less than IssueDate",
                            errors = new { }
                        });
                    }
                }
                var userClaims = User.UserClaims();

                var procurementPlanActivityExist = await _procurementPlanRepository.IsProcurementPlanActivityExist(procurementPlanActivityId);
                if (!procurementPlanActivityExist)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan Activity with id {procurementPlanActivityId} not found",
                        errors = new { }
                    });
                }
                var documentExists =
                    await _procurementPlanRepository.IsProcurementPlanActivityDocumentExists(procurementPlanActivityId);

                if (!documentExists && contractAwardDocumentCreation.Documents?.MandatoryDocument == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Mandatory documents must contain atleast a document",
                        errors = new { }
                    });
                }

                var procurementPlanDocuments = await _procurementService.CreateDocument(userClaims.UserId, contractAwardDocumentCreation.Documents, procurementPlanActivityId);

                await _procurementPlanRepository.AddProcurementPlanDocument(procurementPlanDocuments);

                if (procurementPlanDocuments.Count() == 0)
                {
                    procurementPlanDocuments = await _procurementPlanRepository.GetProcurementPlanDocumentWithObjectType(procurementPlanActivityId, contractAwardDocumentCreation.Documents?.ObjectType);
                }

                var datasheet = new Datasheet();
                if (documentExists)
                {
                    datasheet = await _procurementPlanRepository.GetProcurementDatasheet(procurementPlanActivityId);
                    datasheet.StartDate = contractAwardDocumentCreation.IssuedDate;
                    datasheet.SubmissionDeadline = contractAwardDocumentCreation.EndDate.HasValue ? contractAwardDocumentCreation.EndDate.Value : datasheet.SubmissionDeadline;
                    datasheet.UpdatedAt = DateTime.UtcNow;

                    _procurementPlanRepository.UpdateDatasheet(datasheet);
                    await _procurementPlanRepository.SaveChangesAsync();
                }
                else
                {
                    datasheet.StartDate = contractAwardDocumentCreation.IssuedDate;
                    datasheet.SubmissionDeadline = contractAwardDocumentCreation.EndDate;
                    datasheet.ProcurementPlanActivityId = procurementPlanActivityId;
                    datasheet.CreatedById = userClaims.UserId;

                    await _procurementPlanRepository.AddDatasheet(datasheet);
                    await _procurementPlanRepository.SaveChangesAsync();
                }
                var procurementPlanDocumentDto = _mapper.Map<IEnumerable<ProcurementPlanDocumentDTO>>(procurementPlanDocuments);


                var contractAwardDocumentDTO = new ContractAwardDocumentDTO
                {
                    Documents = procurementPlanDocumentDto,
                    Datasheet = _mapper.Map<ContractAwardDatasheet>(datasheet)
                };

                return Ok(new SuccessResponse<ContractAwardDocumentDTO>
                {
                    success = true,
                    message = "Contract Award Document added successfully",
                    data = contractAwardDocumentDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }



        /// <summary>
        /// An endpoint to get document for contract award decision
        /// </summary>
        /// <param name="procurementPlanActivityId"></param>
        /// <param name="objectType"></param>
        [HttpGet("{procurementPlanActivityId}/contractAwardDecisionDocument")]
        [ProducesResponseType(typeof(SuccessResponse<ContractAwardDocumentResponse>), 200)]
        public async Task<IActionResult> GetDocumentForContractAward(Guid procurementPlanActivityId, EDocumentObjectType? objectType)
        {
            try
            {
                var userClaims = User.UserClaims();

                var procurementPlanActivityExist = await _procurementPlanRepository.IsProcurementPlanActivityExist(procurementPlanActivityId);
                if (!procurementPlanActivityExist)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan Activity with id {procurementPlanActivityId} not found",
                        errors = new { }
                    });
                }

                var procurementPlanDocument = await _procurementPlanRepository.GetProcurementPlanDocumentWithObjectType(procurementPlanActivityId, objectType);

                var datasheet = await _procurementPlanRepository.GetProcurementDatasheet(procurementPlanActivityId);

                var contractAwardResponse = new ContractAwardDocumentResponse
                {
                    Documents = _mapper.Map<IEnumerable<ProcurementPlanDocumentDTO>>(procurementPlanDocument),
                    Datasheet = _mapper.Map<ContractAwardDatasheet>(datasheet)
                };

                return Ok(new SuccessResponse<ContractAwardDocumentResponse>
                {
                    success = true,
                    message = "Contract Award Documents retrieved successfully",
                    data = contractAwardResponse
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An endpoint to upload documents and create datasheet
        /// </summary>
        /// <param name="procurementPlanActivityId"></param>
        /// <param name="uploadDocumentAndDatasheet"></param>
        /// <returns></returns>
        [HttpPost("{procurementPlanActivityId}/documentDatasheet")]
        [ProducesResponseType(typeof(SuccessResponse<DocumentDatasheetDTO>), 200)]
        public async Task<IActionResult> CreateDocumentAndDatasheet(Guid procurementPlanActivityId, [FromForm] DocumentDatasheetCreation uploadDocumentAndDatasheet)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("procurementPlanActivityId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your document datasheetrequest failed",
                        errors = ModelState.Error()
                    });
                }
                var userClaims = User.UserClaims();

                var procurementPlanActivityExist = await _procurementPlanRepository.IsProcurementPlanActivityExist(procurementPlanActivityId);
                if (!procurementPlanActivityExist)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan Activity with id {procurementPlanActivityId} not found",
                        errors = new { }
                    });
                }

                var documentExists =
                    await _procurementPlanRepository.IsProcurementPlanActivityDocumentExists(procurementPlanActivityId);

                if (!documentExists && uploadDocumentAndDatasheet.Documents?.MandatoryDocument == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Mandatory documents must contain atleast a document",
                        errors = new { }
                    });
                }

                var procurementPlanDocuments = await _procurementService.CreateDocument(userClaims.UserId, uploadDocumentAndDatasheet.Documents, procurementPlanActivityId);

                await _procurementPlanRepository.AddProcurementPlanDocument(procurementPlanDocuments);

                if (procurementPlanDocuments.Count() == 0)
                {
                    procurementPlanDocuments = await _procurementPlanRepository.GetProcurementPlanDocumentWithObjectType(procurementPlanActivityId, uploadDocumentAndDatasheet.Documents?.ObjectType);
                }

                var procurementPlanDocumentDto = _mapper.Map<IEnumerable<ProcurementPlanDocumentDTO>>(procurementPlanDocuments);

                var datasheet = new Datasheet();
                if (documentExists)
                {
                    datasheet = await _procurementPlanRepository.GetProcurementDatasheet(procurementPlanActivityId);
                    datasheet.SubmissionDeadline = uploadDocumentAndDatasheet.SubmissionDeadline;
                    datasheet.UpdatedAt = DateTime.UtcNow;

                    _procurementPlanRepository.UpdateDatasheet(datasheet);
                    await _procurementPlanRepository.SaveChangesAsync();
                }
                else
                {
                    datasheet.SubmissionDeadline = uploadDocumentAndDatasheet.SubmissionDeadline;
                    datasheet.ProcurementPlanActivityId = procurementPlanActivityId;
                    datasheet.CreatedById = userClaims.UserId;

                    await _procurementPlanRepository.AddDatasheet(datasheet);
                    await _procurementPlanRepository.SaveChangesAsync();
                }


                var procurementPlanDocumentAndDatasheetDto = new DocumentDatasheetDTO()
                {
                    Documents = procurementPlanDocumentDto,
                    Datasheet = _mapper.Map<DocumentDatasheet>(datasheet)
                };

                return Ok(new SuccessResponse<DocumentDatasheetDTO>
                {
                    success = true,
                    message = "Procurement Plan Document and Datasheet added successfully",
                    data = procurementPlanDocumentAndDatasheetDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to upload documents and create datasheet for contract signing
        /// </summary>
        /// <param name="procurementPlanActivityId"></param>
        /// <param name="contractSigningDocumentAndDatasheetCreation"></param>
        /// <returns></returns>
        [HttpPost("{procurementPlanActivityId}/contractSigningDocument")]
        [ProducesResponseType(typeof(SuccessResponse<ContractSigningDocumentAndDatasheetDTO>), 200)]
        public async Task<IActionResult> CreateContractSigningDocumentAndDatasheet(Guid procurementPlanActivityId, [FromForm] ContractSigningDocumentAndDatasheetCreation contractSigningDocumentAndDatasheetCreation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("procurementPlanActivityId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your contract signing request failed",
                        errors = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();

                var procurementPlanActivityExist = await _procurementPlanRepository.IsProcurementPlanActivityExist(procurementPlanActivityId);
                if (!procurementPlanActivityExist)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan Activity with id {procurementPlanActivityId} not found",
                        errors = new { }
                    });
                }

                var documentExists =
                    await _procurementPlanRepository.IsProcurementPlanActivityDocumentExists(procurementPlanActivityId);

                if (!documentExists && contractSigningDocumentAndDatasheetCreation.Documents?.MandatoryDocument == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Mandatory documents must contain atleast a document",
                        errors = new { }
                    });
                }

                var procurementPlanDocuments = await _procurementService.CreateDocument(userClaims.UserId, contractSigningDocumentAndDatasheetCreation.Documents, procurementPlanActivityId);

                await _procurementPlanRepository.AddProcurementPlanDocument(procurementPlanDocuments);

                if (procurementPlanDocuments.Count() == 0)
                {
                    procurementPlanDocuments = await _procurementPlanRepository.GetProcurementPlanDocumentWithObjectType(procurementPlanActivityId, contractSigningDocumentAndDatasheetCreation.Documents?.ObjectType);
                }

                var datasheet = new Datasheet();
                if (documentExists)
                {
                    datasheet = await _procurementPlanRepository.GetProcurementDatasheet(procurementPlanActivityId);
                    datasheet.SignatureDate = contractSigningDocumentAndDatasheetCreation.SignatureDate;
                    datasheet.Reference = contractSigningDocumentAndDatasheetCreation.Reference ?? datasheet.Reference;
                    datasheet.SubmissionDeadline = contractSigningDocumentAndDatasheetCreation.SubmissionDeadline;
                    datasheet.Description = contractSigningDocumentAndDatasheetCreation.Description ?? datasheet.Description;
                    datasheet.UpdatedAt = DateTime.UtcNow;

                    _procurementPlanRepository.UpdateDatasheet(datasheet);
                    await _procurementPlanRepository.SaveChangesAsync();
                }
                else
                {
                    datasheet.SignatureDate = contractSigningDocumentAndDatasheetCreation.SignatureDate;
                    datasheet.Reference = contractSigningDocumentAndDatasheetCreation.Reference;
                    datasheet.Description = contractSigningDocumentAndDatasheetCreation.Description;
                    datasheet.SubmissionDeadline = contractSigningDocumentAndDatasheetCreation.SubmissionDeadline;
                    datasheet.ProcurementPlanActivityId = procurementPlanActivityId;
                    datasheet.CreatedById = userClaims.UserId;

                    await _procurementPlanRepository.AddDatasheet(datasheet);
                    await _procurementPlanRepository.SaveChangesAsync();
                }


                var procurementPlanDocumentDto = _mapper.Map<IEnumerable<ProcurementPlanDocumentDTO>>(procurementPlanDocuments);

                var contractSigningDocumentAndDatasheetDTO = new ContractSigningDocumentAndDatasheetDTO()
                {
                    Documents = procurementPlanDocumentDto,
                    Datasheet = _mapper.Map<ContractSigningDatasheet>(datasheet)
                };

                return Ok(new SuccessResponse<ContractSigningDocumentAndDatasheetDTO>
                {
                    success = true,
                    message = "Contract signing Document and Datasheet added successfully",
                    data = contractSigningDocumentAndDatasheetDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to get document datasheet
        /// </summary>
        /// <param name="procurementPlanActivityId"></param>
        /// <param name="objectType"></param>
        [HttpGet("{procurementPlanActivityId}/documentDatasheet")]
        [ProducesResponseType(typeof(SuccessResponse<DocumentDatasheetResponse>), 200)]
        public async Task<IActionResult> GetDocumentDatasheet(Guid procurementPlanActivityId, EDocumentObjectType? objectType)
        {
            try
            {
                var userClaims = User.UserClaims();

                var procurementPlanActivityExist = await _procurementPlanRepository.IsProcurementPlanActivityExist(procurementPlanActivityId);
                if (!procurementPlanActivityExist)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan Activity with id {procurementPlanActivityId} not found",
                        errors = new { }
                    });
                }

                var procurementPlanDocument = await _procurementPlanRepository.GetProcurementPlanDocumentWithObjectType(procurementPlanActivityId, objectType);

                var datasheet = await _procurementPlanRepository.GetProcurementDatasheet(procurementPlanActivityId);

                var response = new DocumentDatasheetResponse
                {
                    Documents = _mapper.Map<IEnumerable<ProcurementPlanDocumentDTO>>(procurementPlanDocument),
                    Datasheet = _mapper.Map<DocumentDatasheet>(datasheet)
                };

                return Ok(new SuccessResponse<DocumentDatasheetResponse>
                {
                    success = true,
                    message = "Document datasheets retrieved successfully",
                    data = response
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to get document for contract signing
        /// </summary>
        /// <param name="procurementPlanActivityId"></param>
        /// <param name="objectType"></param>
        [HttpGet("{procurementPlanActivityId}/contractSigningDocument")]
        [ProducesResponseType(typeof(SuccessResponse<ContractSigningDocumentResponse>), 200)]
        public async Task<IActionResult> GetDocumentForContractSigning(Guid procurementPlanActivityId, EDocumentObjectType? objectType)
        {
            try
            {
                var userClaims = User.UserClaims();

                var procurementPlanActivityExist = await _procurementPlanRepository.IsProcurementPlanActivityExist(procurementPlanActivityId);
                if (!procurementPlanActivityExist)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan Activity with id {procurementPlanActivityId} not found",
                        errors = new { }
                    });
                }

                var procurementPlanDocument = await _procurementPlanRepository.GetProcurementPlanDocumentWithObjectType(procurementPlanActivityId, objectType);

                var datasheet = await _procurementPlanRepository.GetProcurementDatasheet(procurementPlanActivityId);

                var contractAwardResponse = new ContractSigningDocumentResponse
                {
                    Documents = _mapper.Map<IEnumerable<ProcurementPlanDocumentDTO>>(procurementPlanDocument),
                    Datasheet = _mapper.Map<ContractSigningDatasheet>(datasheet)
                };

                return Ok(new SuccessResponse<ContractSigningDocumentResponse>
                {
                    success = true,
                    message = "Contract Signing Documents retrieved successfully",
                    data = contractAwardResponse
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An endpoint to create bid evaluation work-flow
        /// </summary>
        /// <param name="procurementPlanActivityId"></param>
        /// <param name="bidEvaluationForCreation"></param>
        /// <returns></returns>
        [HttpPost("{procurementPlanActivityId}/bidEvaluationWorkflow")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<BidEvaluationResponse>>), 200)]
        public async Task<IActionResult> CreateBidEvaluationWorkflow(Guid procurementPlanActivityId, BidEvaluationForCreation bidEvaluationForCreation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("procurementPlanActivityId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your bid evaluation request failed",
                        errors = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();

                var procurementPlanActivity = await _procurementPlanRepository.GetProcurementPlanActivity(procurementPlanActivityId);
                if (procurementPlanActivity == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan Activity with id {procurementPlanActivityId} not found",
                        errors = new { }
                    });
                }

                // await _procurementPlanRepository.RemoveBidVendors(bidEvaluationForCreation.RemovedVendors);

                var proccessingBids = await _procurementPlanRepository.GetProccessingBidVendors(bidEvaluationForCreation.RemovedVendors);

                foreach (var bid in proccessingBids)
                {
                    bid.Type = EVendorContractStatus.REJECTED;
                    _vendorBidRepository.Update(bid);
                }

                foreach (var vendor in bidEvaluationForCreation.AddedVendors)
                {
                    var userExists = await _userRepository.ExistsAsync(u => u.Id == vendor && u.UserType == EUserType.VENDOR);
                    if (userExists)
                    {
                        var isBidVendor = await _procurementPlanRepository.IsBidVendor(vendor.Value, procurementPlanActivity.ProcurementPlanId);

                        if (isBidVendor)
                        {
                            var vendorBid = await _vendorBidRepository.GetVendorBid(vendor.Value, procurementPlanActivity.ProcurementPlanId);
                            vendorBid.Type = EVendorContractStatus.PROCESSING;
                            _vendorBidRepository.Update(vendorBid);
                        }
                        else
                        {
                            var newVendorBid = new VendorBid()
                            {
                                VendorId = vendor.Value,
                                ProcurementPlanId = procurementPlanActivity.ProcurementPlanId,
                                Type = EVendorContractStatus.PROCESSING
                            };
                            await _vendorBidRepository.AddAsync(newVendorBid);
                        }
                    }
                }
                await _procurementPlanRepository.SaveChangesAsync();

                var bidVendors = await _procurementPlanRepository.GetBidVendors(procurementPlanActivity.ProcurementPlanId);

                var vendorProfiles = new List<VendorProfile>();

                foreach (var vendor in bidVendors)
                {
                    var vendorProfile = await _vendorProfileRepository.GetVendorProfile(vendor);

                    vendorProfiles.Add(vendorProfile);
                }

                var vendorProfileDto = _mapper.Map<List<BidEvaluationVendorDto>>(vendorProfiles);

                //var procurementPlanDocumentDto = _mapper.Map<IEnumerable<ProcurementPlanDocumentDTO>>(procurementPlanDocuments);


                var bidEvaluationResponse = new BidEvaluationResponse()
                {
                    //Documents = procurementPlanDocumentDto,
                    Vendors = vendorProfileDto
                };

                return Ok(new SuccessResponse<BidEvaluationResponse>
                {
                    success = true,
                    message = "vendors bid list updated successfully",
                    data = bidEvaluationResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An endpoint to get all vendors interested in a procurement
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}/vendors")]
        [ProducesResponseType(typeof(SuccessResponse<BidEvaluationVendorDto>), 200)]
        public async Task<IActionResult> GetProcurementPlanVendors(Guid id)

        {
            try
            {
                var userClaims = User.UserClaims();

                var procurementPlanExist = await _procurementPlanRepository.IsProcurementPlanExists(id);
                if (!procurementPlanExist)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }

                var indicatedVendors = await _procurementPlanRepository.GetIndicatedVendors(id);

                var vendorProfiles = new List<VendorProfile>();

                foreach (var vendor in indicatedVendors)
                {
                    var vendorProfile = await _vendorProfileRepository.GetVendorProfile(vendor);

                    if (vendorProfile != null)
                    {
                        vendorProfiles.Add(vendorProfile);
                    }
                }

                var vendorProfileDto = _mapper.Map<List<BidEvaluationVendorDto>>(vendorProfiles);

                return Ok(new SuccessResponse<List<BidEvaluationVendorDto>>
                {
                    success = true,
                    message = "Vendors retrieved successfully",
                    data = vendorProfileDto
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }



        /// <summary>
        /// An endpoint to create procurement plan activities. ProcurementPlanType can only be PROCUREMENTPLANNING or PROCUREMENTEXECUTION
        /// </summary>
        /// <param name="id"></param>
        ///  <param name="activityId"></param>
        /// <param name="activity"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{id}/activities/{activityId}")]
        [ProducesResponseType(typeof(SuccessResponse<ProcurementActivityDTO>), 200)]
        public async Task<IActionResult> EditProcurementActivityAsync([FromRoute]Guid id, [FromRoute]Guid activityId, [FromBody]ActivityForEdit activity)
        {
            try
            {

                if (activity.StartDate.Date > activity.EndDate.Date)
                    return BadRequest(new ErrorResponse<string>
                    {
                        success = false,
                        message = "Start date can not be greater than end date",
                        errors = ""
                    });

                if (!ModelState.IsValid)
                {
                    ModelState.Remove("id");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your procurement plan activities update request failed",
                        errors = ModelState.Error()
                    });
                }

                var procurementPlan = await _procurementPlanRepository.GetByIdAsync(id);
                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }

                var activ = await _procurementPlanRepository.GetProcurementPlanActivity(activityId);

                if (activ == null)
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Activity Plan with id {id} not found",
                        errors = new { }
                    });

                if (activity.StartDate != DateTime.MinValue)
                    activ.StartDate = activity.StartDate;
                if (activity.EndDate != DateTime.MinValue)
                    activ.EndDate = activity.EndDate;

                activ.Title = activity.Title ?? activ.Title;
                if (activity.Index.HasValue)
                    activ.Index = activity.Index.Value;

                if (activity.ProcurementPlanType.HasValue)
                    activ.ProcurementPlanType = activity.ProcurementPlanType.Value;


                _procurementPlanRepository.UpdateProcurementPlanActivity(activ);

                var procurementPlanActivityDto = _mapper.Map<ProcurementActivityDTO>(activ);

                return Ok(new SuccessResponse<ProcurementActivityDTO>
                {
                    success = true,
                    message = "Procurement Plan Activities updated successfully",
                    data = procurementPlanActivityDto
                });



            }
            catch (Exception ex)
            {

                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An endpoint to create procurement plan activities. ProcurementPlanType can only be PROCUREMENTPLANNING or PROCUREMENTEXECUTION
        /// </summary>
        /// <param name="id"></param>
        /// <param name="procurementActivityForCreation"></param>
        /// <returns></returns>
        [HttpPost("{id}/activities")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProcurementActivityDTO>>), 200)]
        public async Task<IActionResult> CreateProcurementActivity(Guid id, ProcurementActivityForCreation procurementActivityForCreation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("id");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your procurement plan activities creation request failed",
                        errors = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();

                var procurementPlan = await _procurementPlanRepository.GetByIdAsync(id);
                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }

                var duplicateIndex = procurementActivityForCreation.Activities
                    .GroupBy(x => new { Type = x.ProcurementPlanType.ToUpper(), x.Index })
                    .Where(x => x.Count() > 1).Select(x => x.Key.Index);

                var duplicateTitle = procurementActivityForCreation.Activities
                    .GroupBy(x => new { Type = x.ProcurementPlanType.ToUpper(), Title = x.Title.ToUpper() })
                    .Where(x => x.Count() > 1).Select(x => x.Key.Title);

                if (duplicateIndex.Count() > 0)
                {
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your procurement plan activities creation request failed",
                        errors = new
                        {
                            Index = new string[] { "Duplicate Index" }
                        }
                    });
                }

                if (duplicateTitle.Count() > 0)
                {
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your procurement plan activities creation request failed",
                        errors = new
                        {
                            Title = new string[] { "Duplicate Title" }
                        }
                    });
                }

                var procurementPlanActivities = new List<ProcurementPlanActivity>();

                foreach (var activity in procurementActivityForCreation.Activities)
                {
                    if (activity.Id.HasValue)
                    {
                        var existingActivity = await _procurementPlanRepository.GetProcurementPlanActivity(activity.Id.Value);

                        if (existingActivity == null)
                            return NotFound(new ErrorResponse<object>
                            {
                                success = false,
                                message = $"Activity  with id {activity.Id.Value} not found",
                                errors = new { }
                            });


                        _mapper.Map(activity, existingActivity);
                        existingActivity.UpdatedAt = DateTime.UtcNow;
                        _procurementPlanRepository.UpdateProcurementPlanActivity(existingActivity);
                    }
                    else
                    {
                        var procurementPlanActivity = _mapper.Map<ProcurementPlanActivity>(activity);
                        procurementPlanActivity.CreatedById = userClaims.UserId;
                        procurementPlanActivity.ProcurementPlanId = id;
                        procurementPlanActivities.Add(procurementPlanActivity);
                    }
                }

                await _procurementPlanRepository.AddProcurementPlanActivity(procurementPlanActivities);
                await _procurementPlanRepository.SaveChangesAsync();

                var proPlanActivities = await _procurementPlanRepository.GetProcurementPlanActivities(id);

                var procurementPlanActivityDto = _mapper.Map<IEnumerable<ProcurementActivityDTO>>(proPlanActivities);

                return Ok(new SuccessResponse<IEnumerable<ProcurementActivityDTO>>
                {
                    success = true,
                    message = "Procurement Plan Activities created successfully",
                    data = procurementPlanActivityDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }



        /// <summary>
        /// An endpoint to get contracts for a procurement plan, SignatureStatus => 1 = SIGNED, 2 = UNSIGNED, Status => 1 = ACCEPTED, 2 = REJECTED, 3 = PENDING, 4 = EXPIRED, 5 = CLOSED
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("{id}/contracts", Name = "GetAllProcurementContracts")]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<ContractsDTO>>), 200)]
        public async Task<IActionResult> GetAllContracts(Guid id, [FromQuery] ContractsParameters parameters)
        {
            try
            {
                var procurementPlan = await _procurementPlanRepository.GetByIdAsync(id);
                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Procurement plan with id {id} not found",
                        errors = new { }
                    });
                }

                var contracts = await _procurementPlanRepository.GetContracts(id, parameters);

                //var contractDto = _mapper.Map<IEnumerable<ContractsDTO>>(contracts);

                var prevLink = contracts.HasPrevious
                  ? CreateContractResourceUri(parameters, "GetAllProcurementContracts", ResourceUriType.PreviousPage)
                  : null;
                var nextLink = contracts.HasNext
                    ? CreateContractResourceUri(parameters, "GetAllProcurementContracts", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateContractResourceUri(parameters, "GetAllProcurementContracts", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = contracts.TotalPages,
                    perPage = contracts.PageSize,
                    totalEntries = contracts.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<ContractsDTO>>
                {
                    success = true,
                    message = "Contracts retrieved successfully",
                    data = contracts,
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
        /// An endpoint to get All Procurement Categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet("procurementCategories")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProcurementCategory>>), 200)]
        public async Task<IActionResult> GetAllProcurementCategories()
        {
            var procurementCategory = await _procurementPlanRepository.GetAllProcurementCategories();

            return Ok(new SuccessResponse<IEnumerable<ProcurementCategory>>
            {
                success = true,
                message = "Successfully retrieved procurement category",
                data = procurementCategory
            });
        }

        /// <summary>
        /// An endpoint to get All Procurement Methods.
        /// </summary>
        /// <returns></returns>
        [HttpGet("procurementMethods")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProcurementMethod>>), 200)]
        public async Task<IActionResult> GetAllProcurementMethods()
        {
            var procurementMethod = await _procurementPlanRepository.GetAllProcurementMethods();

            return Ok(new SuccessResponse<IEnumerable<ProcurementMethod>>
            {
                success = true,
                message = "Successfully retrieved procurement method",
                data = procurementMethod
            });
        }


        /// <summary>
        /// An endpoint to get All Procurement Processes.
        /// </summary>
        /// <returns></returns>
        [HttpGet("procurementProcesses")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProcurementProcess>>), 200)]
        public async Task<IActionResult> GetAllProcurementProcess()
        {
            var procurmentProcess = await _procurementPlanRepository.GetAllProcurementProcesses();

            return Ok(new SuccessResponse<IEnumerable<ProcurementProcess>>
            {
                success = true,
                message = "Successfully retrieved procurement processes",
                data = procurmentProcess
            });
        }


        /// <summary>
        /// An endpoint to get All Review Methods.
        /// </summary>
        /// <returns></returns>
        [HttpGet("reviewMethods")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ReviewMethod>>), 200)]
        public async Task<IActionResult> GetAllReviewMethods()
        {
            var reviewMethod = await _procurementPlanRepository.GetAllReviewMethods();

            return Ok(new SuccessResponse<IEnumerable<ReviewMethod>>
            {
                success = true,
                message = "Successfully retrieved review method",
                data = reviewMethod
            });
        }


        /// <summary>
        /// An endpoint to get All Qualification Methods.
        /// </summary>
        /// <returns></returns>
        [HttpGet("qualificationMethods")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<QualificationMethod>>), 200)]
        public async Task<IActionResult> GetAllQualificationMethods()
        {
            var qualificationMethod = await _procurementPlanRepository.GetAllQualificationMethods();

            return Ok(new SuccessResponse<IEnumerable<QualificationMethod>>
            {
                success = true,
                message = "Successfully retrieved qualification method",
                data = qualificationMethod
            });
        }

        /// <summary>
        /// An endpoint to generate procurement plan number
        /// </summary>
        /// <returns></returns>
        [HttpGet("procurementPlanNumber")]
        [ProducesResponseType(typeof(SuccessResponse<ProcurementPlanNumberDTO>), 200)]
        public async Task<IActionResult> GetProcurementPlanNumber([FromQuery] ProcurementPlanNumberParameters parameters)
        {
            try
            {
                var planNumber = await _procurementPlanRepository.GetProcurementPlanNumber(parameters);

                if (planNumber == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Please supply ministry code, procument category and method values",
                        errors = new { }
                    });
                }

                var planNumberDto = _mapper.Map<ProcurementPlanNumberDTO>(planNumber);

                return Ok(new SuccessResponse<ProcurementPlanNumberDTO>
                {
                    success = true,
                    message = "Procurement plan number generated successfully",
                    data = planNumberDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An endpoint to approve a procurement plan section. Section can be keydetails, planning and execution
        /// </summary>
        /// <param name="id"></param>
        /// <param name="section"></param>
        [HttpPost("{id}/approveProcurement/{section}")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> ApproveProcurementPlan(Guid id, string section)
        {
            try
            {
                var userClaims = User.UserClaims();

                var procurementPlan = await _procurementPlanRepository.GetProcurementPlan(id);
                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }

                switch (section.ToLower())
                {
                    case "keydetails": procurementPlan.SectionOne = EProcurementSectionStatus.APPROVED; break;
                    case "planning":
                        procurementPlan.SectionTwo = EProcurementSectionStatus.APPROVED;
                        break;
                    case "execution":
                        procurementPlan.SectionThree = EProcurementSectionStatus.APPROVED;
                        break;
                    default: break;
                }

                // check if all section is approved 
                if (procurementPlan.SectionOne == EProcurementSectionStatus.APPROVED && procurementPlan.SectionTwo == EProcurementSectionStatus.APPROVED && procurementPlan.SectionThree == EProcurementSectionStatus.APPROVED)
                {
                    procurementPlan.Status = EProcurementPlanStatus.APPROVED;
                    procurementPlan.Stage = EProcurementStage.ONGOING;
                    var procurementActivity = procurementPlan.ProcurementPlanActivities.FirstOrDefault(x => x.ProcurementPlanType == EPprocurementPlanTask.PROCUREMENTPLANNING && x.Index == 1);
                    procurementActivity.ProcurementPlanActivityStatus = EProcurementPlanActivityStatus.PENDING;
                    _procurementPlanRepository.UpdateProcurementPlanActivity(procurementActivity);
                }

                procurementPlan.UpdatedAt = DateTime.Now;
                _procurementPlanRepository.Update(procurementPlan);

                await _procurementPlanRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Procurement Plan approved successfully",
                    data = new { }
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An endpoint to get all vendors that bid for a procurement
        /// </summary>
        /// <param name="id"></param>
        [HttpGet("{id}/bidVendors")]
        [ProducesResponseType(typeof(SuccessResponse<BidEvaluationVendorDto>), 200)]
        public async Task<IActionResult> GetProcurementPlanBidVendors(Guid id)

        {
            try
            {
                var userClaims = User.UserClaims();

                var procurementPlanExist = await _procurementPlanRepository.IsProcurementPlanExists(id);
                if (!procurementPlanExist)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }

                var bidVendors = await _procurementPlanRepository.GetBidVendors(id);

                var vendorProfiles = new List<VendorProfile>();

                foreach (var vendor in bidVendors)
                {
                    var vendorProfile = await _vendorProfileRepository.GetVendorProfile(vendor);
                    if (vendorProfile != null)
                    {
                        vendorProfiles.Add(vendorProfile);
                    }

                }

                var vendorProfileDto = _mapper.Map<List<BidEvaluationVendorDto>>(vendorProfiles);

                return Ok(new SuccessResponse<List<BidEvaluationVendorDto>>
                {
                    success = true,
                    message = "Vendors retrieved successfully",
                    data = vendorProfileDto
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to express interest in specific procurement notice
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}/expressInterest")]
        [ProducesResponseType(typeof(SuccessResponse<VendorBidDTO>), 200)]
        public async Task<IActionResult> ExpressInterestInProcurementNotice(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId && u.AccountId == userClaims.AccountId && u.UserType == EUserType.VENDOR);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"user with id {userClaims.UserId} not found or user is not a vendor",
                        errors = new { }
                    });
                }

                var procurementPlan = await _procurementPlanRepository.SingleOrDefault(p => p.Id == id);

                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }


                var isBid = await _procurementPlanRepository.IsIndicatedVendor(user.Id, id);

                if (isBid)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor has shown interest for this procurement already",
                        errors = new { }
                    });
                }

                var vendorBid = new VendorBid
                {
                    ProcurementPlan = procurementPlan,
                    ProcurementPlanId = id,
                    Vendor = user,
                    VendorId = user.Id,
                    Ministry = procurementPlan.Ministry?.Name,
                    ProcurementCategory = procurementPlan.ProcurementCategory?.Name,
                    ProcurementType = procurementPlan.ProcurementProcess?.Name,
                };

                await _vendorBidRepository.AddAsync(vendorBid);

                var userActivity = new UserActivity
                {
                    ObjectClass = "VENDOR EXPRESSES PROCUREMENT INTEREST",
                    EventType = "Specific Procurement Interest Creation",
                    UserId = user.Id,
                    ObjectId = user.Id
                };

                await _userActivityRepository.AddAsync(userActivity);

                await _vendorProcurementRepository.SaveChangesAsync();

                var procurementPlanDTO = _mapper.Map<ProcurementPlanDTO>(procurementPlan);

                var vendorBidDTO = _mapper.Map<VendorBidDTO>(vendorBid);

                vendorBidDTO.ProcurementPlan = procurementPlanDTO;


                return Ok(new SuccessResponse<VendorBidDTO>
                {
                    success = true,
                    message = "procurement notice interest created successfully",
                    data = vendorBidDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to fetch/retrieve all Bidders of a procurement plan
        /// </summary>
        /// <returns></returns>
        [HttpGet("bidders", Name = "BiddersForProcurementPlan")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<VendorBidForProcurementPlanDTO>>>), 200)]
        public async Task<IActionResult> BiddersForProcurementPlan([FromQuery] ResourceParameters parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                var vendorExists = await _userRepository.ExistsAsync(x => x.Id == userClaims.UserId && x.UserType == EUserType.VENDOR);

                if (!vendorExists)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with {userClaims.UserId} not found",
                        errors = new { }
                    });
                }

                //get list of all associated vendor profiles
                var vendorBids = await _procurementPlanRepository.GetBiddersForProcurementPlan(userClaims.UserId, parameters);

                if (vendorBids == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Bidders not found",
                        errors = new { }
                    });
                }

                //map vendorBids to vendorBidsDto
                var vendorBidsDto = _mapper.Map<IEnumerable<VendorBidForProcurementPlanDTO>>(vendorBids);

                var prevLink = vendorBids.HasPrevious
                    ? CreateResourceUri(parameters, "BiddersForProcurementPlan", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = vendorBids.HasNext
                    ? CreateResourceUri(parameters, "BiddersForProcurementPlan", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "BiddersForProcurementPlan", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = vendorBids.TotalPages,
                    perPage = vendorBids.PageSize,
                    totalEntries = vendorBids.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<VendorBidForProcurementPlanDTO>>
                {
                    success = true,
                    message = "bidders retrieved successfully",
                    data = vendorBidsDto,
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
        /// An endpoint to fetch/retrieve all Special Procurement Notice
        /// </summary>
        /// <returns></returns>
        [HttpGet("noticeInformation", Name = "GetSpecialProcurementNotice")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<NoticeInformationDTO>>>), 200)]
        public async Task<IActionResult> GetSpecialProcurementNotice([FromQuery] SpecialNoticeInformationParameters parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                //get list of all special notice information
                var noticeInformation = await _procurementPlanRepository.GetNoticeInformation(parameters);

                //map notice information to notice information dto
                var noticeInformationDto = _mapper.Map<IEnumerable<NoticeInformationDTO>>(noticeInformation);

                var prevLink = noticeInformation.HasPrevious
                    ? CreateResourceUri(parameters, "GetSpecialProcurementNotice", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = noticeInformation.HasNext
                    ? CreateResourceUri(parameters, "GetSpecialProcurementNotice", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "GetSpecialProcurementNotice", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = noticeInformation.TotalPages,
                    perPage = noticeInformation.PageSize,
                    totalEntries = noticeInformation.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<NoticeInformationDTO>>
                {
                    success = true,
                    message = "Special Procurement Notice retrieved successfully",
                    data = noticeInformationDto,
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
        /// An endpoint to place a bid on a procurement plan
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}/placeBids")]
        [ProducesResponseType(typeof(SuccessResponse<VendorBidDTO>), 200)]
        public async Task<IActionResult> BidForProcurementPlan(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId && u.AccountId == userClaims.AccountId && u.UserType == EUserType.VENDOR);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"user with id {userClaims.UserId} not found or user is not a vendor",
                        errors = new { }
                    });
                }

                var procurementPlan = await _procurementPlanRepository.SingleOrDefault(p => p.Id == id);

                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }

                //string title = "Bid Invitation";
                //var planActivity = await _procurementPlanRepository.GetVendorPlanActivityByBidTitle(title, id);

                var bidExpiryDate = await _procurementPlanRepository.GetNoticeExpiryDate(id);

                var hasBided = await _vendorBidRepository.HasBided(id, user.Id);

                if (hasBided)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"You can't place multiple bids for a procurement plan",
                        errors = new { }
                    });
                }

                var vendorBid = await _vendorBidRepository.GetVendorBid(user.Id, id);

                if (vendorBid == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor has not shown interest for this procurement",
                        errors = new { }
                    });
                }
                vendorBid.ExpiryDate = bidExpiryDate;
                vendorBid.Type = EVendorContractStatus.PROCESSING;

                _vendorBidRepository.Update(vendorBid);

                var userActivity = new UserActivity
                {
                    ObjectClass = "VENDOR BID FOR A PROCUREMENT PLAN",
                    EventType = "Vendor Bid For Procurement Plan Creation",
                    UserId = user.Id,
                    ObjectId = user.Id
                };

                await _userActivityRepository.AddAsync(userActivity);

                await _vendorBidRepository.SaveChangesAsync();



                var procurementPlanDTO = _mapper.Map<ProcurementPlanDTO>(procurementPlan);

                var vendorBidDTO = _mapper.Map<VendorBidDTO>(vendorBid);

                vendorBidDTO.ProcurementPlan = procurementPlanDTO;

                await _notificationRepository.LogBidSubmissioNotice(procurementPlan.Id, user.Id);

                return Ok(new SuccessResponse<VendorBidDTO>
                {
                    success = true,
                    message = "Vendor bid for a procurement plan created successfully",
                    data = vendorBidDTO
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An endpoint to retrieve/fetch  Bid evaluation Report for a procurement Plan, 
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/evaluatedReport", Name = "GetBidEvaluationReport")]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<VendorBidDTO>>), 200)]
        public async Task<IActionResult> BidEvaluationReportForProcurementPlan(Guid id, [FromQuery] VendorBidParameter parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId && u.AccountId == userClaims.AccountId && u.UserType == EUserType.VENDOR);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"user with id {userClaims.UserId} not found or user is not a vendor",
                        errors = new { }
                    });
                }

                var procurementPlan = await _procurementPlanRepository.SingleOrDefault(p => p.Id == id);

                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }


                var vendorBids = await _vendorBidRepository.GetAllVendorBidsOfProcurementPlan(id, parameters);
                var vendorBidsDto = _mapper.Map<IEnumerable<VendorBidDTO>>(vendorBids);

                var vendorProfiles = new List<VendorProfile>();

                foreach (var vendor in vendorBidsDto)
                {
                    var vendorProfile = await _vendorProfileRepository.GetVendorProfile(vendor.VendorId);
                    if (vendorProfile != null)
                    {
                        var vProfile = _mapper.Map<EvaluatedBidResponse>(vendorProfile);
                        vendor.VendorProfile = vProfile;
                    }
                }

                var prevLink = vendorBids.HasPrevious
                    ? CreateResourceUri(parameters, "GetBidEvaluationReport", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = vendorBids.HasNext
                    ? CreateResourceUri(parameters, "GetBidEvaluationReport", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "GetBidEvaluationReport", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = vendorBids.TotalPages,
                    perPage = vendorBids.PageSize,
                    totalEntries = vendorBids.TotalCount
                };


                return Ok(new PagedResponse<IEnumerable<VendorBidDTO>>
                {
                    success = true,
                    message = "Bid evaluation Report for a procurement Plan retrieved successfully",
                    data = vendorBidsDto,
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
        /// An endpoint to retrieve/fetch  Bid invitation documents
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/bidInvitationDocuments")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<VendorBid>>), 200)]
        public async Task<IActionResult> BidInvitationDocument(Guid id)
        {
            try
            {
                //get procurement plan
                var procurementPlan = await _procurementPlanRepository.GetProcurementPlan(id);

                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }

                //get activity that has the title bid invitation
                var bidInvitationActivity = procurementPlan.ProcurementPlanActivities.SingleOrDefault(x => x.Title.ToLower() == "bid invitation");
                //get all documents that has that activity Id
                var bidInvitationDocuments = await _procurementPlanRepository.GetProcurementPlanDocument(bidInvitationActivity.Id);
                //return document
                var bidInvitationDocumentDto = _mapper.Map<IEnumerable<ProcurementPlanDocumentDTO>>(bidInvitationDocuments);


                return Ok(new SuccessResponse<IEnumerable<ProcurementPlanDocumentDTO>>
                {
                    success = true,
                    message = "Bid evaluation Report for a procurement Plan retrieved successfully",
                    data = bidInvitationDocumentDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to send a procurement plan for approval
        /// </summary>
        /// <param name="id"></param>
        [HttpPost("{id}/sendForApproval")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> ApproveGeneralPlan(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();

                var procurementPlan = await _procurementPlanRepository.GetByIdAsync(id);

                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement plan with id {id} not found",
                        errors = new { }
                    });
                }

                procurementPlan.Status = EProcurementPlanStatus.INREVIEW;
                procurementPlan.UpdatedAt = DateTime.Now;
                _procurementPlanRepository.Update(procurementPlan);

                var userActivity = new UserActivity
                {
                    EventType = "Procurement Plan sent for Approval",
                    UserId = userClaims.UserId,
                    ObjectClass = "Procurement Plan",
                    ObjectId = id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);

                await _procurementPlanRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Procurement Plan sent for approval successfully",
                    data = new { }
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to request for an amendment for procurement section. Section can be keydetails, planning and execution 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="section"></param>
        [HttpPost("{id}/needAmendment/{section}")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> AmmendProcurementPlan(Guid id, string section)
        {
            try
            {
                var userClaims = User.UserClaims();

                var procurementPlan = await _procurementPlanRepository.GetByIdAsync(id);
                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {id} not found",
                        errors = new { }
                    });
                }

                switch (section.ToLower())
                {
                    case "keydetails": procurementPlan.SectionOne = EProcurementSectionStatus.NEEDAMENDMENT; break;
                    case "planning":
                        procurementPlan.SectionTwo = EProcurementSectionStatus.NEEDAMENDMENT;
                        break;
                    case "execution":
                        procurementPlan.SectionThree = EProcurementSectionStatus.NEEDAMENDMENT;
                        break;
                    default: break;
                }

                procurementPlan.UpdatedAt = DateTime.Now;
                _procurementPlanRepository.Update(procurementPlan);

                await _procurementPlanRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Procurement Plan sent for amendmment successfully",
                    data = new { }
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to get list of tender procurment plans
        /// </summary>
        /// <param name="procurementParameter"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("tenderProcurements", Name = "CreateProcurmentTender")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<ProcurementTender>>>), 200)]
        public async Task<IActionResult> GetProcurementPlans([FromQuery] ProcurementTenderParameters procurementParameter)
        {
            try
            {
                var tenderprocurementPlans = await _procurementPlanRepository.GetTenderProcurments(procurementParameter);

                var prevLink = tenderprocurementPlans.HasPrevious ? CreateProcurmentTenderResourceUri(procurementParameter, "CreateProcurmentTender", ResourceUriType.PreviousPage) : null;
                var nextLink = tenderprocurementPlans.HasNext ? CreateProcurmentTenderResourceUri(procurementParameter, "CreateProcurmentTender", ResourceUriType.NextPage) : null;
                var currentLink = CreateProcurmentTenderResourceUri(procurementParameter, "CreateProcurmentTender", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = tenderprocurementPlans.TotalPages,
                    perPage = tenderprocurementPlans.PageSize,
                    totalEntries = tenderprocurementPlans.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<ProcurementTender>>
                {
                    success = true,
                    message = "procurement plan retrieved successfully",
                    data = tenderprocurementPlans,
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
        /// An endpoint to get a tender 
        /// </summary>
        /// <param name="tenderId"></param>
        [AllowAnonymous]
        [HttpGet("{tenderId}/tender")]
        [ProducesResponseType(typeof(SuccessResponse<ProcurementTender>), 200)]
        public async Task<IActionResult> GetTender(Guid tenderId)
        {
            try
            {
                var tender = await _procurementPlanRepository.GetTender(tenderId);
                if (tender == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Tender with id {tenderId} not found",
                        errors = new { }
                    });
                }

                return Ok(new SuccessResponse<ProcurementTender>
                {
                    success = true,
                    message = "Tender retrieved successfully",
                    data = tender
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        #region CreateResource
        private string CreateResourceUri(ResourceParameters parameters, string name, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(name,
                        new
                        {
                            PageNumber = parameters.PageNumber - 1,
                            parameters.PageSize
                        });

                case ResourceUriType.NextPage:
                    return Url.Link(name,
                        new
                        {
                            PageNumber = parameters.PageNumber + 1,
                            parameters.PageSize
                        });

                default:
                    return Url.Link(name,
                        new
                        {
                            parameters.PageNumber,
                            parameters.PageSize
                        });
            }

        }
        #endregion
        #region CreateProcurmentTenderResourceUri

        private string CreateProcurmentTenderResourceUri(ProcurementTenderParameters parameters, string name, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(name,
                                    new
                                    {
                                        PageNumber = parameters.PageNumber - 1,
                                        parameters.PageSize,
                                        parameters.Category,
                                        parameters.ExpiryDate,
                                        parameters.MinistryId,
                                        parameters.Search
                                    });

                case ResourceUriType.NextPage:
                    return Url.Link(name,
                                    new
                                    {
                                        PageNumber = parameters.PageNumber + 1,
                                        parameters.PageSize,
                                        parameters.Category,
                                        parameters.MinistryId,
                                        parameters.ExpiryDate,

                                        parameters.Search
                                    });

                default:
                    return Url.Link(name,
                                    new
                                    {
                                        parameters.PageNumber,
                                        parameters.PageSize,
                                        parameters.Category,
                                        parameters.MinistryId,
                                        parameters.ExpiryDate,
                                        parameters.Search
                                    });

            }

        }

        #endregion

        #region CreateContractResource
        private string CreateContractResourceUri(ContractsParameters parameters, string name, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(name,
                        new
                        {
                            PageNumber = parameters.PageNumber - 1,
                            parameters.PageSize,
                            parameters.SignatureStatus,
                            parameters.Status
                        });

                case ResourceUriType.NextPage:
                    return Url.Link(name,
                        new
                        {
                            PageNumber = parameters.PageNumber + 1,
                            parameters.PageSize,
                            parameters.SignatureStatus,
                            parameters.Status
                        });

                default:
                    return Url.Link(name,
                        new
                        {
                            parameters.PageNumber,
                            parameters.PageSize,
                            parameters.SignatureStatus,
                            parameters.Status
                        });
            }

        }
        #endregion
    }

}
