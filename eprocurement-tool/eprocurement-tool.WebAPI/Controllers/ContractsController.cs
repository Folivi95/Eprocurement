using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EGPS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using EGPS.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using EGPS.Application.Helpers;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;

namespace EGPS.WebAPI.Controllers
{
    /// <summary>
    /// Contracts controller
    /// </summary>

    [Route("api/v1/contracts")]
    [ApiController]
    [Authorize]
    public class ContractsController : ControllerBase
    {
        private readonly IMapper _mapper;
        public IConfiguration Configuration { get; }
        private readonly IUserRepository _userRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IProcurementPlanRepository _procurementPlanRepository;
        private readonly IContractService _contractService;
        private readonly IVendorBidRepository _vendorBidRepository;
        private readonly IVendorProfileRepository _vendorProfileRepository;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="mapper"></param>
        /// <param name="userRepository"></param>
        /// <param name="userActivityRepository"></param>
        /// <param name="staffRepository"></param>
        /// <param name="contractRepository"></param>
        /// <param name="jwtAuthenticationManager"></param>
        /// <param name="procurementPlanRepository"></param>
        /// <param name="contractService"></param>
        /// <param name="vendorBidRepository"></param>
        /// <param name="vendorProfileRepository"></param>


        public ContractsController(IMapper mapper,
            IConfiguration configuration,
            IUserRepository userRepository,
            IUserActivityRepository userActivityRepository,
            IStaffRepository staffRepository,
            IContractRepository contractRepository,
            IJwtAuthenticationManager jwtAuthenticationManager,
            IProcurementPlanRepository procurementPlanRepository,
            IContractService contractService,
            IVendorBidRepository vendorBidRepository,
            IVendorProfileRepository vendorProfileRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _staffRepository = staffRepository ?? throw new ArgumentNullException(nameof(staffRepository));
            _contractRepository = contractRepository ?? throw new ArgumentNullException(nameof(contractRepository));
            _jwtAuthenticationManager = jwtAuthenticationManager ?? throw new ArgumentNullException(nameof(jwtAuthenticationManager));
            _procurementPlanRepository = procurementPlanRepository;
            _contractService = contractService;
            _vendorBidRepository = vendorBidRepository;
            _vendorProfileRepository = vendorProfileRepository ?? throw new ArgumentNullException(nameof(vendorProfileRepository));
        }


        /// <summary>
        /// Endpoint to summarize contracts data
        /// </summary>
        /// <returns></returns>
        [HttpGet("summary", Name = "SummarizeContracts")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> SummarizeContracts()
        {
            try
            {
                //get token details
                var userClaims = User.UserClaims();

                //check if user is a staff and give error response if user is not a staff
                if (!(await _staffRepository.IsStaff(userClaims.UserId)))
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User {userClaims.UserId} is not a staff",
                        errors = new { }
                    });
                }

                //get summarised details
                var contractsSummary = await _contractRepository.GetContractsSummary();

