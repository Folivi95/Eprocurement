using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EGPS.Application;
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
    /// <summary>
    /// Transactions controller
    /// </summary>
    [Route("api/v1/transactions")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectMileStoneRepository _projectMileStoneRepository;
        private readonly IMilestoneInvoiceRepository _milestoneInvoiceRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProcurementPlanRepository _procurementPlanRepository;
        private readonly IProcurementService _procurementService;
        private const string USER_IP_ADDRESS = "User-IP-Address";

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userActivityRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="projectMileStoneRepository"></param>
        /// <param name="milestoneInvoiceRepository"></param>
        /// <param name="projectRepository"></param>
        /// <param name="procurementPlanRepository"></param>
        /// <param name="procurementService"></param>
        public TransactionsController(IMapper mapper,
            IUserActivityRepository userActivityRepository,
            IUserRepository userRepository,
            IProjectMileStoneRepository projectMileStoneRepository,
            IProjectRepository projectRepository,
            IMilestoneInvoiceRepository milestoneInvoiceRepository,
            IProcurementPlanRepository procurementPlanRepository,
            IProcurementService procurementService
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _projectMileStoneRepository = projectMileStoneRepository ?? throw new ArgumentNullException(nameof(projectMileStoneRepository));
            _milestoneInvoiceRepository = milestoneInvoiceRepository ?? throw new ArgumentNullException(nameof(milestoneInvoiceRepository));
            _projectRepository = projectRepository;
            _procurementPlanRepository = procurementPlanRepository ?? throw new ArgumentNullException(nameof(procurementPlanRepository));
            _procurementService = procurementService ?? throw new ArgumentNullException(nameof(procurementService));
        }


        /// <summary>
        /// An endpoint to fetch a transaction by milestone Id
        /// </summary>
        /// <param name="milestoneId"></param>
        /// <returns></returns>
        [HttpGet("{milestoneId}")]
        [ProducesResponseType(typeof(SuccessResponse<ProjectMileStoneDTO>), 200)]
        public async Task<IActionResult> GetTransactionsByMilestoneId(Guid milestoneId)
        {
            try
            {
                if (!await _projectMileStoneRepository.ExistsAsync(x => x.Id == milestoneId))
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Milestone with id {milestoneId} not found",
                        errors = new { }
                    });
                }

                var milestone = await _milestoneInvoiceRepository.GetTransactionsByMilestoneId(milestoneId);

                return Ok(new SuccessResponse<TransactionProjectMilestoneDTO>
                {
                    success = true,
                    message = "Transaction retrieved successfully",
                    data = _mapper.Map<TransactionProjectMilestoneDTO>(milestone),
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
       
        }

        
        [HttpGet(Name = "GetAllTransactions")]
        [Authorize]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<TransactionTableViewModel>>), 200)]
        public async Task<IActionResult> GetAllVendorTransactionsAsync([FromQuery]TransactionParameters parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                //check if user is a staff and give error response if user is not a staff

                var vendor = await _userRepository.IsVendor(userClaims.UserId);

                if(!vendor)
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User {userClaims.UserId} is not a vendor",
                        errors = new { }
                    });


                var mileStoneInvoices = await _milestoneInvoiceRepository.GetMilestoneInvoicesByVendor(parameters, userClaims.UserId);

                var invoiceTableView = _mapper.Map<IEnumerable<TransactionTableViewModel>>(mileStoneInvoices);


                var prevLink = mileStoneInvoices.HasPrevious
                   ? CreateResourceUri(parameters, "GetAllTransactions", ResourceUriType.PreviousPage)
                   : null;
                var nextLink = mileStoneInvoices.HasNext
                    ? CreateResourceUri(parameters, "GetAllTransactions", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "GetAllTransactions", ResourceUriType.CurrentPage);



                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = mileStoneInvoices.TotalPages,
                    perPage = mileStoneInvoices.PageSize,
                    totalEntries = mileStoneInvoices.TotalCount
                };
                var response = new PagedResponse<IEnumerable<TransactionTableViewModel>>
                {
                    success = true,
                    message = "Transactions retrieved successfully",
                    data = invoiceTableView,
                    meta = new Meta
                    {
                        pagination = pagination
                    }
                };
                return Ok(response);

            }
            catch (Exception ex)
            {

                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to get all invoice in a project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="parm"></param>
        [HttpGet("{projectId}/invoice", Name = nameof(GetInvoiceByProject))]
        [ProducesResponseType(typeof(PagedList<List<MilestoneInvoiceDTO>>), 200)]
        public async Task<IActionResult> GetInvoiceByProject(Guid projectId, [FromQuery] ResourceParameters parm )
        {
            try
            {
                var findProject = await _projectRepository.SingleOrDefault(a => a.Id == projectId);
                if (findProject == null)
                {
                    return NotFound( new ErrorResponse<object>
                    {
                        success = false,
                        message = $"project with {projectId } not found",
                        errors =  new { }
                    });
                }

                var record = await _milestoneInvoiceRepository.GetInvoiceForProject(projectId, parm);
                var invoice = _mapper.Map<List<MilestoneInvoiceDTO>>(record);
                var product = _mapper.Map<ProjectsDTO>(findProject); 
                invoice.ForEach(a=>a.Project = product);
                var pageLinks = PageUtility<MilestoneInvoice>.CreateResourcePageUrl(parm, nameof(GetInvoiceByProject), record, Url);
                var response = new PagedResponse<List<MilestoneInvoiceDTO>>
                {
                    success = true,
                    message = "Transactions retrieved successfully",
                    data = invoice,
                    meta = new Meta
                    {
                        pagination = pageLinks
                    }
                };
               
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        } 
        

        /// <summary>
        /// An endpoint to upload payment document
        /// </summary>
        /// <param name="paymentDocumentRequest"></param>
        /// <returns></returns>
        [HttpPost("paymentDocument")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProcurementPlanDocumentDTO>>), 200)]
        public async Task<IActionResult> PaymentDocument([FromForm] PaymentDocumentRequest paymentDocumentRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("id");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your procurement document upload failed",
                        errors = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();
                var documentDto = new GenericProcurementPlanDocumentDto()
                {
                    UserId = userClaims.UserId,
                    Documents = paymentDocumentRequest.Documents,
                    ObjectId = paymentDocumentRequest.ObjectId,
                    Status = EDocumentStatus.PAYMENT
                };
            
                var procurementPlanDocuments = await _procurementService.CreateGenericDocument(documentDto);

                await _procurementPlanRepository.AddProcurementPlanDocument(procurementPlanDocuments);

                await _procurementPlanRepository.SaveChangesAsync();

                //Create, add and save new UserActivity
                var userActivity = new UserActivity
                {
                    EventType = $"Payment Document Creation",
                    UserId = userClaims.UserId,
                    ObjectClass = "PAYMENT",
                    ObjectId = paymentDocumentRequest.ObjectId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();


                var procurementPlanDocumentDto = _mapper.Map<IEnumerable<ProcurementPlanDocumentDTO>>(procurementPlanDocuments);

                return Ok(new SuccessResponse<IEnumerable<ProcurementPlanDocumentDTO>>
                {
                    success = true,
                    message = "Payment Document uploaded successfully",
                    data = procurementPlanDocumentDto,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An endpoint to get all invoice from project id with milestone data filtered by invoice paid status 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("project/{projectId}/milestone", Name = nameof(GetMilestoneInvoiceById))]
        [ProducesResponseType(typeof(PagedList<List<ProjectMileStoneDTO>>), 200)]
        public async Task<IActionResult> GetMilestoneInvoiceById(Guid projectId, [FromQuery] TransactionParameters parameters )
        {
            try
            {
                var findProject = await _projectRepository.SingleOrDefault(a => a.Id == projectId);
                if (findProject == null)
                {
                    return NotFound( new ErrorResponse<object>
                    {
                        success = false,
                        message = $"project with {projectId } not found",
                        errors =  new { }
                    });
                }

                var record = await _milestoneInvoiceRepository.GetPaidInvoiceMileStoneTaskFromProject(projectId, parameters);
                var data = _mapper.Map<List<ProjectMileStoneDTO>>(record);
                var prevLink = record.HasPrevious
                    ? CreateResourceUri(parameters, nameof(GetMilestoneInvoiceById), ResourceUriType.PreviousPage)
                    : null;
                var nextLink = record.HasNext
                    ? CreateResourceUri(parameters, nameof(GetMilestoneInvoiceById), ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, nameof(GetMilestoneInvoiceById), ResourceUriType.CurrentPage);



                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = record.TotalPages,
                    perPage = record.PageSize,
                    totalEntries = record.TotalCount
                };
                var response = new PagedResponse<List<ProjectMileStoneDTO>>
                {
                    success = true,
                    message = "Transactions retrieved successfully",
                    data = data,
                    meta = new Meta
                    {
                        pagination = pagination
                    }
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

     

        #region CreateResource
        private string CreateTransactionResourceUri(TransactionParameters parameters, string name, ResourceUriType type)
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
    }
}