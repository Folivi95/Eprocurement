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
    [Route("api/v1/bids")]
    [ApiController]
    [Authorize]
    public class BidController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBidRepository _bidRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IProcurementPlanRepository _procurementPlanRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="bidRepository"></param>
        /// <param name="staffRepository"></param>
        /// <param name="procurementPlanRepository"></param>
        /// <param name="userRepository"></param>
        public BidController(IMapper mapper,
            IBidRepository bidRepository,
            IStaffRepository staffRepository,
            IProcurementPlanRepository procurementPlanRepository,
            IUserRepository userRepository
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _bidRepository = bidRepository ?? throw new ArgumentNullException(nameof(bidRepository));
            _staffRepository = staffRepository ?? throw new ArgumentNullException(nameof(staffRepository));
            _procurementPlanRepository = procurementPlanRepository ?? throw new ArgumentNullException(nameof(procurementPlanRepository));
            _userRepository = userRepository ?? throw new ArgumentException(nameof(userRepository));
        }

        /// <summary>
        /// Endpoint to summarize bids data
        /// </summary>
        /// <returns></returns>
        [HttpGet("summary", Name = "SummarizeBids")]
        [ProducesResponseType(typeof(SuccessResponse<BidSummaryDTO>), 200)]
        public async Task<IActionResult> SummarizeBids()
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
                var bidSummary = await _bidRepository.GetBidSummary();

                return Ok(new SuccessResponse<BidSummaryDTO>
                {
                    success = true,
                    message = "Bids summary retrieved successfully",
                    data = bidSummary
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }



        /// <summary>
        /// Endpoint to retrieve/fetch all Bids
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAllBid", Name = "GetAllBids")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<ProcurementPlanActivityDTO>>>), 200)]
        public async Task<IActionResult> GetProcurementPlanWithBidInvitation([FromQuery] BidsParameters parameters)
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

                //get list of all procurement plan documents
                string title = "Bid Invitation";
                var planActivity = await _procurementPlanRepository.GetProcurementPlanActivityByBidTitle(title, parameters);

                if (planActivity == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Bids not found",
                        errors = new { }
                    });
                }

                //map contracts to contractsDto
                var planActivitiesDto = _mapper.Map<IEnumerable<ProcurementPlanActivityDTO>>(planActivity);

                var prevLink = planActivity.HasPrevious
                    ? CreateResourceUri(parameters, "GetAllBids", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = planActivity.HasNext
                    ? CreateResourceUri(parameters, "GetAllBids", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "GetAllBids", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = planActivity.TotalPages,
                    perPage = planActivity.PageSize,
                    totalEntries = planActivity.TotalCount
                };

                if(planActivitiesDto.FirstOrDefault() == null)
                {
                    return Ok(new PagedResponse<IEnumerable<ProcurementPlanActivityDTO>>
                    {
                        success = true,
                        message = "Bids retrieved successfully",
                        data = Enumerable.Empty<ProcurementPlanActivityDTO>(),
                        meta = new Meta
                        {
                            pagination = pagination
                        }
                    });
                }

                return Ok(new PagedResponse<IEnumerable<ProcurementPlanActivityDTO>>
                {
                    success = true,
                    message = "Bids retrieved successfully",
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
        /// Endpoint to retrieve/fetch all Bids for the logged in vendor
        /// </summary>
        /// <returns></returns>
        [HttpGet("vendor/{vendorId}" ,Name = "GetAllBidsForVendor")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<BidsTable>>>), 200)]
        public async Task<IActionResult> GetBidsForVendors(Guid vendorId, [FromQuery] BidsTableParameters parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                //check if user is a staff and give error response if user is not a staff
                if (!(await _userRepository.IsVendor(vendorId)))
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User {vendorId} is not a vendor",
                        errors = new { }
                    });
                }

                var result = await _procurementPlanRepository.GetBidsForVendor(userClaims.UserId, parameters);

                var vendorBidsDTO =  _mapper.Map<IEnumerable<BidsTable>>(result);            

                var prevLink = result.HasPrevious
                    ? CreateResourceUri(parameters, "GetAllBidsForVendor", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = result.HasNext
                    ? CreateResourceUri(parameters, "GetAllBidsForVendor", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "GetAllBidsForVendor", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = result.TotalPages,
                    perPage = result.PageSize,
                    totalEntries = result.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<BidsTable>>
                {
                    success = true,
                    message = "Bids retrieved successfully",
                    data = vendorBidsDTO,
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
        /// Endpoint to get a single bid for a vendor
        /// </summary>
        /// <returns></returns>
        [HttpGet("{bidId}/vendor", Name = "SingleBid")]
        [ProducesResponseType(typeof(SuccessResponse<BidsTable>), 200)]
        public async Task<IActionResult> GetABid([FromRoute]Guid bidId)
        {
            try
            {
                //get token details
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId);

                if (user == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User {userClaims.UserId} is not a found",
                        errors = new { }
                    });
                }

                //get a single bid
                var bid = await _bidRepository.GetABid(bidId);

                var vendorBidDTO = _mapper.Map<BidsTable>(bid);

                return Ok(new SuccessResponse<BidsTable>
                {
                    success = true,
                    message = "Bid retrieved successfully",
                    data = vendorBidDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }



        /// <summary>
        /// Endpoint to summarize bids for a vendor
        /// </summary>
        /// <returns></returns>
        [HttpGet("summary/{userId}/vendor", Name = "SummarizeVendorBids")]
        [ProducesResponseType(typeof(SuccessResponse<BidSummaryDTO>), 200)]
        public async Task<IActionResult> SummarizeVendorBids(Guid userId)
        {
            try
            {
                //get token details
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId);

                if(user == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User {userClaims.UserId} is not a found",
                        errors = new { }
                    });
                }

                var vendor = await _userRepository.SingleOrDefault(v => v.Id == userId && v.UserType == Domain.Enums.EUserType.VENDOR);

                if (vendor == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"The user Id you passed in,  {userId} does not exist is not a vendor",
                        errors = new { }
                    });
                }


                //get summarised details
                var bidSummary = await _bidRepository.GetVendorBidSummary(vendor.Id);

                return Ok(new SuccessResponse<BidSummaryDTO>
                {
                    success = true,
                    message = "Bids summary retrieved successfully",
                    data = bidSummary
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
    }
}