                return Ok(new SuccessResponse<ContractsSummaryDTO>
                {
                    success = true,
                    message = "Contracts summary retrieved successfully",
                    data = contractsSummary
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// Endpoint to summarize contracts data
        /// </summary>
        /// <returns></returns>
        [HttpGet("summary/vendor/{vendorId}", Name = "SummarizeContractsForVendor")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> SummarizeContractsForVendir([FromRoute]Guid vendorId)
        {
            try
            {
                //get token details
                var userClaims = User.UserClaims();

                //check if user is a staff and give error response if user is not a staff
                if (!(await _staffRepository.IsStaff(userClaims.UserId)))
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User {userClaims.UserId} is not a staff",
                        errors = new { }
                    });
                }

                //get summarised details
                var contractsSummary = await _contractRepository.GetContractSummaryForVendor(vendorId);

                return Ok(new SuccessResponse<ContractsSummaryDTO>
                {
                    success = true,
                    message = "Contracts summary retrieved successfully",
                    data = contractsSummary
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to get all contracts, Status is either Accepted, Rejected or Pending
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllContracts")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<ContractsDTO>>>), 200)]
        public async Task<IActionResult> GetAllContracts([FromQuery] ContractParameters parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                //check if user is a staff and give error response if user is not a staff
                if (!(await _staffRepository.IsStaff(userClaims.UserId)))
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User {userClaims.UserId} is not a staff",
                        errors = new { }
                    });
                }

                //get list of all contracts
                var contracts = await _contractRepository.GetAllContracts(userClaims.UserId, parameters);

                //map contracts to contractsDto
                var contractsDto = _mapper.Map<IEnumerable<ContractsDTO>>(contracts);

                var prevLink = contracts.HasPrevious
                    ? CreateResourceUri(parameters, "GetAllContracts", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = contracts.HasNext
                    ? CreateResourceUri(parameters, "GetAllContracts", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "GetAllContracts", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = contracts.TotalPages,
                    perPage = contracts.PageSize,
                    totalEntries = contracts.TotalCount
                };

                if (contractsDto.FirstOrDefault() == null)
                {
                    return Ok(new PagedResponse<IEnumerable<ContractsDTO>>
                    {
                        success = false,
                        message = $"Contracts retrieved successfully",
                        data = Enumerable.Empty<ContractsDTO>(),
                        meta = new Meta
                        {
                            pagination = pagination
                        }
                    });
                }

                return Ok(new PagedResponse<IEnumerable<ContractsDTO>>
                {
                    success = true,
                    message = "contracts retrieved successfully",
                    data = contractsDto,
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
        /// An endpoint to get a contract
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<IndividualContractDTO>), 200)]
        public async Task<IActionResult> GetContract(Guid id)
        {
            try
            {
                var contract = await _contractRepository.GetByIdAsync(id);
                if (contract == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Contract with id {id} not found",
                        errors = new { }
                    });
                }

                var contractDto = await _contractRepository.GetContract(id);
                var documents = await _contractRepository.GetDocuments(contractDto.ProcurementPlan.Id);

                contractDto.Documents = _mapper.Map<IEnumerable<ProcurementPlanDocumentDTO>>(documents);

                //var contractDto = _mapper.Map<ContractsDTO>(contract);

                return Ok(new SuccessResponse<IndividualContractDTO>
                {
                    success = true,
                    message = "Contract retrieved successfully",
                    data = contractDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// Endpoint to create contracts
        /// </summary>
        /// <param name="procurementPlanId"></param>
        /// <param name="contractForCreation"></param>
        /// <returns></returns>
        [HttpPost("{procurementPlanId}")]
        [ProducesResponseType(typeof(SuccessResponse<CreatedContractDTO>), 200)]
        public async Task<IActionResult> CreateContract(Guid procurementPlanId, ContractForCreation contractForCreation)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    ModelState.Remove("procurementPlanId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your  contract creation request failed",
                        errors = ModelState.Error()
                    });
                }
                var userClaims = User.UserClaims();

                var procurementPlan = await _procurementPlanRepository.GetByIdAsync(procurementPlanId);

                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan  with id {procurementPlanId} not found",
                        errors = new { }
                    });
                }

                var contractsCount =
                    await _contractRepository.GetAllContractsForProcurementPlanCount(procurementPlanId);

