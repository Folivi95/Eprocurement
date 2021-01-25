using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
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
using System.Threading.Tasks;
using UserDTO = EGPS.Application.Models.UserDTO;

namespace EGPS.WebAPI.Controllers
{
    /// <summary>
    /// Accounts controller
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IEmailSender _emailSender;
        private IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IPhotoAcessor _photoAcessor;
        private readonly IEmailTemplate _emailTemplate;
        private const string USER_IP_ADDRESS = "User-IP-Address";

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="accountRepository"></param>
        /// <param name="userActivityRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="emailSender"></param>
        /// <param name="accessor"></param>
        /// <param name="mapper"></param>
        /// <param name="roleRepository"></param>
        /// <param name="configuration"></param>
        /// <param name="jwtAuthenticationManager"></param>
        /// <param name="photoAcessor"></param>
        /// <param name="emailTemplate"></param>
        public AccountsController(
            IAccountRepository accountRepository,
            IRoleRepository roleRepository,
            IUserActivityRepository userActivityRepository,
            IUserRepository userRepository,
            IEmailSender emailSender,
            IHttpContextAccessor accessor,
            IConfiguration configuration,
            IMapper mapper,
            IJwtAuthenticationManager jwtAuthenticationManager,
            IPhotoAcessor photoAcessor,
            IEmailTemplate emailTemplate)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _jwtAuthenticationManager = jwtAuthenticationManager ?? throw new ArgumentNullException(nameof(jwtAuthenticationManager));
            _photoAcessor = photoAcessor ?? throw new ArgumentNullException(nameof(photoAcessor));
            _emailTemplate = emailTemplate ?? throw new ArgumentNullException(nameof(emailTemplate));
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("signup")]
        [ProducesResponseType(typeof(SuccessResponse<AccountDTO>), 200)]
        public async Task<IActionResult> CreateAccount(AccountForCreationDTO accountForCreation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your account signup request failed",
                        errors = ModelState.Error()
                    });
                }

                var accountCompanyNameExist = await _accountRepository.ExistsAsync(
                    x => x.CompanyName == accountForCreation.CompanyName);

                var accountEmailExist = await _accountRepository.ExistsAsync(
                    x => x.ContactEmail == accountForCreation.ContactEmail);

                if (accountCompanyNameExist)
                {
                    return Conflict(new
                    {
                        success = false,
                        message = "Your account signup request failed",
                        errors = new
                        {
                            companyName = new string[] { "Company name already exists" }
                        }
                    });
                }

                if (accountEmailExist)
                {
                    return Conflict(new
                    {
                        success = false,
                        message = "Your account signup request failed",
                        errors = new
                        {
                            email = new string[] { "Email already exists" }
                        }
                    });
                }

                var account = _mapper.Map<Account>(accountForCreation);

                await _accountRepository.AddAsync(account);
                await _accountRepository.SaveChangesAsync();

                var user = new User
                {
                    FirstName = accountForCreation.FirstName,
                    LastName = accountForCreation.LastName,
                    Password = BCrypt.Net.BCrypt.HashPassword(accountForCreation.Password),
                    Email = accountForCreation.ContactEmail,
                    VerificationToken = CustomToken.GenerateToken(),
                    AccountId = account.Id,
                    Status = EStatus.DISABLED
                };

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();


                account.CreatedById = user.Id;
                account.UpdatedAt = DateTime.Now;
                _accountRepository.Update(account);
                await _accountRepository.SaveChangesAsync();

                string filePath = Path.Combine(Environment.CurrentDirectory, @"wwwroot/AppData", "Templates");
                string htmlPath = $@"{filePath}/ConfirmEmail.html";
                var verificationLink = $"{Configuration["BASE_URL"]}/login?token={user.VerificationToken}&email={user.Email}";
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
                    EventType = "Account Signup",
                    UserId = user.Id,
                    ObjectClass = "ACCOUNT",
                    ObjectId = account.Id,
                    AccountId = account.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                var systemRole = new Role[]
                {
                    new Role { Type = "SYSTEM", Title = "Admin", AccountId = account.Id, CreatedById = user.Id },
                    new Role { Type = "SYSTEM", Title = "User", AccountId = account.Id, CreatedById = user.Id },
                };

                await _roleRepository.AddRangeAsync(systemRole);
                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<AccountDTO>
                {
                    success = true,
                    message = "Organization account created successfully",
                    data = _mapper.Map<AccountDTO>(account)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("resendInvitation")]
        [ProducesResponseType(typeof(SuccessResponse<ResendAccountDTO>), 200)]
        public async Task<IActionResult> ResendInvitation(ResendAccountMailDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your account signup email resend request failed",
                        errors = ModelState.Error()
                    });
                }
                var user = await _userRepository.SingleOrDefault(u => u.Email == model.Email);
                var error = new Dictionary<string, string[]>();
                if (user == null)
                {
                    error.Add("email",new []{"Email does not exist" });
                    return NotFound(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your account signup email resend request failed",
                        errors = error
                });
                }

                if (user.EmailVerified)
                {
                    error.Add("email", new[] {"Email is already verified" });
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your account signup email resend request failed",
                        errors = error
                    });
                }

                var account = await _accountRepository.SingleOrDefault(a => a.CreatedById == user.Id);
                if (account == null)
                {
                    error.Add("email", new[] {"Account does not exist"});
                    return NotFound(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your account signup email resend request failed",
                        errors  = error
                    });
                }

                string filePath = Path.Combine(Environment.CurrentDirectory, @"wwwroot/AppData", "Templates");
                string htmlPath = $@"{filePath}/ConfirmEmail.html";
                var verificationLink = $"{Configuration["BASE_URL"]}/login?token={user.VerificationToken}&email={user.Email}";
                var body = _emailTemplate.GetEmailTemplate(verificationLink, htmlPath);

                var notification = new Notification
                {
                    UserId = user.Id,
                    AccountId = user.AccountId,
                    NotificationType = ENotificationType.Email,
                    Recipient = user.Email,
                    Subject = "Email Confirmation",
                    Body = body,
                    TemplateId = Configuration["EMAIL_VERIFICATION_TEMPLATE_ID"]
                };

                await _emailSender.SendEmailAsync("Password Reset", body, user.Email, notification);

                return Ok(new SuccessResponse<ResendAccountDTO>
                {
                    success = true,
                    message = "Account signup email sent successfully",
                    data = _mapper.Map<ResendAccountDTO>(account)
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("confirmEmail")]
        [ProducesResponseType(typeof(UserResponse<UserDTO>), 200)]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailTokenDTO model)
        {
            try
            {
                var user = await _userRepository.SingleOrDefault(u => u.Email == model.Email && u.VerificationToken == model.Token);
                if (user == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your email verification link is invalid",
                        errors = new {}
                    });
                }

                if (user.EmailVerified)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your email verification is already done. Login with your email and password",
                        errors  = new { }
                    });
                }

                user.EmailVerified = true;
                user.Status = EStatus.ENABLED;
                user.UpdatedAt = DateTime.Now;
                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();
                var jwt = _jwtAuthenticationManager.Authenticate(user);
                var userDto = _mapper.Map<UserDTO>(user);
                return Ok(new UserResponse<UserDTO>
                {
                    success = true,
                    message = "Your email verification link is valid",
                    data    = new UserData<UserDTO>
                    {
                        user = userDto,
                        token     = jwt.Token,
                        expiresIn = jwt.ExpiresIn
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        
        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<AccountDTO>), 200)]
        public async Task<IActionResult> UpdateAccount(AccountForUpdateDTO accountForUpdate, Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("accountId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Account details update failed",
                        errors  = ModelState.Error()
                    });
                }
                var account = await _accountRepository.GetByIdAsync(id);
                
                if (account == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Account with id {id} not found",
                        errors  = new { }
                    });
                }

                _mapper.Map(accountForUpdate, account);
                _accountRepository.Update(account);
                var accountDto = _mapper.Map<AccountDTO>(account);
                await _userRepository.SaveChangesAsync();
                return Ok(new SuccessResponse<AccountDTO>
                {
                    success = true,
                    message = "Organization account details updated successfully",
                    data    = accountDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<AccountDTO>), 200)]
        public async Task<IActionResult> GetAccount(Guid id)
        {
            try
            {
                var accountEntity = await _accountRepository.GetByIdAsync(id);

                if (accountEntity == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Account with id {id} not found",
                        errors = new { }
                    });
                }

                var accountDTO = _mapper.Map<AccountDTO>(accountEntity);
                return Ok(new SuccessResponse<AccountDTO>
                {
                    success = true,
                    message = "Account details retrieved successfully",
                    data = accountDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpPost]
        [Route("{id}/companyLogo")]
        [ProducesResponseType(typeof(SuccessResponse<AccountDTO>), 200)]
        public async Task<IActionResult> UploadCompanyLogo(Guid id, IFormFile photo)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(id);

                if (account == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Account with id {id} not found",
                        errors = new { }
                    });
                }

                if (photo != null && photo.Length > 0)
                {
                    var photoUploadResult = _photoAcessor.AddPhoto(photo);

                    account.CompanyLogo = JsonConvert.SerializeObject(photoUploadResult);
                    account.UpdatedAt = DateTime.Now;
                    _accountRepository.Update(account);
                    await _accountRepository.SaveChangesAsync();

                    var userClaims = User.UserClaims();
                    var userActivity = new UserActivity
                    {
                        EventType = "Account Upload Company Logo",
                        UserId = userClaims.UserId,
                        ObjectClass = "ACCOUNT",
                        ObjectId = account.Id,
                        AccountId = userClaims.AccountId,
                        IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                    };

                    await _userActivityRepository.AddAsync(userActivity);
                    await _userActivityRepository.SaveChangesAsync();

                    var accountDto = _mapper.Map<AccountDTO>(account);

                    return Ok(new SuccessResponse<AccountDTO>
                    {
                        success = true,
                        message = "Account company logo uploaded successfully",
                        data = accountDto
                    });
                }
                else
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "company logo cannot be null",
                        errors = new { }
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

    }
}
