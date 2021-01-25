using System;
using System.Collections.Generic;
using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using EGPS.Domain.Enums;
using EGPS.Domain.Entities;

namespace EGPS.WebAPI.Controllers
{
    /// <summary>
    /// Ministries controller
    /// </summary>

    [Route("api/v1/ministries")]
    [ApiController]
    [Authorize]
    public class MinistriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IUserRepository _userRepository;
        private readonly IMinistryRepository _ministryRepository;
        private readonly IStaffRepository _staffRepository;
        private const string USER_IP_ADDRESS = "User-IP-Address";

        public IConfiguration Configuration { get; }
        public MinistriesController(IMapper mapper,
            IJwtAuthenticationManager jwtAuthenticationManager,
            IConfiguration configuration,
            IMinistryRepository ministryRepository,
            IStaffRepository staffRepository,
            IUserRepository userRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _jwtAuthenticationManager = jwtAuthenticationManager ?? throw new ArgumentNullException(nameof(jwtAuthenticationManager));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _ministryRepository = ministryRepository ?? throw new ArgumentNullException(nameof(ministryRepository));
            _staffRepository = staffRepository ?? throw new ArgumentNullException(nameof(staffRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// Endpoint to get all ministries
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllMinistries")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<MinistryDTO>>>), 200)]
        public async Task<IActionResult> GetAllMinistries([FromQuery] MinistryParameters parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                //check if user exist in the system
                if (!(await _userRepository.ExistsAsync(x => x.Id == userClaims.UserId)))
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User {userClaims.UserId} is does not exist",
                        errors = new { }
                    });
                }

                //get list of all ministries
                var ministries = await _ministryRepository.GetAllMinistriesByUserId(parameters, userClaims.UserId);


                //map staffUsers to StaffUsersDto
                var ministriesDto = _mapper.Map<IEnumerable<MinistryDTO>>(ministries);

                var ministriesToReturn = await _ministryRepository.TotalBidsForMinistry(ministriesDto, parameters);

                var prevLink = ministries.HasPrevious ? CreateResourceUri(parameters, "GetAllMinistries", ResourceUriType.PreviousPage) : null;
                var nextLink = ministries.HasNext ? CreateResourceUri(parameters, "GetAllMinistries", ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(parameters, "GetAllMinistries", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = ministries.TotalPages,
                    perPage = ministries.PageSize,
                    totalEntries = ministries.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<MinistryDTO>>
                {
                    success = true,
                    message = "ministries retrieved successfully",
                    data = ministriesDto,
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
        /// Endpoint to add a new Ministry
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "AddMinistries")]
        [ProducesResponseType(typeof(SuccessResponse<List<MinistryDTO>>), 200)]
        public async Task<IActionResult> CreateMinistry(List<MinistryForCreationDTO> model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your create Ministry post request failed",
                        errors = ModelState.Error()
                    });
                }

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

                List<Ministry> minitries = new List<Ministry>();

                foreach (var ministryItem in model)
                {
                    var ministry = new Ministry
                    {
                        Name = ministryItem.Name,
                        Email = ministryItem.Email,
                        Code = ministryItem.Code,
                        CreatedById = userClaims.UserId,
                        AccountId = userClaims.AccountId
                    };

                    minitries.Add(ministry);
                }

                await _ministryRepository.AddRangeAsync(minitries);
                await _ministryRepository.SaveChangesAsync();

                var ministryDTO = _mapper.Map<List<MinistryDTO>>(minitries);