                if (!Enum.IsDefined(typeof(EDurationType), contractForCreation.Type.ToUpper()))
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Duration type must be month, year, week or day",
                        errors = new Dictionary<string, string[]>()
                    });
                }


                var pendingContractsExists = await _contractRepository.PendingContractExists(procurementPlanId);

                if (pendingContractsExists > 0)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "A pending contract already exists",
                        errors = new Dictionary<string, string[]>()
                    });
                }

                var endDate = _contractService.ConvertDuration(contractForCreation.Duration, (EDurationType)Enum.Parse(typeof(EDurationType), contractForCreation.Type.ToUpper()));
                var formerContract = await _contractRepository.GetRejectedOrExpiredContract(procurementPlanId);
                if (formerContract != null)
                {
                    formerContract.Status = EContractStatus.CLOSED;
                    _contractRepository.Update(formerContract);
                }

                var notStartedBids = await _vendorBidRepository.GetNotStartedBids(procurementPlanId);
                foreach (var vBid in notStartedBids)
                {
                    vBid.Type = EVendorContractStatus.EXPIRED;
                }


                var contract = new Contract()
                {
                    Title = $"Contract {contractsCount + 1}",
                    Description = contractForCreation.Description,
                    EvaluationCurrency = contractForCreation.EvaluationCurrency,
                    StartDate = DateTime.Now,
                    EndDate = endDate,
                    ProcurementPlanId = procurementPlanId,
                    CreateAt = DateTime.Now,
                    UserId = userClaims.UserId,
                    PercentageCompletion = 0,
                    Status = EContractStatus.PENDING,
                    Type = (EDurationType)Enum.Parse(typeof(EDurationType), contractForCreation.Type.ToUpper()),
                    Duration = contractForCreation.Duration

                };
                await _contractRepository.AddAsync(contract);

                await _contractRepository.SaveChangesAsync();
                var contractsDto = _mapper.Map<CreatedContractDTO>(contract);
                contractsDto.ProcurementPlan = _mapper.Map<ProcurementPlanForContractDto>(procurementPlan);

                return Ok(new SuccessResponse<CreatedContractDTO>
                {
                    success = true,
                    message = "contract created successfully",
                    data = contractsDto,
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to return a list of contracts for a vendor
        /// </summary>
        /// <returns></returns>
        [HttpGet("vendorcontracts", Name = "ContractsForVendor")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<ContractsForVendorDTO>>>), 200)]
        public async Task<IActionResult> ContractsForVendor([FromQuery]ResourceParameters parameters)
        {
            try
            {
                //get token details
                var userClaims = User.UserClaims();

                //check if user is a staff and give error response if user is not a vendor
                if (!(await _userRepository.IsVendor(userClaims.UserId)))
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User {userClaims.UserId} is not a vendor",
                        errors = new { }
                    });
                }

                //get contracts details
                var contracts = await _contractRepository.GetContractsForVendor(userClaims.UserId, parameters);

                //map response back to contractsdto
                var contractsDto = _mapper.Map<IEnumerable<ContractsForVendorDTO>>(contracts);

                var prevLink = contracts.HasPrevious
                    ? CreateResourceUri(parameters, "ContractsForVendor", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = contracts.HasNext
                    ? CreateResourceUri(parameters, "ContractsForVendor", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "ContractsForVendor", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = contracts.TotalPages,
                    perPage = contracts.PageSize,
                    totalEntries = contracts.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<ContractsForVendorDTO>>
                {
                    success = true,
                    message = "Contracts retrieved successfully",
                    data = contractsDto,
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
        /// Endpoint to return a list of contracts by using it's procurement plan id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{procurementPlanId}/GetContractsByProcurementPlan")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<ContractsForVendorDTO>>>), 200)]
        public async Task<IActionResult> GetContractByProcurementPlan(Guid procurementPlanId, [FromQuery] ResourceParameters parameters)
        {
            try
            {
                //get token details
                var userClaims = User.UserClaims();

                //get contracts details
                var contracts = await _contractRepository.GetContractByProcurementPlanId(procurementPlanId, parameters);

                if (contracts == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Contracts with procurement plan id {procurementPlanId} not found",
                        errors = new { }
                    });
                }
                //map response back to contractsdto
                var contractsDto = _mapper.Map<IEnumerable<ContractsForVendorDTO>>(contracts);

                var prevLink = contracts.HasPrevious
                    ? CreateResourceUri(parameters, "GetContractByProcurementPlan", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = contracts.HasNext
                    ? CreateResourceUri(parameters, "GetContractByProcurementPlan", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "GetContractByProcurementPlan", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = contracts.TotalPages,
                    perPage = contracts.PageSize,
                    totalEntries = contracts.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<ContractsForVendorDTO>>
                {
                    success = true,
                    message = "Contracts retrieved successfully",
                    data = contractsDto,
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
        /// An endpoint to get all awarded contracts
        /// </summary>
        [HttpGet("awardedContract")]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<ContractsDTO>>), 200)]
        public async Task<IActionResult> GetAwardedContract([FromQuery] ResourceParameters parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                var awardedContracts = await _contractRepository.GetAwardedContracts(parameters);

                //map contracts to contractsDto
                var awardedContractDTO = _mapper.Map<IEnumerable<ContractAwardDTO>>(awardedContracts);

                var prevLink = awardedContracts.HasPrevious
                    ? CreateResourceUri(parameters, "GetAwardedContract", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = awardedContracts.HasNext
                    ? CreateResourceUri(parameters, "GetAwardedContract", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "GetAwardedContract", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = awardedContracts.TotalPages,
                    perPage = awardedContracts.PageSize,
                    totalEntries = awardedContracts.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<ContractAwardDTO>>
                {
                    success = true,
                    message = "Contract Award Documents retrieved successfully",
                    data = awardedContractDTO,
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
        /// Endpoint to retrieve/fetch all documents for Letter of Acceptance in a Procurement Plan
        /// </summary>
        /// <returns></returns>
        [HttpGet("acceptanceLetter", Name = "GetProcurementPlanActivityWithLetterOfAcceptance")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<ProcurementPlanActivityDTO>>>), 200)]
        public async Task<IActionResult> GetProcurementPlanActivityWithLetterOfAcceptance(Guid procurementPlanId, [FromQuery] ResourceParameters parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                //get list of all procurement plan documents
                string title = "Letter of Acceptance";
                var planActivity = await _procurementPlanRepository.GetProcurementPlanActivityByTitle(title, procurementPlanId, parameters);

                if (planActivity == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Letter of Acceptance not found",
                        errors = new { }
                    });
                }

                //map contracts to contractsDto
                var planActivitiesDto = _mapper.Map<IEnumerable<ProcurementPlanActivityDTO>>(planActivity);

                var prevLink = planActivity.HasPrevious
                    ? CreateResourceUri(parameters, "GetProcurementPlanActivityWithLetterOfAcceptance", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = planActivity.HasNext
                    ? CreateResourceUri(parameters, "GetProcurementPlanActivityWithLetterOfAcceptance", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "GetProcurementPlanActivityWithLetterOfAcceptance", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = planActivity.TotalPages,
                    perPage = planActivity.PageSize,
                    totalEntries = planActivity.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<ProcurementPlanActivityDTO>>
                {
                    success = true,
                    message = "Letter of acceptance with Documents retrieved successfully",
                    data = planActivitiesDto,
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
        /// Endpoint to get all contracts by a particular vendor
        /// </summary>
        /// <returns></returns>
        [HttpGet("vendor/{vendorId}", Name = "GetContractsByVendor")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<ContractsDTO>>>), 200)]
        public async Task<IActionResult> GetAllContractByVendor(Guid vendorId, [FromQuery] ResourceParameters parameters)
        {
            try
            {
                //get list of all contracts
                var contracts = await _contractRepository.GetAllContractsByVendor(vendorId, parameters);


                //map contracts to contractsDto
                var contractsDto = _mapper.Map<IEnumerable<ContractsDTO>>(contracts);

                var prevLink = contracts.HasPrevious
                    ? CreateResourceUriVendor(parameters, "GetContractsByVendor", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = contracts.HasNext
                    ? CreateResourceUriVendor(parameters, "GetContractsByVendor", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUriVendor(parameters, "GetContractsByVendor", ResourceUriType.CurrentPage);

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
                    message = "contracts retrieved successfully",
                    data = contractsDto,
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
        /// An endpoint to get list of awarded procurment plans
        /// </summary>
        /// <param name="procurementParameter"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("awardedProcurements", Name = "GetProcurmentContractPlans")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<ProcurementContract>>>), 200)]
        public async Task<IActionResult> GetProcurementPlans([FromQuery] ProcurementContractParameters procurementParameter)
        {
            try
            {
                var ContractprocurementPlans = await _contractRepository.GetProcurmentContract(procurementParameter);

                var prevLink = ContractprocurementPlans.HasPrevious ? CreateProcurmentResourceUri(procurementParameter, "GetProcurmentContractPlans", ResourceUriType.PreviousPage) : null;
                var nextLink = ContractprocurementPlans.HasNext ? CreateProcurmentResourceUri(procurementParameter, "GetProcurmentContractPlans", ResourceUriType.NextPage) : null;
                var currentLink = CreateProcurmentResourceUri(procurementParameter, "GetProcurmentContractPlans", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = ContractprocurementPlans.TotalPages,
                    perPage = ContractprocurementPlans.PageSize,
                    totalEntries = ContractprocurementPlans.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<ProcurementContract>>
                {
                    success = true,
                    message = "procurement plan retrieved successfully",
                    data = ContractprocurementPlans,
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
        /// Endpoint to review vendor bid
        /// </summary>
        /// <param name="procurementPlanId"></param>
        /// <param name="VendorBid"></param>
        /// <returns></returns>
        [HttpPost("{procurementPlanId}/reviewVendorBid")]
        [ProducesResponseType(typeof(SuccessResponse<VendorBidDTO>), 200)]
        public async Task<IActionResult> ReviewVendorBid(Guid procurementPlanId, VendorBidForCreationDTO VendorBid)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    ModelState.Remove("procurementPlanId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your  vendor review request failed",
                        errors = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();

                var procurementPlan = await _procurementPlanRepository.GetByIdAsync(procurementPlanId);

                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan  with id {procurementPlanId} not found",
                        errors = new { }
                    });
                }

                var recommendedVendorExists = await _contractRepository.IsRecommendedVendorExists(procurementPlanId);

                if (recommendedVendorExists && VendorBid.Type == EVendorContractStatus.RECOMMENDED)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "A vendor has already been recommended",
                        errors = new Dictionary<string, string[]>()
                    });
                }

                if (VendorBid.Type == EVendorContractStatus.RECOMMENDED)
                {
                    var contract = await _procurementPlanRepository.GetContract(procurementPlan.Id);
                    contract.ContractorId = VendorBid.VendorId;
                    contract.EstimatedValue = (double)VendorBid.Amount;
                    await _vendorBidRepository.SaveChangesAsync();
                }


                var bid = await _contractRepository.GetVendorBid(procurementPlanId, VendorBid.VendorId);
                if (bid != null)
                {
                    bid.Amount = VendorBid.Amount;
                    bid.BidPrice = VendorBid.BidPrice;
                    bid.Currency = VendorBid.Currency;
                    bid.EvaluatedPrice = VendorBid.EvaluatedPrice;
                    bid.Reason = VendorBid.Reason;
                    bid.Type = VendorBid.Type;
                    bid.Position = VendorBid.Position;

                    _vendorBidRepository.Update(bid);
                }


                var bidToReturn = _mapper.Map<VendorBidDTO>(bid);


                return Ok(new SuccessResponse<VendorBidDTO>
                {
                    success = true,
                    message = "Vendor Bid updated successfully",
                    data = bidToReturn,
                });


            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to get an awarded procurement contract 
        /// </summary>
        /// <param name="id"></param>
        [AllowAnonymous]
        [HttpGet("{id}/awardedProcurement")]
        [ProducesResponseType(typeof(SuccessResponse<ProcurementContract>), 200)]
        public async Task<IActionResult> GetTender(Guid id)
        {
            try
            {
                var contract = await _contractRepository.GetProcurmentContractById(id);
                if (contract == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Awarded procurement contract with id {id} not found",
                        errors = new { }
                    });
                }

                return Ok(new SuccessResponse<ProcurementContract>
                {
                    success = true,
                    message = "Awarded procurement retrieved successfully",
                    data = contract
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

        #region CreateResourceForVendor

        private string CreateResourceUriVendor(ResourceParameters parameters, string name, ResourceUriType type)
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

        private string CreateProcurmentResourceUri(ProcurementContractParameters parameters, string name, ResourceUriType type)
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
                                        parameters.DateAwarded,
                                        parameters.Search
                                    });

                case ResourceUriType.NextPage:
                    return Url.Link(name,
                                    new
                                    {
                                        PageNumber = parameters.PageNumber + 1,
                                        parameters.PageSize,
                                        parameters.Category,
                                        parameters.DateAwarded,
                                        parameters.Search
                                    });

                default:
                    return Url.Link(name,
                                    new
                                    {
                                        parameters.PageNumber,
                                        parameters.PageSize,
                                        parameters.Category,
                                        parameters.DateAwarded,
                                        parameters.Search
                                    });

            }

        }
    }
}
