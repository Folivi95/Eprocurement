using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EGPS.WebAPI.Controllers
{
    [Route("api/v1/units")]
    [ApiController]
    [Authorize]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private const string USER_IP_ADDRESS = "User-IP-Address";

        public UnitsController(IUnitRepository unitRepository,
            IMapper mapper,
            IHttpContextAccessor accessor,
            IUserActivityRepository userActivityRepository,
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository)
        {
            _unitRepository = unitRepository ?? throw new ArgumentNullException(nameof(unitRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponse<UnitDTO>), 200)]
        public async Task<IActionResult> CreateUnit(UnitForCreationDTO unitForCreation) 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your unit creation request failed",
                        errors  = ModelState.Error()
                    });
                }
                
                var userClaims = User.UserClaims();

                var departmentExist = await _departmentRepository.ExistsAsync(u => u.Id == unitForCreation.DepartmentId && u.AccountId == userClaims.AccountId);

                var leadExists = await _userRepository.ExistsAsync(u => u.Id == unitForCreation.LeadId && u.AccountId == userClaims.AccountId);

                var unitExists = await _unitRepository.ExistsAsync(u => u.Name == unitForCreation.Name && u.AccountId == userClaims.AccountId && u.DepartmentId == unitForCreation.DepartmentId);

                if (!leadExists)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your unit creation request failed",
                        errors = new
                        {
                            LeadId = new string[] {$"Lead id {unitForCreation.LeadId} does not exist"}
                        }
                    });
                }

                if (!departmentExist)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your unit creation request failed",
                        errors = new
                        {
                            DepartmentId = new string[] {$"Department id {unitForCreation.DepartmentId} does not exist"}
                        }
                    });
                }

                if (unitExists)
                {
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your unit creation request failed",
                        errors = new
                        {
                            title = new string[] { "Duplicate Name" }
                        }
                    });
                }

                var unit = new Unit
                {
                    AccountId = userClaims.AccountId,
                    Website = unitForCreation.Website,
                    Name = unitForCreation.Name,
                    Description = unitForCreation.Description,
                    LeadId = unitForCreation.LeadId,
                    CreatedById = userClaims.UserId,
                    DepartmentId = unitForCreation.DepartmentId
                };
                await _unitRepository.AddAsync(unit);
                await _userRepository.SaveChangesAsync();

                var userActivity = new UserActivity
                {
                    EventType   = "Unit Created",
                    UserId      = userClaims.UserId,
                    ObjectClass = "UNIT",
                    ObjectId    = unit.Id,
                    AccountId   = userClaims.AccountId,
                    IpAddress   = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);

                await _unitRepository.AddUnitMemeber(unitForCreation.LeadId, unit.Id);

                await _userActivityRepository.SaveChangesAsync();

                var unitDto = _mapper.Map<UnitDTO>(unit);

                return Ok(new SuccessResponse<UnitDTO>
                {
                    success = true,
                    message = "Unit created successfully",
                    data    = unitDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<UnitDTO>), 200)]
        public async Task<IActionResult> UpdateUnit(Guid id, UnitForUpdateDTO unitForUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("unitId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your unit update request failed",
                        errors = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();

                var departmentExist = await _departmentRepository.ExistsAsync(u => u.Id == unitForUpdate.DepartmentId && u.AccountId == userClaims.AccountId);

                var leadExists = await _userRepository.ExistsAsync(u => u.Id == unitForUpdate.LeadId && u.AccountId == userClaims.AccountId);

                var unitExists = await _unitRepository.ExistsAsync(u => u.Name == unitForUpdate.Name && u.AccountId == userClaims.AccountId);

                var unit = await _unitRepository.SingleOrDefault(u => u.Id == id && u.AccountId == userClaims.AccountId);

                if (!leadExists)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your unit update request failed",
                        errors = new
                        {
                            LeadId = new string[] { $"Lead id {unitForUpdate.LeadId} does not exist" }
                        }
                    });
                }

                if (!departmentExist)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your unit update request failed",
                        errors = new
                        {
                            DepartmentId = new string[] { $"Department id {unitForUpdate.DepartmentId} does not exist" }
                        }
                    });
                }

                if (unitExists)
                {
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your unit update request failed",
                        errors = new
                        {
                            title = new string[] { "Duplicate Name" }
                        }
                    });
                }

                if (unit == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Unit with id {id} not found",
                        errors = new { }
                    });
                }

                var userActivity = new UserActivity
                {
                    EventType = "Unit Updated",
                    UserId = userClaims.UserId,
                    ObjectClass = "UNIT",
                    ObjectId = unit.Id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                _mapper.Map(unitForUpdate, unit);
                _unitRepository.Update(unit);
                await _userRepository.SaveChangesAsync();

                var unitDto = _mapper.Map<UnitDTO>(unit);
                return Ok(new SuccessResponse<UnitDTO>
                {
                    success = true,
                    message = "Unit updated successfully",
                    data = unitDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpGet(Name = "GetUnits")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<UnitsDTO>>>), 200)]
        public async Task<IActionResult> GetUnits([FromQuery] UnitParameters unitParameters)
        {
            try
            {
                var userClaims = User.UserClaims();
                var units      = await _unitRepository.GetUnits(unitParameters, userClaims.AccountId);

                var prevLink = units.HasPrevious
                    ? CreateResourceUri(unitParameters, ResourceUriType.PreviousPage)
                    : null;
                var nextLink    = units.HasNext ? CreateResourceUri(unitParameters, ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(unitParameters, ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage  = currentLink,
                    nextPage     = nextLink,
                    previousPage = prevLink,
                    totalPages   = units.TotalPages,
                    perPage      = units.PageSize,
                    totalEntries = units.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<UnitsDTO>>
                {
                    success = true,
                    message = "Units retrieved successfully",
                    data    = units,
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

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> SoftDeleteUnit(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();

                var unit = await _unitRepository.SingleOrDefault(u => u.Id == id && u.AccountId == userClaims.AccountId && !u.Department.Deleted);

                if (unit == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Unit with id {id} not found",
                        errors = new { }
                    });
                }

                unit.Deleted = true;
                unit.DeletedAt = DateTime.Now;
                _unitRepository.Update(unit);
                await _unitRepository.SaveChangesAsync();

                var userActivity = new UserActivity
                {
                    EventType = "Unit Deleted",
                    UserId = userClaims.UserId,
                    ObjectClass = "UNIT",
                    ObjectId = unit.Id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();
                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Unit deleted successfully",
                    data = new { }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<UnitDTO>), 200)]
        public async Task<IActionResult> GetUnitById(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();
                var unit = await _unitRepository.SingleOrDefault(u => u.Id == id && u.AccountId == userClaims.AccountId && !u.Department.Deleted);

                if (unit == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Unit with id {id} not found",
                        errors = new { }
                    });
                }

                var unitDto = _mapper.Map<UnitDTO>(unit);

                return Ok(new SuccessResponse<UnitDTO>
                {
                    success = true,
                    message = "Unit retrieved successfully",
                    data = unitDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpPost]
        [Route("{id}/members")]
        public async Task<IActionResult> AddUserToDepartment(Guid id, UnitMembersDTO memberList)
        {
            try
            {
                var userClaims = User.UserClaims();

                var unit = await _unitRepository.SingleOrDefault(u => u.Id == id && u.AccountId == userClaims.AccountId && !u.Department.Deleted);

                if (unit == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Unit with id {id} not found",
                        errors = new { }
                    });
                }

                if (memberList.Members != null && memberList.Members.Count > 0)
                {
                    var errors = new List<string>();
                    var errorDictionary = new Dictionary<string, List<string>>();
                    foreach (var memberId in memberList.Members)
                    {
                        var user = await _userRepository.SingleOrDefault(u => u.Id == memberId && u.AccountId == userClaims.AccountId);
                        if (user == null)
                        {
                            return BadRequest(new ErrorResponse<object>
                            {
                                success = false,
                                message = "Unit members creation failed",
                                errors = new
                                {
                                    members = new[] {$"Member with id {memberId} not found"}
                                }
                            });
                        }

                        var userExists = await _unitRepository.UserExistInUnit(user.Id, id);
                        if (!userExists)
                        {
                            await _unitRepository.AddUnitMemeber(user.Id, id);
                        }
                    }

                    await _unitRepository.SaveChangesAsync();

                    var userActivity = new UserActivity
                    {
                        EventType   = "Unit Members Created",
                        UserId      = userClaims.UserId,
                        ObjectClass = "UNIT",
                        ObjectId    = id,
                        AccountId   = userClaims.AccountId,
                        IpAddress   = Request.GetHeader(USER_IP_ADDRESS)
                    };

                    await _userActivityRepository.AddAsync(userActivity);
                    await _userActivityRepository.SaveChangesAsync();
                    return Ok(new SuccessResponse<object>
                    {
                        success = true,
                        message = "Unit members created successfully",
                        data    = new { }
                    });
                }

                return BadRequest(new ErrorResponse<object>
                {
                    success = false,
                    message = "members cannot not be null",
                    errors  = new { }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        [HttpDelete]
        [Route("{id}/members/{userId}")]
        public async Task<IActionResult> DeleteUserFromUnit(Guid id, Guid userId)
        {
            try
            {
                var userClaims = User.UserClaims();
                var unit = await _unitRepository.SingleOrDefault(u => u.Id == id && u.AccountId == userClaims.AccountId && !u.Department.Deleted);

                var user = await _userRepository.SingleOrDefault(x => x.Id == userId && x.AccountId == userClaims.AccountId);

                var unitMember = await _unitRepository.GetUnitMember(id, userId);

                if (unit == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Unit with id {id} not found",
                        errors = new { }
                    });
                }

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {userId} not found",
                        errors = new { }
                    });
                }

                if (unitMember == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {userId} does not belongs to this department",
                        errors = new { }
                    });
                }

                _unitRepository.RemoveUserFromUnit(unitMember);
                await _unitRepository.SaveChangesAsync();

                var userActivity = new UserActivity
                {
                    EventType = "Unit Member Deleted",
                    UserId = userClaims.UserId,
                    ObjectClass = "UNIT",
                    ObjectId = id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Unit member removed successfully",
                    data = new { }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpGet("{id}/members", Name = "GetUnitMembers")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<UserDTO>>>), 200)]
        public async Task<IActionResult> GetUnitMemebers(Guid id, [FromQuery] UnitMembersParameter parameter)
        {
            try
            {
                var userClaims = User.UserClaims();
                var unit = await _unitRepository.SingleOrDefault(x => x.Id == id && x.AccountId == userClaims.AccountId && !x.Department.Deleted);

                if (unit == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Unut with id {id} not found",
                        errors = new { }
                    });
                }
                var users = await _unitRepository.GetMembers(id, parameter);

                var prevLink = users.HasPrevious
                    ? CreateResourceMemeber(parameter, ResourceUriType.PreviousPage)
                    : null;
                var nextLink = users.HasNext ? CreateResourceMemeber(parameter, ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceMemeber(parameter, ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = users.TotalPages,
                    perPage= users.PageSize,
                    totalEntries = users.TotalCount
                };

                var userDto = _mapper.Map<IEnumerable<UserDTO>>(users);

                return Ok(new PagedResponse<IEnumerable<UserDTO>>
                {
                    success = true,
                    message = "Unit members retrieved successfully",
                    data = userDto,
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
        private string CreateResourceUri(UnitParameters unitParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetUnits",
                        new
                        {
                            PageNumber = unitParameters.PageNumber - 1,
                            unitParameters.PageSize,
                            unitParameters.DepartmentId,
                            unitParameters.Search
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetUnits",
                        new
                        {
                            PageNumber = unitParameters.PageNumber + 1,
                            unitParameters.PageSize,
                            unitParameters.DepartmentId,
                            unitParameters.Search
                        });

                default:
                    return Url.Link("GetUnits",
                        new
                        {
                            unitParameters.PageNumber,
                            unitParameters.PageSize,
                            unitParameters.DepartmentId,
                            unitParameters.Search
                        });
            }

        }
        #endregion

        #region CreateResourceMember
        private string CreateResourceMemeber(UnitMembersParameter parameter, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetUnitMembers",
                        new
                        {
                            PageNumber = parameter.PageNumber - 1,
                            parameter.PageSize,
                            parameter.Search
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetUnitMembers",
                        new
                        {
                            PageNumber = parameter.PageNumber + 1,
                            parameter.PageSize,
                            parameter.Search
                        });

                default:
                    return Url.Link("GetUnitMembers",
                        new
                        {
                            parameter.PageNumber,
                            parameter.PageSize,
                            parameter.Search
                        });
            }

        }
        #endregion
    }
}