                return Ok(new SuccessResponse<List<MinistryDTO>>
                {
                    success = true,
                    message = "ministries created successfully",
                    data = ministryDTO
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to search and get all vendors in a ministry
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parameters"></param>
        [HttpGet("{id}/vendors", Name = "GetVendorsInMinistry")]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<VendorProfile>>), 200)]
        public async Task<IActionResult> GetVendorsInMinistry(Guid id, [FromQuery] VendorParameters parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                var findMinistry = await _ministryRepository.ExistsAsync(a => a.Id == id);

                if (!findMinistry)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"ministry with id {id} not found",
                        errors = new { }
                    });
                }

                var vendorProfiles = await _ministryRepository.GetVendors(id, parameters);

                var prevLink = vendorProfiles.HasPrevious
                    ? CreateResourceUri(parameters, "GetVendorsInMinistry", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = vendorProfiles.HasNext
                    ? CreateResourceUri(parameters, "GetVendorsInMinistry", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "GetVendorsInMinistry", ResourceUriType.CurrentPage);



                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = vendorProfiles.TotalPages,
                    perPage = vendorProfiles.PageSize,
                    totalEntries = vendorProfiles.TotalCount
                };
                var response = new PagedResponse<IEnumerable<VendorProfile>>
                {
                    success = true,
                    message = "Vendors retrieved successfully",
                    data = vendorProfiles,
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
        /// An endpoint to get all users in a ministry
        /// </summary>
        /// <param name="ministryId"></param>
        /// <param name="parameters"></param>
        [HttpGet("{ministryId}/users", Name = nameof(GetUserInMinistry))]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<UserDTO>>), 200)]
        public async Task<IActionResult> GetUserInMinistry(Guid ministryId, [FromQuery] ResourceParameters parameters)
        {
            var findMinistry = await _ministryRepository.ExistsAsync(a => a.Id == ministryId);

            if (!findMinistry)
            {
                return NotFound(new ErrorResponse<object>
                {
                    success = false,
                    message = $"ministry with id {ministryId} not found",
                    errors = new { }
                });
            }

            var query = await _ministryRepository.GetUsersByMinistry(ministryId, parameters);
            var data = _mapper.Map<List<UserDTO>>(query);
            var prevLink = query.HasPrevious
                ? CreateResourceUri(parameters, nameof(GetUserInMinistry), ResourceUriType.PreviousPage)
                : null;
            var nextLink = query.HasNext
                ? CreateResourceUri(parameters, nameof(GetUserInMinistry), ResourceUriType.NextPage)
                : null;
            var currentLink = CreateResourceUri(parameters, nameof(GetUserInMinistry), ResourceUriType.CurrentPage);



            var pagination = new Pagination
            {
                currentPage = currentLink,
                nextPage = nextLink,
                previousPage = prevLink,
                totalPages = query.TotalPages,
                perPage = query.PageSize,
                totalEntries = query.TotalCount
            };
            var response = new PagedResponse<List<UserDTO>>
            {
                success = true,
                message = "Vendors retrieved successfully",
                data = data,
                meta = new Meta
                {
                    pagination = pagination
                }
            };
            return Ok(response);


        }


        /// <summary>
        /// An endpoint to get all users in a ministry by loggedInUser
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="search"></param>
        [HttpGet("users", Name = nameof(GetLoggedUserInMinistry))]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<UserDTO>>), 200)]
        public async Task<IActionResult> GetLoggedUserInMinistry([FromQuery] ResourceParameters parameters, [FromQuery] string search = "")
        {
            var userClaims = User.UserClaims();
            if (userClaims.Role == ERole.EXECUTIVE)
            {
                var query = await _userRepository.GetUsers(parameters, search);
                var data = _mapper.Map<List<UserDTO>>(query);
                var prevLink = query.HasPrevious
                    ? CreateResourceUri(parameters, nameof(GetUserInMinistry), ResourceUriType.PreviousPage)
                    : null;
                var nextLink = query.HasNext
                    ? CreateResourceUri(parameters, nameof(GetUserInMinistry), ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, nameof(GetUserInMinistry), ResourceUriType.CurrentPage);
                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = query.TotalPages,
                    perPage = query.PageSize,
                    totalEntries = query.TotalCount
                };
                var response = new PagedResponse<List<UserDTO>>
                {
                    success = true,
                    message = "Vendors retrieved successfully",
                    data = data,
                    meta = new Meta
                    {
                        pagination = pagination
                    }
                };
                return Ok(response);
            }
            else
            {
                var findUser = await _userRepository.GetUserDetail(userClaims.Email);
                var query = await _ministryRepository.GetUsersByMinistry(findUser.MinistryId, parameters);
                var data = _mapper.Map<List<UserDTO>>(query);
                var prevLink = query.HasPrevious
                    ? CreateResourceUri(parameters, nameof(GetUserInMinistry), ResourceUriType.PreviousPage)
                    : null;
                var nextLink = query.HasNext
                    ? CreateResourceUri(parameters, nameof(GetUserInMinistry), ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, nameof(GetUserInMinistry), ResourceUriType.CurrentPage);
                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = query.TotalPages,
                    perPage = query.PageSize,
                    totalEntries = query.TotalCount
                };
                var response = new PagedResponse<List<UserDTO>>
                {
                    success = true,
                    message = "Vendors retrieved successfully",
                    data = data,
                    meta = new Meta
                    {
                        pagination = pagination
                    }
                };
                return Ok(response);
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
