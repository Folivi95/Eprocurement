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
using System.Linq;
using System.Threading.Tasks;

namespace EGPS.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/departments")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper; 
        private const string USER_IP_ADDRESS = "User-IP-Address";
        public DepartmentsController(IDepartmentRepository departmentRepository,
            IUserActivityRepository userActivityRepository,
            IUserRepository userRepository,
            IHttpContextAccessor accessor,
            IMapper mapper)
        {
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetDepartments")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<DepartmentsDTO>>>), 200)]
        public async Task<IActionResult> GetDepartments([FromQuery] DepartmentParameters departmentParameters)
        {
            try
            {
                var userClaims = User.UserClaims();
                var departments = await _departmentRepository.GetDepartments(departmentParameters, userClaims.AccountId);

                var prevLink = departments.HasPrevious ? CreateResourceUri(departmentParameters, ResourceUriType.PreviousPage) : null;
                var nextLink = departments.HasNext ? CreateResourceUri(departmentParameters, ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(departmentParameters, ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = departments.TotalPages,
                    perPage = departments.PageSize,
                    totalEntries = departments.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<DepartmentsDTO>>
                {
                    success = true,
                    message = "Departments retrieved successfully",
                    data = departments,
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

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponse<DepartmentDTO>), 200)]
        public async Task<IActionResult> CreateDepartment(DepartmentForCreationDTO departmentForCreation)
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

                var leadExists = await _userRepository.ExistsAsync(x =>
                    x.Id == departmentForCreation.LeadId &&
                    x.AccountId == userClaims.AccountId);

                var departmentExists = await _departmentRepository.ExistsAsync(x =>
                    x.Name == departmentForCreation.Name &&
                    x.AccountId == userClaims.AccountId);

                if (!leadExists)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your department creation request failed",
                        errors = new
                        {
                            LeadId = new string[] { $"Lead id {departmentForCreation.LeadId} does not exist" }
                        }
                    });
                }

                if (departmentExists)
                {
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your department creation request failed",
                        errors = new
                        {
                            name = new string[] { "Duplicate Name" }
                        }
                    });
                }

                var departmentEntity = _mapper.Map<Department>(departmentForCreation);
                departmentEntity.AccountId = userClaims.AccountId;
                departmentEntity.CreatedById = userClaims.UserId;

                await _departmentRepository.AddAsync(departmentEntity);
                await _departmentRepository.SaveChangesAsync();

                var userActivity = new UserActivity
                {
                    EventType = "Department Created",
                    UserId = userClaims.UserId,
                    ObjectClass = "DEPARTMENT",
                    ObjectId = departmentEntity.Id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);

                await _departmentRepository.AddDepartmentMemeber(departmentForCreation.LeadId, departmentEntity.Id);

                await _userActivityRepository.SaveChangesAsync();

                var departmentDTO = _mapper.Map<DepartmentDTO>(departmentEntity);

                return Ok(new SuccessResponse<DepartmentDTO>
                {
                    success = true,
                    message = "Department created successfully",
                    data = departmentDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(SuccessResponse<DepartmentDTO>), 200)]
        public async Task<IActionResult> UpdateDepartment(Guid departmentId, DepartmentForUpdateDTO departmentForUpdate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("departmentId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your department update request failed",
                        errors = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();
                var leadExists = await _userRepository.ExistsAsync(x =>
                        x.Id == departmentForUpdate.LeadId &&
                        x.AccountId == userClaims.AccountId);

                var departmentExists = await _departmentRepository.ExistsAsync(x =>
                    x.Name == departmentForUpdate.Name &&
                    x.AccountId == userClaims.AccountId);

                if (!leadExists)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your department update request failed",
                        errors = new
                        {
                            LeadId = new string[] { $"Lead id {departmentForUpdate.LeadId} does not exist" }
                        }
                    });
                }

                if (departmentExists)
                {
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your department update request failed",
                        errors = new
                        {
                            name = new string[] { "Duplicate Name" }
                        }
                    });
                }

                var departmentEntity = await _departmentRepository.SingleOrDefault(x =>
                    x.Id == departmentId &&
                    x.AccountId == userClaims.AccountId);

                if (departmentEntity == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Department with id {departmentId} not found"
                    });
                }

                _mapper.Map(departmentForUpdate, departmentEntity);
                _departmentRepository.Update(departmentEntity);
                await _departmentRepository.SaveChangesAsync();

                var departmentDTO = _mapper.Map<DepartmentDTO>(departmentEntity);

                var userActivity = new UserActivity
                {
                    EventType = "Department Updated",
                    UserId = userClaims.UserId,
                    ObjectClass = "DEPARTMENT",
                    ObjectId = departmentEntity.Id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<DepartmentDTO>
                {
                    success = true,
                    message = "Department updated successfully",
                    data = departmentDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<DepartmentDTO>), 200)]
        public async Task<IActionResult> GetDepartmentById(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();
                var departmentEntity = await _departmentRepository.SingleOrDefault(x =>
                    x.Id == id &&
                    x.AccountId == userClaims.AccountId);

                if (departmentEntity == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Department with id {id} not found",
                        errors = new { }
                    });
                }

                var departmentDTO = _mapper.Map<DepartmentDTO>(departmentEntity);

                return Ok(new SuccessResponse<DepartmentDTO>
                {
                    success = true,
                    message = "Department retrieved successfully",
                    data = departmentDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpPost]
        [Route("{id}/members")]
        public async Task<IActionResult> AddUserToDepartment(Guid id, DepartmentMembersDTO departmentMemberList)
        {
            try
            {
                var userClaims = User.UserClaims();
                var departmentEntity = await _departmentRepository.SingleOrDefault(x => x.Id == id && x.AccountId == userClaims.AccountId);

                if (departmentEntity == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Department with id {id} not found",
                        errors = new { }
                    });
                }

                if (departmentMemberList.Members != null && departmentMemberList.Members.Count > 0)
                {
                    var errors = new List<string>();
                    var errorDictionary = new Dictionary<string, List<string>>();
                    foreach (var memberId in departmentMemberList.Members)
                    {
                        var user = await _userRepository.SingleOrDefault(u => u.Id == memberId && u.AccountId == userClaims.AccountId);
                        if (user == null)
                        {
                            return BadRequest(new ErrorResponse<object>
                            {
                                success = false,
                                message = "Department members creation failed",
                                errors = new
                                {
                                    members = new[] {$"Member with id {memberId} not found"}
                                }
                            });
                        }

                        var userExist = await _departmentRepository.UserExistInDepartment(user.Id, id);
                        if (!userExist)
                        {
                            await _departmentRepository.AddDepartmentMemeber(user.Id, id);
                        }
                    }

                    await _departmentRepository.SaveChangesAsync();

                    var userActivity = new UserActivity
                    {
                        EventType = "Department Members added",
                        UserId = userClaims.UserId,
                        ObjectClass = "DEPARTMENT",
                        ObjectId = id,
                        AccountId = userClaims.AccountId,
                        IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                    };

                    await _userActivityRepository.AddAsync(userActivity);
                    await _userActivityRepository.SaveChangesAsync();
                    return Ok(new SuccessResponse<object>
                    {
                        success = true,
                        message = "Department members added successfully",
                        data = new { }
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
        public async Task<IActionResult> DeleteUserFromDepartment(Guid id, Guid userId)
        {
            try
            {
                var userClaims = User.UserClaims();
                var department = await _departmentRepository.SingleOrDefault(x => x.Id == id && x.AccountId == userClaims.AccountId);

                var user = await _userRepository.SingleOrDefault(x => x.Id == userId && x.AccountId == userClaims.AccountId);

                var departmentMember = await _departmentRepository.GetDepartmentMember(id, userId);

                if (department == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Department with id {id} not found",
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

                if (departmentMember == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {userId} does not belongs to this department",
                        errors = new { }
                    });
                }

                _departmentRepository.RemoveUserFromDepartment(departmentMember);
                await _departmentRepository.SaveChangesAsync();

                var userActivity = new UserActivity
                {
                    EventType = "Department Member Deleted",
                    UserId = userClaims.UserId,
                    ObjectClass = "DEPARTMENT",
                    ObjectId = id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Department member removed successfully",
                    data = new { }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteDepartment(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();

                var department = await _departmentRepository.SingleOrDefault(x => x.Id == id && x.AccountId == userClaims.AccountId);

                if (department == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Department with id {id} not found",
                        errors = new { }
                    });
                }

                department.Deleted = true;
                department.DeletedAt = DateTime.Now;

                _departmentRepository.Update(department);
                await _departmentRepository.SaveChangesAsync();

                var userActivity = new UserActivity
                {
                    EventType = "Department Deleted",
                    UserId = userClaims.UserId,
                    ObjectClass = "DEPARTMENT",
                    ObjectId = userClaims.UserId,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Department deleted successfully",
                    data = new { }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpGet("{id}/members", Name = "GetDepartmentMembers")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<UserMemberDTO>>>), 200)]
        public async Task<IActionResult> GetDepartmentMembers(Guid id, [FromQuery]
            DepartmentMembersParameter parameter)
        {
            try
            {
                var userClaims = User.UserClaims();
                var departmentEntity = await _departmentRepository.SingleOrDefault(x =>
                    x.Id == id &&
                    x.AccountId == userClaims.AccountId);

                if (departmentEntity == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Department with id {id} not found",
                        errors = new { }
                    });
                }
                var users = await _departmentRepository.GetMembers(id, parameter);

                var prevLink = users.HasPrevious
                    ? CreateResourceMember(parameter, ResourceUriType.PreviousPage)
                    : null;
                var nextLink = users.HasNext ? CreateResourceMember(parameter, ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceMember(parameter, ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = users.TotalPages,
                    perPage = users.PageSize,
                    totalEntries = users.TotalCount
                };

                var userDto = _mapper.Map<IEnumerable<UserMemberDTO>>(users);

                return Ok(new PagedResponse<IEnumerable<UserMemberDTO>>
                {
                    success = true,
                    message = "Department members retrieved successfully",
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
        private string CreateResourceUri(DepartmentParameters departmentParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetDepartments",
                        new
                        {
                            PageNumber = departmentParameters.PageNumber - 1,
                            departmentParameters.PageSize
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetDepartments",
                        new
                        {
                            PageNumber = departmentParameters.PageNumber + 1,
                            departmentParameters.PageSize
                        });

                default:
                    return Url.Link("GetDepartments",
                        new
                        {
                            departmentParameters.PageNumber,
                            departmentParameters.PageSize
                        });
            }

        }
        #endregion

        #region CreateResourceMember
        private string CreateResourceMember(DepartmentMembersParameter parameter, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetDepartmentMembers",
                        new
                        {
                            PageNumber = parameter.PageNumber - 1,
                            parameter.PageSize,
                            parameter.Search 
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetDepartmentMembers",
                        new
                        {
                            PageNumber = parameter.PageNumber + 1,
                            parameter.PageSize,
                            parameter.Search
                        });

                default:
                    return Url.Link("GetDepartmentMembers",
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
