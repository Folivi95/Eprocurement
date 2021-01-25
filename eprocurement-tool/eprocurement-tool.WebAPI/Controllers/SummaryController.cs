using System;
using System.Threading.Tasks;
using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EGPS.WebAPI.Controllers
{
    /// <summary>
    /// Summary Controller
    /// </summary>


    [Route("api/v1/Summary")]
    [ApiController]
    [Authorize]
    public class SummaryController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IProcurementPlanRepository _procurementPlanRepository;
        private readonly IVendorProfileRepository _vendorProfileRepository;


        /// <summary>
        /// Class constructor 
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userRepository"></param>
        /// <param name="userActivityRepository"></param>
        /// <param name="projectRepository"></param>
        /// <param name="contractRepository"></param>
        /// <param name="bidRepository"></param>
        /// <param name="procurementPlanRepository"></param>
        /// <param name="vendorProfileRepository"></param>
        public SummaryController(
            IMapper mapper,
            IUserRepository userRepository,
            IUserActivityRepository userActivityRepository,
            IProjectRepository projectRepository,
            IContractRepository contractRepository,
            IBidRepository bidRepository,
            IProcurementPlanRepository procurementPlanRepository,
            IVendorProfileRepository vendorProfileRepository
            )
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _contractRepository = contractRepository ?? throw new ArgumentNullException(nameof(contractRepository));
            _bidRepository = bidRepository ?? throw new ArgumentNullException(nameof(bidRepository));
            _procurementPlanRepository = procurementPlanRepository;
            _vendorProfileRepository = vendorProfileRepository;
        }


        /// <summary>
        /// Endpoint to summarize Total PROJECT, Total BIDS, Total CONTRACTS for a Vendor
        /// </summary>
        /// <returns></returns>
        [HttpGet("vendorOverviewSummary", Name = "SummarizeOverview")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> VendorOverviewSummary()
        {
            try
            {
                var userClaims = User.UserClaims();

                ResourceParameters parameters = new ResourceParameters();
                parameters.PageNumber = 1;
                parameters.PageSize = 5;

                //get summarised details
                var ProjectSummary = await _projectRepository.GetProjectsSummaryForVendor(userClaims.UserId);
                var ContractSummary = await _contractRepository.GetContractSummaryForVendor(userClaims.UserId);
                var bidSummary = await _bidRepository.GetVendorBidSummary(userClaims.UserId);

                var VendorRecentBids = await _bidRepository.GetRecentBidsForVendor(userClaims.UserId, parameters);
                var VendorRecentContracts = await _contractRepository.GetContractsForVendor(userClaims.UserId, parameters);
                var VendorRecentProjects = await _projectRepository.GetAllVendorProjects(userClaims.UserId, parameters);

                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId);

                var vendorOverviewSummaryDTO = new
                {
                    vendorProjectSummary = ProjectSummary,
                    vendorContractSummary = ContractSummary,
                    vendorBidSummary = bidSummary,
                    vendorRecentBids = VendorRecentBids,
                    vendorRecentContracts = VendorRecentContracts,
                    vendorRecentProjects = VendorRecentProjects,
                    vendorRegStage = user.VendorRegStage
                };

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Vendor Overview summary retrieved successfully",
                    data = vendorOverviewSummaryDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// Endpoint to summarize Total PROJECT, Total PROCUREMENT, Total VENDOR for a Staff
        /// </summary>
        /// <returns></returns>
        [HttpGet("staffOverviewSummary", Name = "SummarizeStaffOverview")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> StaffOverviewSummary()
        {
            try
            {
                var userClaims = User.UserClaims();
                var user = await _userRepository.GetByIdAsync(userClaims.UserId);

                //Summary

                var projectSummary = new ProjectsSummaryDTO();
                var procurementSummary = new ProcurementSummaryDTO();
                var vendorSummary = new VendorSummaryDto();

                if (userClaims.UserType == Domain.Enums.EUserType.VENDOR)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User is not a staff",
                        errors = new { }
                    });
                }


                if (userClaims.Role != Domain.Enums.ERole.EXECUTIVE)
                {
                    projectSummary = await _projectRepository.GetProjectsSummaryByMinistry(user.MinistryId);
                    procurementSummary = await _procurementPlanRepository.GetProcurementSummaryByMinistry(user.MinistryId);
                    vendorSummary = await _vendorProfileRepository.GetVendorSummaryDetails();
                }
                else
                {
                    projectSummary = await _projectRepository.GetProjectsSummary();
                    procurementSummary = await _procurementPlanRepository.GetProcurementSummary();
                    vendorSummary = await _vendorProfileRepository.GetVendorSummaryDetails();
                }

                var staffOverviewSummaryDTO = new
                {
                    projectSummary,
                    procurementSummary,
                    vendorSummary,
                };

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Staff Overview summary retrieved successfully",
                    data = staffOverviewSummaryDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }



    }
}
