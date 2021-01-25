using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Application.Models.StaffModels;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EGPS.WebAPI.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private IHttpContextAccessor _accessor;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserInvitationRepository _userInvitationRepository;
        private readonly IPhotoAcessor _photoAcessor;
        private readonly IEmailTemplate _emailTemplate;
        private readonly IMinistryRepository _ministryRepository;
        private readonly IVendorProfileRepository _vendorProfileRepository;
        private const string USER_IP_ADDRESS = "User-IP-Address";

        public UsersController(IJwtAuthenticationManager jwtAuthenticationManager,
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IUserActivityRepository userActivityRepository,
            IRoleRepository roleRepository,
            IEmailSender emailSender,
            IConfiguration configuration,
            IHttpContextAccessor accessor,
            IMapper mapper, 
            IAccountRepository accountRepository, 
            IUserInvitationRepository userInvitationRepository,
            IPhotoAcessor photoAcessor,
            IEmailTemplate emailTemplate,
            IMinistryRepository ministryRepository,
            IVendorProfileRepository vendorProfileRepository)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager ?? throw new ArgumentNullException(nameof(jwtAuthenticationManager));
            _userRoleRepository = userRoleRepository ?? throw new ArgumentNullException(nameof(userRoleRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _userInvitationRepository = userInvitationRepository ?? throw new ArgumentNullException(nameof(userInvitationRepository));
            _photoAcessor = photoAcessor ?? throw new ArgumentNullException(nameof(photoAcessor));
            _emailTemplate = emailTemplate ?? throw new ArgumentNullException(nameof(emailTemplate));
            _ministryRepository = ministryRepository ?? throw new ArgumentNullException(nameof(ministryRepository));
            _vendorProfileRepository = vendorProfileRepository ?? throw new ArgumentNullException(nameof(vendorProfileRepository));
            Configuration = configuration;
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

        

        [AllowAnonymous]
        [HttpPost]
        [Route("confirmEmail")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmInvitationDTO model)
        {
            try
            {
                var userInvite = await _userRepository.FirstOrDefault(u => u.Email == model.Email && u.VerificationToken == model.Token);
                if (userInvite == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your invitation link is invalid",
                        errors = new { }
                    });
                }

                if (userInvite.Status == EStatus.DISABLED)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your invitation is already verified, you can login with email and password",
                        errors = new { }
                    });
                }

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Your invitation link is valid",
                    data = new { }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(UserResponse<UserDTO>), 200)]
        public async Task<IActionResult> LoginUser(UserLoginForCreationDTO userLogin)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Your login request failed",
                        errors = ModelState.Error()
                    });
                }

                //var userEntity = await _userRepository.SingleOrDefault(x => x.Email == userLogin.Email && x.EmailVerified);
                var userEntity = await _userRepository.SingleOrDefault(x => x.Email == userLogin.Email);

                if (userEntity == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Wrong email or password provided",
                        errors = new { }
                    });
                }

                var isUserValid = BCrypt.Net.BCrypt.Verify(userLogin.Password, userEntity.Password);

                if (!isUserValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Wrong email or password provided",
                        errors = new { }
                    });
                }

                userEntity.Lastlogin = DateTime.Now;
                _userRepository.Update(userEntity);
                await _userRepository.SaveChangesAsync();

                var userActivity = new UserActivity
                {
                    EventType = "User Login",
                    UserId = userEntity.Id,
                    ObjectClass = "USER",
                    ObjectId = userEntity.Id,
                    AccountId = userEntity.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                var (Token, ExpiresIn) = _jwtAuthenticationManager.Authenticate(userEntity);
                var userDto = _mapper.Map<UserDTO>(userEntity);

                //get vendor profile Id
                if (userEntity.UserType == EUserType.VENDOR)
                {
                    var vendorProfile = await _vendorProfileRepository.FirstOrDefault(m => m.UserId == userEntity.Id);
                    if (vendorProfile == null)
                    {
                        userDto.VendorProfileId = null;
                    }
                    else
                    {
                        userDto.VendorProfileId = vendorProfile.Id;
                    }
                }
                

                return Ok(new UserResponse<UserDTO>
                {
                    success = true,
                    message = "Your login request was succeed",
                    data = new UserData<UserDTO>
                    {
                        user = userDto,
                        token = Token,
                        expiresIn = ExpiresIn,
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost("forgotPassword")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> ResetPassword(PasswordResetLinkForCreationDTO passwordResetLinkFor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Your user password reset request failed",
                        errors = ModelState.Error()
                    });
                }

                var resetToken = CustomToken.GenerateToken();
                var user = await _userRepository.SingleOrDefault(x => x.Email == passwordResetLinkFor.Email);

                if (user == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Your user password reset request failed",
                        errors = new
                        {
                            email = new[] { "Email does not exist" }
                        }
                    });
                }

                var passwordResetEntity = new PasswordReset
                {
                    Email = passwordResetLinkFor.Email,
                    AccountId = user.AccountId,
                    Token = resetToken
                };

                await _userRepository.CreatePasswordReset(passwordResetEntity);


                string filePath = Path.Combine(Environment.CurrentDirectory, @"wwwroot/AppData", "Templates");
                string htmlPath = $@"{filePath}/ForgotPassword.html";
                var verificationLink = $"{Configuration["BASE_URL"]}/reset-password?token={resetToken}&email={user.Email}";
                var body = _emailTemplate.GetEmailTemplate(verificationLink, htmlPath);

                var notification = new Notification
                {
                    UserId = user.Id,
                    AccountId = user.AccountId,
                    NotificationType = ENotificationType.Email,
                    Recipient = user.Email,
                    Subject = "Password Reset",
                    Body = body,
                    TemplateId = Configuration["EMAIL_VERIFICATION_TEMPLATE_ID"]
                };

                await _emailSender.SendEmailAsync("Password Reset", body, user.Email, notification);

                var userActivity = new UserActivity
                {
                    EventType = "User Forgot Password",
                    UserId = user.Id,
                    ObjectClass = "USER",
                    ObjectId = user.Id,
                    AccountId = user.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Your user password reset request was successful",
                    data = new { }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpGet(Name = "GetUsers")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<UsersDTO>>>), 200)]
        public async Task<IActionResult> Get([FromQuery] UserPageModel model)
        {
            try
            {
                var userClaims = User.UserClaims();
                var users = await _userRepository.GetUsers(model, userClaims.AccountId);
                var prevLink = users.HasPrevious ? CreateResourceUri(model, ResourceUriType.PreviousPage) : null;
                var nextLink = users.HasNext ? CreateResourceUri(model, ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(model, ResourceUriType.CurrentPage);
                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = users.TotalPages,
                    perPage = users.PageSize,
                    totalEntries = users.TotalCount
                };
                var usersDto = _mapper.Map<IEnumerable<UsersDTO>>(users);

                return Ok(new PagedResponse<IEnumerable<UsersDTO>>
                {
                    success = true,
                    message = "Users retrieved successfully",
                    data = usersDto,
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

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<UsersDTO>), 200)]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();
                var user = await _userRepository.GetUserById(id, userClaims.AccountId);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {id} not found",
                        errors = new { }
                    });
                }

                var userDto = _mapper.Map<UsersDTO>(user);
                return Ok(new SuccessResponse<UsersDTO>
                {
                    success = true,
                    message = "User details retrieved successfully",
                    data = userDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to retrieve a specific user from the token
        /// UserType: 1 = Vendor, 2 = Staff
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("token")]
        [ProducesResponseType(typeof(SuccessResponse<UsersDTO>), 200)]
        public async Task<IActionResult> GetUserFromToken()
        {
            try
            {
                var userClaims = User.UserClaims();
                var user = await _userRepository.GetUserById(userClaims.UserId, userClaims.AccountId);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {userClaims.UserId} not found",
                        errors = new { }
                    });
                }

                if (user.UserType == EUserType.STAFF)
                {

                    var staffUserDto = _mapper.Map<StaffWithTokenDto>(user);
                    staffUserDto.Ministry = _mapper.Map<MinistryDTO>(await _ministryRepository.FirstOrDefault(m => m.Id == user.MinistryId));

                    return Ok(new SuccessResponse<StaffWithTokenDto>
                    {
                        success = true,
                        message = "User details retrieved successfully",
                        data = staffUserDto
                    });
                }

                var userDto = _mapper.Map<UsersDTO>(user);
                return Ok(new SuccessResponse<UsersDTO>
                {
                    success = true,
                    message = "User details retrieved successfully",
                    data = userDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost("resetPassword")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> UserResetPassword(UserResetPasswordForCreationDTO userResetPasswordFor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Your user password reset failed",
                        errors = ModelState.Error()
                    });
                }

                var userResetPasswordDetails = await _userRepository.GetUserResetPassword(userResetPasswordFor);

                if (userResetPasswordDetails == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Your user password reset failed",
                        errors = new
                        {
                            token = "Invalid token"
                        }
                    });
                }

                var userEntity = await _userRepository.SingleOrDefault(x => x.Email == userResetPasswordDetails.Email);

                userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userResetPasswordFor.Password);
                _userRepository.Update(userEntity);
                await _userRepository.SaveChangesAsync();

                userResetPasswordDetails.Status = false;
                _userRepository.UpdateUserResetPassword(userResetPasswordDetails);
                await _userRepository.SaveChangesAsync();

                var userActivity = new UserActivity
                {
                    EventType = "User Reset Password",
                    UserId = userEntity.Id,
                    ObjectClass = "USER",
                    ObjectId = userEntity.Id,
                    AccountId = userEntity.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Your user password reset was successful",
                    data = new { }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpPost("{id}/changePassword")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> ChangeUserPassword(Guid id, UserChangePasswordForCreationDTO userChangePasswordFor)
        {
            try
            {
                var currentUser = User.UserClaims();
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("userId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "User change password failed",
                        errors = ModelState.Error()
                    });
                }

                var userEntity = await _userRepository.SingleOrDefault(x => x.Id == id && x.AccountId == currentUser.AccountId);

                if (userEntity == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {id} not found",
                        errors = new { }
                    });
                }

                var isCurrentPasswordValid = BCrypt.Net.BCrypt.Verify(userChangePasswordFor.CurrentPassword, userEntity.Password);
                if (!isCurrentPasswordValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "User change password failed",
                        errors = new
                        {
                            currentPassword = new[] { "Current password is incorrect" }
                        }
                    });
                }

                if (userChangePasswordFor.CurrentPassword == userChangePasswordFor.NewPassword)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "User change password failed",
                        errors = new
                        {
                            newPassword = new[] { "New password can not be the same as current password" }
                        }
                    });
                }

                userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userChangePasswordFor.NewPassword);

                _userRepository.Update(userEntity);
                await _userRepository.SaveChangesAsync();

                var userActivity = new UserActivity
                {
                    EventType = "User Changed Password",
                    UserId = userEntity.Id,
                    AccountId = userEntity.AccountId,
                    ObjectClass = "USER",
                    ObjectId = userEntity.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "user change password successfully",
                    data = new { }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<UserDTO>), 200)]
        public async Task<IActionResult> UpdateUser(UserForUpdateDTO userModelForUpdate, Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("userId"); 
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "User details update failed",
                        errors = ModelState.Error()
                    });
                }
                var userClaims = User.UserClaims();
                var user = await _userRepository.SingleOrDefault(u => u.Id == id && u.AccountId == userClaims.AccountId);
                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {id} not found",
                        errors = new { }
                    });
                }

                _mapper.Map(userModelForUpdate, user);
                _userRepository.Update(user);
                var userDto = _mapper.Map<UserDTO>(user);
                await _userRepository.SaveChangesAsync();
                var userActivity = new UserActivity
                {
                    EventType = "User Details Updated",
                    UserId = user.Id,
                    ObjectClass = "USER",
                    ObjectId = user.Id,
                    AccountId = user.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();
                return Ok(new SuccessResponse<UserDTO>
                {
                    success = true,
                    message = "User details updated successfully",
                    data = userDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpPost]
        [Route("{id}/profilePicture")]
        [ProducesResponseType(typeof(SuccessResponse<UserDTO>), 200)]
        public async Task<IActionResult> UploadUserProfile(Guid id, IFormFile photo)
        {
            try
            {
                var userClaims = User.UserClaims();
                var user = await _userRepository.SingleOrDefault(u => u.Id == id && u.AccountId == userClaims.AccountId);
                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {id} not found",
                        errors = new { }
                    });
                }

                if (photo != null && photo.Length > 0)
                {
                    var photoUploadResult = _photoAcessor.AddPhoto(photo);
                    var profilePicture = JsonConvert.SerializeObject(photoUploadResult);
                    user.ProfilePicture = profilePicture;
                    user.UpdatedAt      = DateTime.Now;
                    _userRepository.Update(user);
                    await _userRepository.SaveChangesAsync();

                    var userActivity = new UserActivity
                    {
                        EventType   = "User Upload Profile Picture",
                        UserId      = userClaims.UserId,
                        ObjectClass = "USER",
                        ObjectId    = user.AccountId,
                        AccountId   = userClaims.AccountId,
                        IpAddress   = Request.GetHeader(USER_IP_ADDRESS)
                    };
                    await _userActivityRepository.AddAsync(userActivity);
                    await _userActivityRepository.SaveChangesAsync();

                    var userDto = _mapper.Map<UserDTO>(user);
                    return Ok(new SuccessResponse<UserDTO>
                    {
                        success = true,
                        message = "User profile picture uploaded successfully",
                        data    = userDto
                    });
                }
                
                return BadRequest(new ErrorResponse<object>
                {
                    success = false,
                    message = "Profile picture cannot be null",
                    errors  = new { }
                });
                
               
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpGet]
        [Route("checkers/{ministryId}")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<UserDTO>>), 200)]
        public async Task<IActionResult> GetCheckers(Guid ministryId)
        {
            try
            {
                var checkers = await _userRepository.GetCheckers(ministryId);
                var userDto = _mapper.Map<IEnumerable<UserDTO>>(checkers);

                return Ok(new SuccessResponse<IEnumerable<UserDTO>>
                {
                    success = true,
                    message = "List of checkers retrieved successfully",
                    data = userDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpPatch]
        [Route("{id}/setThreshold")]
        [ProducesResponseType(typeof(SuccessResponse<UserDTO>), 200)]
        public async Task<IActionResult> SetThreshold(UserForSetThreshold userForSetThreshold, Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("userId"); 
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "User details update failed",
                        errors = ModelState.Error()
                    });
                }
                var userClaims = User.UserClaims();
                var user = await _userRepository.SingleOrDefault(u => u.Id == id && u.AccountId == userClaims.AccountId && u.MinistryId == userForSetThreshold.MinistryId);
                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {id} not found",
                        errors = new { }
                    });
                }

                if ((user.Role == ERole.PERMANENTSECRETARY || user.Role == ERole.EXECUTIVE || user.Role == ERole.COMMISSIONER) == false)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with Id {id} and Role {user.Role} cannot be assigned a threshold",
                        errors = new { }
                    });
                }

                user.Threshold = userForSetThreshold.Threshold;
                _userRepository.Update(user);
                var userDto = _mapper.Map<UserDTO>(user);
                await _userRepository.SaveChangesAsync();

                var userActivity = new UserActivity
                {
                    EventType = "Set User Threshold",
                    UserId = user.Id,
                    ObjectClass = "USER",
                    ObjectId = user.Id,
                    AccountId = user.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();
                return Ok(new SuccessResponse<UserDTO>
                {
                    success = true,
                    message = "User threshold set successfully",
                    data = userDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        #region CreateResource
        private string CreateResourceUri(UserPageModel parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetUsers",
                        new
                        {
                            PageNumber = parameters.PageNumber - 1,
                            PageSize = parameters.PageSize
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetUsers",
                        new
                        {
                            PageNumber = parameters.PageNumber + 1,
                            PageSize = parameters.PageSize
                        });

                default:
                    return Url.Link("GetUsers",
                        new
                        {
                            PageNumber = parameters.PageNumber,
                            PageSize = parameters.PageSize
                        });
            }

        }
        #endregion
    }
}