using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Application.Models.StaffModels;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EGPS.WebAPI.Controllers
{
    /// <summary>
    /// Staffs controller
    /// </summary>


    [Route("api/v1/staffs")]
    [ApiController]
    [Authorize]
    public class StaffsController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IEmailSender _emailSender;
        private readonly IRoleRepository _roleRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserInvitationRepository _userInvitationRepository;
        private readonly IMapper _mapper;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IMinistryRepository _ministryRepository;
        private readonly IEmailTemplate _emailTemplate;
        private readonly IVendorProfileRepository _vendorProfileRepository;
        private const string USER_IP_ADDRESS = "User-IP-Address";

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="staffRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="userRepository"></param>
        /// <param name="userActivityRepository"></param>
        /// <param name="configuration"></param>
        /// <param name="jwtAuthenticationManager"></param>
        /// <param name="roleRepository"></param>
        /// <param name="userRoleRepository"></param>
        /// <param name="emailSender"></param>
        /// <param name="accountRepository"></param>
        /// <param name="userInvitationRepository"></param>
        /// <param name="ministryRepository"></param>
        /// <param name="emailTemplate"></param>
        /// <param name="vendorProfileRepository"></param>

        public StaffsController(IMapper mapper,
            IConfiguration configuration,
            IUserActivityRepository userActivityRepository,
            IUserRepository userRepository,
            IStaffRepository staffRepository,
            IJwtAuthenticationManager jwtAuthenticationManager,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IEmailSender emailSender,
            IAccountRepository accountRepository,
            IUserInvitationRepository userInvitationRepository,
            IMinistryRepository ministryRepository,
            IEmailTemplate emailTemplate,
            IVendorProfileRepository vendorProfileRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userActivityRepository =
                userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _staffRepository = staffRepository ?? throw new ArgumentNullException(nameof(staffRepository));
            _jwtAuthenticationManager = jwtAuthenticationManager ??
                                        throw new ArgumentNullException(nameof(jwtAuthenticationManager));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _userRoleRepository = userRoleRepository ?? throw new ArgumentNullException(nameof(userRoleRepository));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _userInvitationRepository = userInvitationRepository ??
                                        throw new ArgumentNullException(nameof(userInvitationRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _ministryRepository = ministryRepository ?? throw new ArgumentNullException(nameof(ministryRepository));
            _emailTemplate = emailTemplate ?? throw new ArgumentNullException(nameof(emailTemplate));
            _vendorProfileRepository = vendorProfileRepository ?? throw new ArgumentNullException(nameof(vendorProfileRepository));
        }


        private static bool IsEmailValid(string email)
        {
            try
            {
                var m = new MailAddress(email);

                return false;
            }
            catch (FormatException)
            {
                return true;
            }
        }

        public IConfiguration Configuration { get; }


        /// <summary>
        /// Endpoint to Register a Staff
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>        
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserResponse<UserDTO>), 200)]
        public async Task<IActionResult> RegisterUser(UserForCreationDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                {
                    success = false,
                    message = "Your user registration failed",
                    errors = ModelState.Error()
                });
            }

            var userEntity = await _userRepository.SingleOrDefault(x => x.Email == user.Email);

            if (userEntity == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Your user registration failed",
                    errors = new { email = new string[] { "Enter a valid value" } }
                });
            }

            var userInvitation = await _userRepository.GetUserInvitation(user.Email, user.Token);

            if (userInvitation == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Your Invitation link is invalid",
                    errors = new { }
                });
            }

            userEntity.EmailVerified = true;
            userEntity.Status = EStatus.ENABLED;
            userEntity.FirstName = user.FirstName;
            userEntity.LastName = user.LastName;
            userEntity.JobTitle = user.Phone;
            userEntity.UserType = EUserType.STAFF;
            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var userDTO = _mapper.Map<UserDTO>(userEntity);
            userInvitation.Status = false;

            try
            {
                _userRepository.UpdateUserInvitation(userInvitation);
                await _userRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }

            var userActivity = new UserActivity
            {
                EventType = "User Signup",
                UserId = userEntity.Id,
                AccountId = userEntity.AccountId,
                ObjectClass = "USER",
                ObjectId = userEntity.Id,
                IpAddress = Request.GetHeader(USER_IP_ADDRESS)
            };

            try
            {
                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }

            var (Token, ExpiresIn) = _jwtAuthenticationManager.Authenticate(userEntity);

            return Ok(new
            {
                success = true,
                message = "Your email verification link is valid",
                data = new UserData<UserDTO>
                {
                    user = userDTO,
                    token = Token,
                    expiresIn = ExpiresIn
                }
            });
        }




        /// <summary>
        /// Endpoint to invite a Staff
        /// </summary>
        /// <param name="userInviteDTO"></param>
        /// <returns></returns>
        [HttpPost("invite")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> InviteUser(UserInvitationForCreationDTO[] userInviteDTO)
        {
            try
            {
                var userClaims = User.UserClaims();

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your user invitation request failed",
                        errors = ModelState.Error()
                    });
                }

                var user = new User();

                foreach (var item in userInviteDTO)
                {
                    var userActiveEmailExist = _userRepository.Exists(
                    x => x.Email == item.Email &&
                         x.Status == EStatus.ENABLED);

                    if (userActiveEmailExist)
                    {
                        return Conflict(new
                        {
                            success = false,
                            message = "Your user invitation request failed",
                            errors = new
                            {
                                email = new[] { "Email already exist" }
                            }
                        });
                    }

                    var userPendingEmailExist = _userRepository.Exists(
                        x => x.Email == item.Email &&
                             x.Status == EStatus.DISABLED);

                    if (userPendingEmailExist)
                    {
                        return Conflict(new
                        {
                            success = false,
                            message = "Your user invitation request failed",
                            errors = new
                            {
                                email = new[] { "Email already exist within your organization" }
                            }
                        });
                    }

                    //var roleExist = await _roleRepository.ExistsAsync(x => x.Id == item.RoleId && x.Type == "SYSTEM");

                    // if (!roleExist)
                    // {
                    //     return NotFound(new
                    //     {
                    //         success = false,
                    //         message = "Your user invitation request failed",
                    //         errors = new
                    //         {
                    //             roleId = new[] { $"The role with {item.RoleId} does not exist" }
                    //         }
                    //     });
                    // }

                    if (item.MinistryId != null)
                    {
                        var minstryExist = await _ministryRepository.ExistsAsync(x => x.Id == item.MinistryId);

                        if (!minstryExist)
                        {
                            return NotFound(new
                            {
                                success = false,
                                message = "Your user invitation request failed",
                                errors = new
                                {
                                    MinistryId = new[] { $"The Ministry with {item.MinistryId} does not exist" }
                                }
                            });
                        }

                        user.MinistryId = item.MinistryId;
                    }

                    var invitationToken = CustomToken.GenerateToken();
                    var userInviteEntity = new UserInvitation
                    {
                        AccountId = userClaims.AccountId,
                        Token = invitationToken,
                        Email = item.Email
                    };
                    await _userRepository.CreateUserInvitation(userInviteEntity);

                    // The assigned strings are passed to pass the validation
                    // on the UserForCreationDtoValidator. 
                    // It will be updated with its correct user details
                    // in the user signups.
                    user.FirstName = "user";
                    user.LastName = "user";
                    user.JobTitle = "user";
                    user.Password = "passwordxoxo";
                    user.Email = item.Email;
                    user.AccountId = userClaims.AccountId;
                    user.Status = EStatus.DISABLED;
                    user.Role = (ERole)item.RoleId;

                    await _userRepository.AddAsync(user);


                    // var systemRoles = new UserRole
                    // {
                    //     UserId = user.Id,
                    //     RoleId = item.RoleId
                    // };
                    // await _userRoleRepository.AddAsync(systemRoles);

                    //var customRoles = new List<UserRole>();

                    //THe commented part is for custom role
                    // if (userInviteDTO.CustomRoleIds != null && userInviteDTO.CustomRoleIds.Length > 0)
                    // {
                    //     foreach (var role in userInviteDTO.CustomRoleIds)
                    //     {
                    //         var customRoleId = await _roleRepository.ExistsAsync(x =>
                    //             x.AccountId == userClaims.AccountId && x.Id == role && x.Type == "CUSTOM");
                    //
                    //         if (!customRoleId)
                    //         {
                    //             return NotFound(new
                    //             {
                    //                 success = false,
                    //                 message = "Your user invitation request failed",
                    //                 errors = new
                    //                 {
                    //                     roleId = new[] { $"The role with {userInviteDTO.RoleId} does not exist" }
                    //                 }
                    //             });
                    //         }
                    //
                    //         customRoles.Add(new UserRole { RoleId = role, UserId = user.Id });
                    //     }
                    // }

                    //await _userRoleRepository.AddRangeAsync(customRoles);

                    string filePath = Path.Combine(Environment.CurrentDirectory, @"wwwroot/AppData", "Templates");
                    string htmlPath = $@"{filePath}/UserReInvitation.html";
                    var verificationLink = $"{Configuration["BASE_URL"]}/profile-setup?token={invitationToken}&email={user.Email}";
                    var body = _emailTemplate.GetEmailTemplate(verificationLink, htmlPath);

                    var notification = new Notification
                    {
                        UserId = user.Id,
                        AccountId = userClaims.AccountId,
                        NotificationType = ENotificationType.Email,
                        Recipient = item.Email,
                        Subject = "User Invitation",
                        Body = body,
                        TemplateId = Configuration["EMAIL_VERIFICATION_TEMPLATE_ID"]
                    };

                    await _emailSender.SendEmailAsync("User Invitation", body, item.Email, notification);

                    var userActivity = new UserActivity
                    {
                        EventType = "User Invite",
                        UserId = user.Id,
                        ObjectClass = "USER",
                        ObjectId = user.Id,
                        AccountId = userClaims.AccountId,
                        IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                    };

                    await _userActivityRepository.AddAsync(userActivity);
                    await _userActivityRepository.SaveChangesAsync();


                }


                return Ok(new
                {
                    success = true,
                    message = "Your user invitation request was successful",
                    data = new { }
                });
            }

            catch (Exception ex)
            {

                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to resend Invitation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("resendInvitation")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> ResendInvitation([FromBody]ResendInvitationMailDTO[] model)
        {
            try
            {
                var userClaims = User.UserClaims();

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your user invitation email resend request failed",
                        errors = ModelState.Error()
                    });
                }

                //var userInvite = await _userInvitationRepository.SingleOrDefault(u =>
                //    u.AccountId == userClaims.AccountId && u.Email == model.Email);
                //var user = await _userRepository.SingleOrDefault(u =>
                //    u.AccountId == userClaims.AccountId && u.Email == model.Email);

                foreach (var item in model)
                {
                    var userInvite = await _userInvitationRepository.SingleOrDefault(u => u.Email == item.Email);
                    var user = await _userRepository.SingleOrDefault(u => u.Email == item.Email && u.Status == EStatus.DISABLED);

                    var error = new Dictionary<string, string[]>();

                    if (userInvite == null || user == null)
                    {
                        error.Add("email", new[] { "Email does not exist" });
                        return NotFound(new ErrorResponse<Dictionary<string, string[]>>
                        {
                            success = false,
                            message = "Your user invitation email resend request failed",
                            errors = error
                        });
                    }

                    var account = await _accountRepository.GetByIdAsync(userClaims.AccountId);

                    if (account == null)
                    {
                        error.Add("email", new[] { "Account does not exist" });
                        return NotFound(new ErrorResponse<Dictionary<string, string[]>>
                        {
                            success = false,
                            message = "Your user invitation email resend request failed",
                            errors = error
                        });
                    }

                    if (!userInvite.Status || account.CreatedById == user.Id)
                    {
                        error.Add("email", new[] { "Email already accepted invitation" });
                        return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                        {
                            success = false,
                            message = "Your user invitation email resend request failed",
                            errors = error
                        });
                    }


                    string filePath = Path.Combine(Environment.CurrentDirectory, @"wwwroot/AppData", "Templates");
                    string htmlPath = $@"{filePath}/UserReInvitation.html";
                    var verificationLink = $"{Configuration["BASE_URL"]}/profile-setup?token={userInvite.Token}&email={user.Email}";
                    var body = _emailTemplate.GetEmailTemplate(verificationLink, htmlPath);

                    var notification = new Notification
                    {
                        UserId = user.Id,
                        AccountId = userClaims.AccountId,
                        NotificationType = ENotificationType.Email,
                        Recipient = item.Email,
                        Subject = "User Invitation",
                        Body = body,
                        TemplateId = Configuration["EMAIL_VERIFICATION_TEMPLATE_ID"]
                    };


                    await _emailSender.SendEmailAsync("User Invitation", body, item.Email, notification);


                }//end foreach statement


                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "User invitation email sent successfully",
                    data = new { }
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }




        /// <summary>
        /// Endpoint to get all staffs
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllStaffs")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<StaffUserDto>>>), 200)]
        public async Task<IActionResult> GetStaffs([FromQuery] StaffParameters staffParameters)
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

                //get list of all staff users
                var staffUsers = await _staffRepository.GetAllStaffs(staffParameters, userClaims.UserId);


                //map staffUsers to StaffUsersDto
                var staffUsersDto = _mapper.Map<IEnumerable<StaffUserDto>>(staffUsers);

                var prevLink = staffUsers.HasPrevious
                    ? CreateResourceUri(staffParameters, "GetAllStaffs", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = staffUsers.HasNext
                    ? CreateResourceUri(staffParameters, "GetAllStaffs", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(staffParameters, "GetAllStaffs", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = staffUsers.TotalPages,
                    perPage = staffUsers.PageSize,
                    totalEntries = staffUsers.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<StaffUserDto>>
                {
                    success = true,
                    message = "staffs retrieved successfully",
                    data = staffUsersDto,
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
        /// Endpoint to update a staff
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<StaffUserDto>), 200)]
        public async Task<IActionResult> UpdateStaff([FromForm] StaffForUpdateDTO staffForUpdate, Guid id)
        {
            try
            {
                //Get userClaims from token
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u =>
                    u.Id == id && u.UserType == EUserType.STAFF);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {id} not found",
                        errors = new { }
                    });
                }

                if (staffForUpdate.MinistryId != null)
                {
                    var ministry = await _ministryRepository.SingleOrDefault(u => u.Id == staffForUpdate.MinistryId);

                    if (ministry == null)
                    {
                        return NotFound(new ErrorResponse<object>
                        {
                            success = false,
                            message = $"Ministry with id {id} not found",
                            errors  = new { }
                        });
                    }

                    user.MinistryId = staffForUpdate.MinistryId.Value;
                    user.Ministry = ministry;
                }
                

               
                user.Status = staffForUpdate.Status ?? user.Status;
                user.Role = staffForUpdate.Role ?? user.Role;

                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();

                //Create, add and save new UserActivity
                var userActivity = new UserActivity
                {
                    EventType = $"Update Staff",
                    UserId = user.Id,
                    ObjectClass = "STAFF",
                    ObjectId = user.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                var staffUserDto = _mapper.Map<StaffUserDto>(user);

                return Ok(new SuccessResponse<StaffUserDto>
                {
                    success = true,
                    message = "Staff updated successfully",
                    data = staffUserDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to summarize staffs data
        /// </summary>
        /// <returns></returns>
        [HttpGet("summary", Name = "SummarizeStaffsData")]
        [ProducesResponseType(typeof(SuccessResponse<StaffSummaryDto>), 200)]
        public async Task<IActionResult> SummarizeStaffsData()
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

                //get summarised details
                var staffSummary = await _staffRepository.GetStaffSummaryDetails();

                return Ok(new SuccessResponse<StaffSummaryDto>
                {
                    success = true,
                    message = "Staffs summary retrieved successfully",
                    data = staffSummary
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// Endpoint to get a list of pending invites
        /// </summary>
        /// <returns></returns>
        [HttpGet("pendingInvites", Name = "GetPendingInvites")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<UserInvitationDTO>>>), 200)]
        public async Task<IActionResult> GetPendingInvites([FromQuery] ResourceParameters parameters)
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

                //get list of all pending invites
                var pendingInvites = await _userInvitationRepository.GetPendingInvites(parameters);



                var pendingInvitesDto = _mapper.Map<IEnumerable<UserInvitationDTO>>(pendingInvites);

                var prevLink = pendingInvites.HasPrevious
                    ? CreateResourceUri(parameters, "GetPendingInvites", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = pendingInvites.HasNext
                    ? CreateResourceUri(parameters, "GetPendingInvites", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "GetPendingInvites", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = pendingInvites.TotalPages,
                    perPage = pendingInvites.PageSize,
                    totalEntries = pendingInvites.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<UserInvitationDTO>>
                {
                    success = true,
                    message = "Pending invites retrieved successfully",
                    data = pendingInvitesDto,
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


        #region RevokeUserInvite


        /// <summary>
        /// Endpoint to revoke a staff invite
        /// </summary>
        /// <param name="userEmails"></param>
        /// <returns></returns>
        [HttpPost("revokeInvite")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> RevokeUserInvite([FromBody] RevokeStaffInvitationRequestDTO userEmails)
        {

            try
            {
                var userClaims = User.UserClaims();

                foreach (var item in userEmails.Emails)
                {
                    var userInvitation = await _userInvitationRepository.FirstOrDefault(x => x.Email == item);

                    if (userInvitation == null)
                    {
                        return BadRequest(new ErrorResponse<object>
                        {
                            success = false,
                            message = $"User {userInvitation.Email} has not been invited",
                            errors = new { }
                        });
                    }

                    if (!userInvitation.Status)
                    {
                        return BadRequest(new ErrorResponse<object>
                        {
                            success = false,
                            message = $"User invitation {userInvitation.Email} has been revoked already",
                            errors = new { }
                        });
                    }

                    userInvitation.Status = false;
                    _userInvitationRepository.Update(userInvitation);
                }

                //save changes to db
                await _userInvitationRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Staffs invite revoked successfully",
                    data = new { }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }


        }

        #endregion


        /// <summary>
        /// Endpoint to approve a vendor
        /// </summary>
        /// <param name="vendorProfileId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{vendorProfileId}/approveVendor")]
        [ProducesResponseType(typeof(SuccessResponse<VendorProfileDTO>), 200)]
        public async Task<IActionResult> ApproveVendorProfile(Guid vendorProfileId)
        {
            try
            {
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId && u.UserType == EUserType.STAFF);

                if (user == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User {userClaims.UserId} can't be found or is not a staff",
                        errors = new { }
                    });
                }

                var vendor = await _vendorProfileRepository.SingleOrDefault(v => v.Id == vendorProfileId);

                if (vendor == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor profile {vendor} can't be found",
                        errors = new { }
                    });
                }

                vendor.Status = EVendorStatus.APPROVED;

                var userActvity = new UserActivity
                {
                    ObjectClass = "STAFF",
                    ObjectId = vendor.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS),
                    EventType = "APPROVE A VENDOR",
                    UserId = user.Id
                };


                await _vendorProfileRepository.SaveChangesAsync();

                var vendorProfileDTO = _mapper.Map<VendorProfileDTO>(vendor);

                return Ok(new SuccessResponse<VendorProfileDTO>
                {
                    success = true,
                    message = "Vendor approved successfully",
                    data = vendorProfileDTO
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// Endpoint to approve a vendor
        /// </summary>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{vendorId}/rejectVendor")]
        [ProducesResponseType(typeof(SuccessResponse<VendorProfileDTO>), 200)]
        public async Task<IActionResult> RejectVendorProfile(Guid vendorId)
        {
            try
            {
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId && u.UserType == EUserType.STAFF);

                if (user == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User {userClaims.UserId} can't be found or is not a staff",
                        errors = new { }
                    });
                }

                var vendor = await _vendorProfileRepository.SingleOrDefault(v => v.Id == vendorId);

                if (vendor == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor profile {vendor} can't be found",
                        errors = new { }
                    });
                }

                vendor.Status = EVendorStatus.REJECTED;

                var userActvity = new UserActivity
                {
                    ObjectClass = "STAFF",
                    ObjectId = vendor.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS),
                    EventType = "REJECT A VENDOR",
                    UserId = user.Id
                };


                await _vendorProfileRepository.SaveChangesAsync();

                var vendorProfileDTO = _mapper.Map<VendorProfileDTO>(vendor);

                return Ok(new SuccessResponse<VendorProfileDTO>
                {
                    success = true,
                    message = "Vendor rejected successfully",
                    data = vendorProfileDTO
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

    }
}
