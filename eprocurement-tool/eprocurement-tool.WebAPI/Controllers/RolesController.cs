using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EGPS.WebAPI.Controllers
{
    [Route("api/v1/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private const string USER_IP_ADDRESS = "User-IP-Address";
        public RolesController(IRoleRepository roleRepository,
            IUserActivityRepository userActivityRepository,
            IHttpContextAccessor accessor,
            IMapper mapper)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<RoleResponse>>), 200)]
        public IActionResult GetRoles()
        {
            try
            {
                var userClaims = User.UserClaims();

                return Ok(new SuccessResponse<IEnumerable<RoleResponse>>
                {
                    success = true,
                    message = "Role retrieved successfully",
                    data = EnumExtension.GetRoles(),
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<RoleDTO>), 200)]
        public async Task<IActionResult> PatchRoleById(Guid id, RoleForUpdateDTO roleForUpdate)
        {
            try
            {
                var userClaims = User.UserClaims();

                var role = await _roleRepository.SingleOrDefault(x =>
                    x.Id == id &&
                    x.AccountId == userClaims.AccountId);

                var roleExist = await _roleRepository.ExistsAsync(x =>
                    x.Title == roleForUpdate.Title &&
                    x.AccountId == userClaims.AccountId);

                if (role == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Role with id {id} not found",
                        errors = new { }
                    });
                }

                if (roleExist && role.Title != roleForUpdate.Title)
                {
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your role update request failed",
                        errors = new
                        {
                            title = new[] { "Duplicate title" }
                        }
                    });
                }

                foreach (var item in roleForUpdate.Resources)
                {
                    var resource = await _roleRepository.GetResourceById(item.Id);

                    if (resource == null)
                    {
                        return BadRequest(new ErrorResponse<object>
                        {
                            success = false,
                            message = "Your role update request fail",
                            errors = new
                            {
                                resources = new[] { $"Resource id {item.Id} not found" }
                            }
                        });
                    }

                    resource.Permissions = JsonConvert.SerializeObject(item.Permissions);
                    resource.UpdatedAt = DateTime.Now;

                    _roleRepository.UpdateRoleResource(resource);
                }

                role.Title = roleForUpdate.Title;
                role.UpdatedAt = DateTime.Now;
                _roleRepository.Update(role);
                await _roleRepository.SaveChangesAsync();

                var roleDTO = _mapper.Map<RoleDTO>(role);

                var userActivity = new UserActivity
                {
                    EventType = "Role Updated",
                    UserId = userClaims.UserId,
                    ObjectClass = "ROLE",
                    ObjectId = userClaims.AccountId,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<RoleDTO>
                {
                    success = true,
                    message = "Role updated successfully",
                    data = roleDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponse<RoleDTO>), 200)]
        public async Task<IActionResult> CreateRole(RoleForCreationDTO roleForCreation)
        {
            try
            {
                var userClaims = User.UserClaims();

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your role creation request failed",
                        errors = ModelState.Error()
                    });
                }

                var roleExist = await _roleRepository.ExistsAsync(x =>
                        x.Title == roleForCreation.Title &&
                        x.AccountId == userClaims.AccountId);

                if (roleExist)
                {
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your role creation request failed",
                        errors = new
                        {
                            title = new[] { "Duplicate title" }
                        }
                    });
                }

                var role = _mapper.Map<Role>(roleForCreation);

                var roleResources = new List<RoleResource>();

                foreach (var item in roleForCreation.Resources)
                {
                    var resource = await _roleRepository.ResourceExists(item.ResourceId);

                    if (resource == null)
                    {
                        return BadRequest(new ErrorResponse<object>
                        {
                            success = false,
                            message = "Your role creation request fail",
                            errors = new
                            {
                                resources = new[] { $"Resource id {item.ResourceId} not found" }
                            }
                        });
                    }

                    roleResources.Add(new RoleResource
                    {
                        Permissions = JsonConvert.SerializeObject(item.Permissions),
                        ResourceId = item.ResourceId,
                        Resource = resource
                    });
                }

                role.AccountId = userClaims.AccountId;
                role.CreatedById = userClaims.UserId;
                role.Resources = roleResources;
                await _roleRepository.AddAsync(role);
                await _roleRepository.SaveChangesAsync();

                var roleDTO = _mapper.Map<RoleDTO>(role);

                var userActivity = new UserActivity
                {
                    EventType = "Role Created",
                    UserId = userClaims.UserId,
                    ObjectClass = "ROLE",
                    ObjectId = role.Id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<RoleDTO>
                {
                    success = true,
                    message = "Role created successfully",
                    data = roleDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteRole(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();

                var role = await _roleRepository.SingleOrDefault(x => x.AccountId == userClaims.AccountId && x.Id == id);

                if (role == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Role with id {id} not found",
                        errors = new { }
                    });
                }

                role.Deleted = true;
                role.DeletedAt = DateTime.Now;

                _roleRepository.Update(role);
                await _roleRepository.SaveChangesAsync();

                var userActivity = new UserActivity
                {
                    EventType = "Role Deleted",
                    UserId = userClaims.UserId,
                    ObjectClass = "ROLE",
                    ObjectId = id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Role deleted successfully",
                    data = new { }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<RoleDTO>), 200)]
        public async Task<IActionResult> GetRoleDetails(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();

                var role = await _roleRepository.GetRoleById(id, userClaims.AccountId);

                if (role == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Role with id {id} not found",
                        errors = new { }
                    });
                }

                var roleDTO = _mapper.Map<RoleDTO>(role);

                return Ok(new SuccessResponse<RoleDTO>
                {
                    success = true,
                    message = "Role retrieved successfully",
                    data = roleDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        #region CreateResource
        private string CreateResourceUri(RoleParamaters roleParamaters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetRoles",
                        new
                        {
                            PageNumber = roleParamaters.PageNumber - 1,
                            roleParamaters.PageSize,
                            roleParamaters.Type
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetRoles",
                        new
                        {
                            PageNumber = roleParamaters.PageNumber + 1,
                            roleParamaters.PageSize,
                            roleParamaters.Type
                        });

                default:
                    return Url.Link("GetRoles",
                        new
                        {
                            roleParamaters.PageNumber,
                            roleParamaters.PageSize,
                            roleParamaters.Type
                        });
            }

        }
        #endregion
    }
}
