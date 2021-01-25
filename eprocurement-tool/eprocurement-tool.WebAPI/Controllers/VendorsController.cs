using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EGPS.WebAPI.Controllers
{
    /// <summary>
    /// Vendors controller
    /// </summary>
    [Route("api/v1/vendors")]
    [ApiController]
    [Authorize]
    public class VendorsController : ControllerBase
    {
        private readonly IVendorProfileRepository _vendorProfileRepository;
        private readonly IVendorCorrespondenceRepository _vendorCorrespondenceRepository;
        private readonly IVendorAttestationRepository _vendorAttestationRepository;
        private readonly IBusinessServiceRepository _businessServiceRepository;
        private readonly IVendorContactRepository _vendorContactRepository;
        private readonly IVendorServiceRepository _vendorServiceRepository;
        private readonly IRegistrationPlanRepository _registrationPlanRepository;
        private readonly IVendorRegistrationCategoryRepository _vendorRegistrationCategoryRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IProcurementPlanRepository _procurementPlanRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IPhotoAcessor _photoAcessor;
        private readonly IVendorDirectorRepository _vendorDirectorRepository;
        private readonly IVendorDirectorCertificateRepository _vendorDirectorCertificateRepository;
        private readonly IVendorDocumentRepository _vendorDocumentRepository;
        private readonly IVendorDocumentTypeRepository _vendorDocumentTypeRepository;
        private readonly IBusinessCategoryRepository _businessCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IProjectRepository _projectRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IProcurementPlanActivityRepository _procurementPlanActivityRepository;
        private readonly IEmailTemplate _emailTemplate;
        private readonly IProcurementService _procurementService;
        private const string USER_IP_ADDRESS = "User-IP-Address";

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="vendorProfileRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="userRepository"></param>
        /// <param name="accountRepository"></param>
        /// <param name="emailSender"></param>
        /// <param name="configuration"></param>
        /// <param name="userActivityRepository"></param>
        /// <param name="vendorCorrespondenceRepository"></param>
        /// <param name="vendorContactRepository"></param>
        /// <param name="vendorAttestationRepository"></param>
        /// <param name="stateRepository"></param>
        /// <param name="countryRepository"></param>
        /// <param name="vendorDirectorRepository"></param>
        /// <param name="vendorDirectorCertificateRepository"></param>
        /// <param name="photoAcessor"></param>
        /// <param name="vendorDocumentRepository"></param>
        /// <param name="vendorDocumentTypeRepository"></param>
        /// <param name="businessServiceRepository"></param>
        /// <param name="businessCategoryRepository"></param>
        /// <param name="vendorServiceRepository"></param>
        /// <param name="registrationPlanRepository"></param>
        /// <param name="vendorRegistrationCategoryRepository"></param>
        /// <param name="contractRepository"></param>
        /// <param name="projectRepository"></param>
        /// <param name="procurementPlanRepository"></param>
        /// <param name="notificationRepository"></param>
        /// <param name="procurementPlanActivityRepository"></param>
        /// <param name="emailTemplate"></param>
        /// <param name="procurementService"></param>

        public VendorsController(
            IVendorProfileRepository vendorProfileRepository,
            IMapper mapper,
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            IEmailSender emailSender,
            IConfiguration configuration,
            IUserActivityRepository userActivityRepository,
            IVendorCorrespondenceRepository vendorCorrespondenceRepository,
            IVendorContactRepository vendorContactRepository,
            IVendorAttestationRepository vendorAttestationRepository,
            IStateRepository stateRepository,
            ICountryRepository countryRepository,
            IPhotoAcessor photoAcessor,
            IVendorDirectorRepository vendorDirectorRepository,
            IVendorDirectorCertificateRepository vendorDirectorCertificateRepository,
            IVendorDocumentRepository vendorDocumentRepository,
            IVendorDocumentTypeRepository vendorDocumentTypeRepository,
            IBusinessServiceRepository businessServiceRepository,
            IBusinessCategoryRepository businessCategoryRepository,
            IVendorServiceRepository vendorServiceRepository,
            IRegistrationPlanRepository registrationPlanRepository,
            IVendorRegistrationCategoryRepository vendorRegistrationCategoryRepository,
            IContractRepository contractRepository,
            IProjectRepository projectRepository,
            IProcurementPlanRepository procurementPlanRepository,
            INotificationRepository notificationRepository,
            IProcurementPlanActivityRepository procurementPlanActivityRepository,
            IEmailTemplate emailTemplate, IProcurementService procurementService)
        {
            _vendorProfileRepository = vendorProfileRepository ?? throw new ArgumentNullException(nameof(vendorProfileRepository));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            Configuration = configuration;
            _vendorCorrespondenceRepository = vendorCorrespondenceRepository ?? throw new ArgumentNullException(nameof(vendorCorrespondenceRepository));
            _vendorContactRepository = vendorContactRepository ?? throw new ArgumentNullException(nameof(vendorContactRepository));
            _vendorAttestationRepository = vendorAttestationRepository ?? throw new ArgumentNullException(nameof(vendorAttestationRepository));
            _stateRepository = stateRepository ?? throw new ArgumentNullException(nameof(stateRepository));
            _countryRepository = countryRepository ?? throw new ArgumentNullException(nameof(countryRepository));
            _photoAcessor = photoAcessor ?? throw new ArgumentNullException(nameof(photoAcessor));
            _vendorDirectorRepository = vendorDirectorRepository ?? throw new ArgumentNullException(nameof(vendorDirectorRepository));
            _vendorDirectorCertificateRepository = vendorDirectorCertificateRepository ?? throw new ArgumentNullException(nameof(vendorDirectorCertificateRepository));
            _vendorDocumentRepository = vendorDocumentRepository ?? throw new ArgumentNullException(nameof(vendorDocumentRepository));
            _vendorDocumentTypeRepository = vendorDocumentTypeRepository ?? throw new ArgumentNullException(nameof(vendorDocumentTypeRepository));
            _businessServiceRepository = businessServiceRepository ?? throw new ArgumentNullException(nameof(businessServiceRepository));
            _businessCategoryRepository = businessCategoryRepository ?? throw new ArgumentNullException(nameof(businessCategoryRepository));
            _vendorServiceRepository = vendorServiceRepository ?? throw new ArgumentNullException(nameof(vendorServiceRepository));
            _registrationPlanRepository = registrationPlanRepository ?? throw new ArgumentNullException(nameof(registrationPlanRepository));
            _vendorRegistrationCategoryRepository = vendorRegistrationCategoryRepository ?? throw new ArgumentNullException(nameof(vendorRegistrationCategoryRepository));
            _contractRepository = contractRepository ?? throw new ArgumentNullException(nameof(contractRepository));
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _procurementPlanRepository = procurementPlanRepository ?? throw new ArgumentNullException(nameof(procurementPlanRepository));
            _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _procurementPlanActivityRepository = procurementPlanActivityRepository ?? throw new ArgumentNullException(nameof(procurementPlanActivityRepository));
            _emailTemplate = emailTemplate ?? throw new ArgumentNullException(nameof(emailTemplate));
            _procurementService = procurementService ?? throw new ArgumentNullException(nameof(procurementService));
        }

        public IConfiguration Configuration { get; }



        /// <summary>
        /// Endpoint to register a Vendor
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("signup")]
        [ProducesResponseType(typeof(SuccessResponse<UserDTO>), 200)]
        public async Task<IActionResult> CreateVendorAccount(UserVendorForCreationDTO uservendorForCreation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your vendor signup request failed",
                        errors = ModelState.Error()
                    });
                }

                var vendorEmailExist = await _userRepository.ExistsAsync(x => x.Email == uservendorForCreation.Email);

                if (vendorEmailExist)
                {
                    return Conflict(new
                    {
                        success = false,
                        message = "Your vendor signup request failed",
                        errors = new
                        {
                            email = new string[] { "Email already exists" }
                        }
                    });
                }

                var account = await _accountRepository.FirstOrDefault(x => x.CompanyName == "Sample State Government");

                if (account == null)
                {
                    return Conflict(new
                    {
                        success = false,
                        message = "Your vendor signup request failed",
                        errors = new
                        {
                            account = new string[] { "Account does not exist" }
                        }
                    });
                }

                var user = new User
                {
                    FirstName = uservendorForCreation.FirstName,
                    LastName = uservendorForCreation.LastName,
                    Password = BCrypt.Net.BCrypt.HashPassword(uservendorForCreation.Password),
                    Email = uservendorForCreation.Email,
                    VerificationToken = CustomToken.GenerateToken(),
                    Status = EStatus.ENABLED,
                    UserType = EUserType.VENDOR,
                    EmailVerified = false,
                    AccountId = account.Id,
                    Role = ERole.Vendor
                };

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();



                var userActivity = new UserActivity
                {
                    EventType = "Vendor Signup",
                    UserId = user.Id,
                    ObjectClass = "VENDOR",
                    ObjectId = user.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();


                string filePath = Path.Combine(Environment.CurrentDirectory, @"wwwroot/AppData", "Templates");
                string htmlPath = $@"{filePath}/WelcomeVendor.html";
                var verificationLink = $"{Configuration["BASE_URL"]}/vendors/confirm-email?token={user.VerificationToken}&email={user.Email}";
                var body = _emailTemplate.GetEmailTemplate(verificationLink, htmlPath);

                var notification = new Notification
                {
                    UserId = user.Id,
                    AccountId = user.AccountId,
                    NotificationType = ENotificationType.Email,
                    Recipient = user.Email,
                    Subject = "Welcome to EGPS Corporation",
                    Body = body,
                    TemplateId = Configuration["EMAIL_VERIFICATION_TEMPLATE_ID"]
                };

                await _emailSender.SendEmailAsync("Welcome", body, user.Email, notification);

                var createdVendorUser = _mapper.Map<UserDTO>(user);

                return Ok(new SuccessResponse<UserDTO>
                {
                    success = true,
                    message = "Vendor account created successfully",
                    data = createdVendorUser
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// Endpoint to create a Vendor Profile
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{userId}/CreateProfile")]
        [ProducesResponseType(typeof(SuccessResponse<VendorProfileDTO>), 200)]
        public async Task<IActionResult> CreateVendorProfile(Guid userId, [FromBody] VendorProfileForCreationDTO uservendorForCreation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your vendor signup request failed",
                        errors = ModelState.Error()
                    });
                }

                // check if userid is equal to null or userid is empty
                if (userId == null || userId.Equals(""))
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {userId} not found",
                        errors = new { }
                    });
                }

                var userClaims = User.UserClaims();

                // check if user exists and return error message is user does not exist
                var userExists = await _userRepository.FirstOrDefault(x => x.Id == userId);

                if (userExists == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {userId} not found",
                        errors = new { }
                    });
                }

                var stateExists = await _stateRepository.ExistsAsync(s => s.Name == uservendorForCreation.State);
                var correspondenceStateExists = await _stateRepository.ExistsAsync(s => s.Name == uservendorForCreation.CorrespondenceState);

                if (!stateExists && !correspondenceStateExists)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"State does not Exist",
                        errors = new { }
                    });
                }

                var countryExists = await _countryRepository.ExistsAsync(c => c.Name == uservendorForCreation.Country);
                var correspondenceCountryExists = await _countryRepository.ExistsAsync(c => c.Name == uservendorForCreation.CorrespondenceCountry);

                if (!countryExists && !correspondenceCountryExists)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Country does not Exist",
                        errors = new { }
                    });
                }

                VendorProfileDTO vendorProfileDTO = null;

                //check if user already has a Vendor Profile
                var vendorExist = await _vendorProfileRepository.FirstOrDefault(x => x.UserId == userExists.Id);
                if (vendorExist != null)
                {
                    //Update Vendor Profile
                    vendorExist.IncorporationDate = uservendorForCreation.IncorporationDate;
                    vendorExist.CoreCompetency = uservendorForCreation.CoreCompetency;
                    vendorExist.OrganizationType = uservendorForCreation.OrganizationType;
                    vendorExist.CompanyName = uservendorForCreation.CompanyName;
                    vendorExist.IncorporationDate = uservendorForCreation.IncorporationDate;
                    vendorExist.CACRegistrationNumber = uservendorForCreation.CACRegistrationNumber;
                    vendorExist.AddressLine1 = uservendorForCreation.AddressLine1;
                    vendorExist.AddressLine2 = uservendorForCreation.AddressLine2;
                    vendorExist.City = uservendorForCreation.City;
                    vendorExist.Status = EVendorStatus.PENDING;
                    vendorExist.AuthorizedShareCapital = uservendorForCreation.AuthorizedShareCapital;
                    vendorExist.Website = uservendorForCreation.Website;
                    vendorExist.State = uservendorForCreation.State;
                    vendorExist.Country = uservendorForCreation.Country;

                    _vendorProfileRepository.Update(vendorExist);
                    await _vendorProfileRepository.SaveChangesAsync();


                    var vendorContact = await _vendorContactRepository.FirstOrDefault(x => x.UserId == userExists.Id);
                    vendorContact.FirstName = uservendorForCreation.ContactFirstName;
                    vendorContact.LastName = uservendorForCreation.ContactLastName;
                    vendorContact.Email = uservendorForCreation.ContactEmail;
                    vendorContact.Position = uservendorForCreation.ContactPosition;
                    vendorContact.PhoneNumber = uservendorForCreation.ContactPhoneNumber;

                    _vendorContactRepository.Update(vendorContact);
                    await _vendorContactRepository.SaveChangesAsync();

                    var vendorCorrespondence = await _vendorCorrespondenceRepository.FirstOrDefault(x => x.UserID == userExists.Id);
                    vendorCorrespondence.UserID = userExists.Id;
                    vendorCorrespondence.Address1 = uservendorForCreation.CorrespondenceAddress1;
                    vendorCorrespondence.Address2 = uservendorForCreation.CorrespondenceAddress2;
                    vendorCorrespondence.State = uservendorForCreation.CorrespondenceState;
                    vendorCorrespondence.City = uservendorForCreation.CorrespondenceCity;
                    vendorCorrespondence.Country = uservendorForCreation.CorrespondenceCountry;

                    _vendorCorrespondenceRepository.Update(vendorCorrespondence);
                    await _vendorCorrespondenceRepository.SaveChangesAsync();


                    var vendorContactDTO = _mapper.Map<VendorContactDTO>(vendorContact);
                    var vendorCorrespondenceDTO = _mapper.Map<VendorCorrespondenceDTO>(vendorCorrespondence);

                    vendorProfileDTO = _mapper.Map<VendorProfileDTO>(vendorExist);
                    vendorProfileDTO.VendorContact = vendorContactDTO;
                    vendorProfileDTO.VendorCorrespondence = vendorCorrespondenceDTO;
                }

                else
                {
                    var vendorProfile = new VendorProfile
                    {
                        UserId = userExists.Id,
                        CoreCompetency = uservendorForCreation.CoreCompetency,
                        OrganizationType = uservendorForCreation.OrganizationType,
                        CompanyName = uservendorForCreation.CompanyName,
                        IncorporationDate = uservendorForCreation.IncorporationDate,
                        CACRegistrationNumber = uservendorForCreation.CACRegistrationNumber,
                        AddressLine1 = uservendorForCreation.AddressLine1,
                        AddressLine2 = uservendorForCreation.AddressLine2,
                        City = uservendorForCreation.City,
                        Status = EVendorStatus.PENDING,
                        AuthorizedShareCapital = uservendorForCreation.AuthorizedShareCapital,
                        State = uservendorForCreation.State,
                        Country = uservendorForCreation.Country,
                        Website = uservendorForCreation.Website
                    };


                    await _vendorProfileRepository.AddAsync(vendorProfile);
                    await _vendorProfileRepository.SaveChangesAsync();


                    var vendorCorrespondence = new VendorCorrespondence
                    {
                        UserID = userExists.Id,
                        Address1 = uservendorForCreation.CorrespondenceAddress1,
                        Address2 = uservendorForCreation.CorrespondenceAddress2,
                        State = uservendorForCreation.CorrespondenceState,
                        City = uservendorForCreation.CorrespondenceCity,
                        Country = uservendorForCreation.CorrespondenceCountry
                    };

                    await _vendorCorrespondenceRepository.AddAsync(vendorCorrespondence);
                    await _vendorCorrespondenceRepository.SaveChangesAsync();


                    var vendorContact = new VendorContact
                    {
                        UserId = userExists.Id,
                        FirstName = uservendorForCreation.ContactFirstName,
                        LastName = uservendorForCreation.ContactLastName,
                        Email = uservendorForCreation.ContactEmail,
                        Position = uservendorForCreation.ContactPosition,
                        PhoneNumber = uservendorForCreation.ContactPhoneNumber
                    };

                    await _vendorContactRepository.AddAsync(vendorContact);
                    await _vendorContactRepository.SaveChangesAsync();


                    var vendorContactDTO = _mapper.Map<VendorContactDTO>(vendorContact);
                    var vendorCorrespondenceDTO = _mapper.Map<VendorCorrespondenceDTO>(vendorCorrespondence);

                    vendorProfileDTO = _mapper.Map<VendorProfileDTO>(vendorProfile);
                    vendorProfileDTO.VendorContact = vendorContactDTO;
                    vendorProfileDTO.VendorCorrespondence = vendorCorrespondenceDTO;
                }



                var userActivity = new UserActivity
                {
                    EventType = "Vendor Create Profile",
                    UserId = userExists.Id,
                    ObjectClass = "VENDOR",
                    ObjectId = userExists.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();


                return Ok(new SuccessResponse<VendorProfileDTO>
                {
                    success = true,
                    message = "Vendor Profile created successfully",
                    data = vendorProfileDTO
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to get a Vendor's details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<VendorProfileUserDTO>), 200)]
        public async Task<IActionResult> GetVendorProfile(Guid id)
        {
            try
            {
                // check if userid is equal to null or userid is empty
                if (id == null || id.Equals(""))
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                //Check for accountId in token
                var UserClaims = User.UserClaims();

                // check if user exists and return error message is user does not exist
                var userExists = await _userRepository.FirstOrDefault(x => x.Id == id && x.UserType == EUserType.VENDOR);

                if (userExists == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                var vendorProfile = await _vendorProfileRepository.GetVendorProfile(userExists.Id);

                if (vendorProfile == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"VendorProfile with id {userExists.Id} not found",
                        errors = new { }
                    });
                }

                var vendorCorrespondence = await _vendorCorrespondenceRepository.FirstOrDefault(vc => vc.UserID == userExists.Id);
                var vendorContact = await _vendorContactRepository.FirstOrDefault(vc => vc.UserId == userExists.Id);



                var vendorProfileDTO = _mapper.Map<VendorProfileDTO>(vendorProfile);

                var vendorCorrespondenceDTO = _mapper.Map<VendorCorrespondenceDTO>(vendorCorrespondence);
                var vendorContactDTO = _mapper.Map<VendorContactDTO>(vendorContact);

                vendorProfileDTO.VendorCorrespondence = vendorCorrespondenceDTO;
                vendorProfileDTO.VendorContact = vendorContactDTO;

                return Ok(new SuccessResponse<VendorProfileDTO>
                {
                    success = true,
                    message = "Vendor details retrieved successfully",
                    data = vendorProfileDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }



        /// <summary>
        /// Endpoint to get All business services in the organization
        /// </summary>
        /// <returns></returns>
        [HttpGet("services", Name = "GetAllBusinessServices")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<BusinessServiceDTO>>), 200)]
        public async Task<IActionResult> GetAllBusinessServices([FromQuery] BusinessServicesParameter parameters)
        {
            try
            {

                // get the business services for the userId supplied
                var businessServices = await _businessServiceRepository.GetAllBusinessServices(parameters);

                // convert business service to business service dto
                var businessServicesDto = _mapper.Map<IEnumerable<BusinessServiceDTO>>(businessServices);

                // pagination variables
                var prevLink = businessServices.HasPrevious
                    ? CreateResourceUri(parameters, "GetAllBusinessServices", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = businessServices.HasNext ? CreateResourceUri(parameters, "GetAllBusinessServices", ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(parameters, "GetAllBusinessServices", ResourceUriType.CurrentPage);

                // create pagination items
                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = businessServices.TotalPages,
                    perPage = businessServices.PageSize,
                    totalEntries = businessServices.TotalCount
                };

                // return success response
                return Ok(new PagedResponse<IEnumerable<BusinessServiceDTO>>
                {
                    success = true,
                    message = "Bussiness services retrieved successfully",
                    data = businessServicesDto,
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
        /// Endpoint to to save/persist a vendor's list of business services
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}/services", Name = "SaveBusinessServicesForVendor")]
        [ProducesResponseType(typeof(SuccessResponse<List<BusinessServiceDTO>>), 200)]
        public async Task<IActionResult> SaveBusinessServicesForVendor(Guid id, [FromBody] BusinessServiceForCreationDTO businessServiceForCreationDto)
        {
            try
            {
                // check if id is equal to null or id is empty
                if (id == null || id.Equals(""))
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                var userClaims = User.UserClaims();

                // check if user exists and return error message is user does not exist
                var userExists = await _userRepository.FirstOrDefault(x => x.Id == id);

                if (userExists == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                List<BusinessServiceDTO> businessServiceDTO = new List<BusinessServiceDTO>();

                foreach (var businessServiceId in businessServiceForCreationDto.Add)
                {
                    var businessServiceIdExist = await _businessServiceRepository.ExistsAsync(x => x.Id == businessServiceId);
                    if (!businessServiceIdExist)
                    {
                        return BadRequest(new ErrorResponse<object>
                        {
                            success = false,
                            message = $"Business Service with id {businessServiceId} does not exist",
                            errors = new { }
                        });
                    }

                    if (businessServiceId != null)
                    {
                        var addVendorServices = new VendorService()
                        {
                            BusinessServiceID = businessServiceId,
                            UserID = userExists.Id
                        };

                        await _vendorServiceRepository.AddAsync(addVendorServices);
                        await _vendorServiceRepository.SaveChangesAsync();

                        var businessService = await _businessServiceRepository.FirstOrDefault(x => x.Id == businessServiceId);
                        var businessServiceDto = _mapper.Map<BusinessServiceDTO>(businessService);
                        businessServiceDTO.Add(businessServiceDto);
                    }
                }


                foreach (var businessServiceId in businessServiceForCreationDto.Remove)
                {
                    var businessServiceIdExist = await _businessServiceRepository.ExistsAsync(x => x.Id == businessServiceId);
                    if (!businessServiceIdExist)
                    {
                        return BadRequest(new ErrorResponse<object>
                        {
                            success = false,
                            message = $"Business Service with id {businessServiceId} does not exist",
                            errors = new { }
                        });
                    }

                    if (businessServiceId != null)
                    {
                        var removeVendorServices = await _vendorServiceRepository.FirstOrDefault(x => x.BusinessServiceID == businessServiceId);

                        _vendorServiceRepository.Remove(removeVendorServices);
                        await _vendorServiceRepository.SaveChangesAsync();
                    }
                }


                var userActivity = new UserActivity
                {
                    EventType = "Vendor Services Update - Registration Section 3",
                    UserId = userExists.Id,
                    ObjectClass = "VENDOR",
                    ObjectId = userExists.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();


                return Ok(new SuccessResponse<List<BusinessServiceDTO>>
                {
                    success = true,
                    message = "Vendor services updated successfully",
                    data = businessServiceDTO
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }




        /// <summary>
        /// Endpoint to get business services for vendor
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/services", Name = "GetBusinessServicesForVendor")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<BusinessServiceDTO>>), 200)]
        public async Task<IActionResult> GetBusinessServicesForVendor(Guid id, [FromQuery] BusinessServicesParameter parameters)
        {
            try
            {
                // check if id is equal to null or id is empty
                if (id == null || id.Equals(""))
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                // check if vendor exists and return error message is user does not exist
                var vendorExists = await _vendorProfileRepository.ExistsAsync(x => x.UserId == id);

                if (!vendorExists)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                // get the business services for the userId supplied
                var businessServices = await _vendorProfileRepository.GetBusinessServicesForVendor(parameters, id);

                // convert business service to business service dto
                var businessServicesDto = _mapper.Map<IEnumerable<BusinessServiceDTO>>(businessServices);

                // pagination variables
                var prevLink = businessServices.HasPrevious
                    ? CreateResourceUri(parameters, "GetBusinessServicesForVendor", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = businessServices.HasNext ? CreateResourceUri(parameters, "GetBusinessServicesForVendor", ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(parameters, "GetBusinessServicesForVendor", ResourceUriType.CurrentPage);

                // create pagination items
                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = businessServices.TotalPages,
                    perPage = businessServices.PageSize,
                    totalEntries = businessServices.TotalCount
                };

                // return success response
                return Ok(new PagedResponse<IEnumerable<BusinessServiceDTO>>
                {
                    success = true,
                    message = "Vendor services retrieved successfully",
                    data = businessServicesDto,
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
        /// Endpoint to create Attestation for a vendor
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/attestations")]
        [ProducesResponseType(typeof(SuccessResponse<VendorAttestationDTO>), 200)]
        public async Task<IActionResult> CreateAttestationsForVendor(Guid id, [FromBody] VendorAttestationForCreationDTO dtoModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your vendor signup request failed",
                        errors = ModelState.Error()
                    });
                }

                // check if userid is equal to null or userid is empty
                if (id == null || id.Equals(""))
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {id} not found",
                        errors = new { }
                    });
                }

                //Get userClaims from token
                var userClaims = User.UserClaims();

                // check if user exists and return error message is user does not exist
                var userExists = await _userRepository.FirstOrDefault(x => x.Id == id);

                if (userExists == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {id} not found",
                        errors = new { }
                    });
                }


                var vendorAttestation = new VendorAttestation
                {
                    UserId = userExists.Id,
                    FirstName = dtoModel.FirstName,
                    LastName = dtoModel.LastName,
                    AttestedAt = dtoModel.AttestedAt,
                    CreateAt = DateTime.UtcNow,
                    CreatedById = userExists.Id
                };


                await _vendorAttestationRepository.AddAsync(vendorAttestation);
                await _vendorAttestationRepository.SaveChangesAsync();


                var vendorAttestationDTO = _mapper.Map<VendorAttestationDTO>(vendorAttestation);


                var userActivity = new UserActivity
                {
                    EventType = "Vendor Attestation Creation - Registration Section 6",
                    UserId = userExists.Id,
                    ObjectClass = "VENDOR",
                    ObjectId = userExists.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();


                return Ok(new SuccessResponse<VendorAttestationDTO>
                {
                    success = true,
                    message = "Vendor Attestation created successfully",
                    data = vendorAttestationDTO
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to Get Attestations For Vendors
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(SuccessResponse<VendorProfileDTO>), 200)]
        [HttpGet("{id}/attestations", Name = "GetAttestationsForVendor")]
        [ProducesResponseType(typeof(SuccessResponse<VendorAttestationDTO>), 200)]
        public async Task<IActionResult> GetAttestationsForVendor(Guid id, [FromQuery] VendorAttestationParameter parameter)
        {
            try
            {
                // check if id is equal to null or id is empty
                if (id == null || id.Equals(""))
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                // check if vendor exists and return error message is user does not exist
                var vendorExists = await _vendorProfileRepository.ExistsAsync(x => x.UserId == id);

                if (!vendorExists)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                // get the attestations for the userId supplied
                var attestations = await _vendorProfileRepository.GetAttestationsForVendor(parameter, id);

                // convert business service to business service dto
                var attestationsDto = _mapper.Map<IEnumerable<VendorAttestationDTO>>(attestations);

                // pagination variables
                var prevLink = attestations.HasPrevious
                    ? CreateResourceUri(parameter, "GetAttestationsForVendor", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = attestations.HasNext ? CreateResourceUri(parameter, "GetAttestationsForVendor", ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(parameter, "GetAttestationsForVendor", ResourceUriType.CurrentPage);

                // create pagination items
                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = attestations.TotalPages,
                    perPage = attestations.PageSize,
                    totalEntries = attestations.TotalCount
                };

                // return success response
                return Ok(new PagedResponse<IEnumerable<VendorAttestationDTO>>
                {
                    success = true,
                    message = "Vendor attestations retrieved successfully",
                    data = attestationsDto,
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
        /// Endpoint to create document for a vendor
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}/documents", Name = "CreateVendorDocument")]
        [ProducesResponseType(typeof(SuccessResponse<VendorDocumentDTO>), 200)]
        public async Task<IActionResult> CreateVendorDocument(Guid id, [FromForm] VendorDocumentForCreationDTO dtoModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = new Dictionary<string, string[]>();
                    foreach (KeyValuePair<string, string[]> item in ModelState.Error())
                    {
                        foreach (var arrayValue in item.Value)
                        {
                            if (arrayValue.Length > 1)
                            {
                                errors.Add(item.Key, item.Value);
                            }
                        }
                    }
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your vendor document creation failed",
                        errors = errors
                    });
                }

                // check if userid is equal to null or userid is empty
                if (id == null || id.Equals(""))
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                //Get userClaims from token
                var userClaims = User.UserClaims();

                // check if user exists and return error message is user does not exist
                var userExists = await _userRepository.FirstOrDefault(x => x.Id == id);

                if (userExists == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }


                if (dtoModel != null)
                {
                    //check if vendor document type id exists and return not found response if it doesn't
                    var vendorDocumentTypeExists = await _vendorDocumentTypeRepository.ExistsAsync(x => x.Id == dtoModel.VendorDocumentTypeId);

                    if (!vendorDocumentTypeExists)
                    {
                        var error = new Dictionary<string, string[]>();
                        error.Add("vendorDocumentTypeId", new[] { "vendorDocumentTypeId does not exist" });

                        return NotFound(new ErrorResponse<object>
                        {
                            success = false,
                            message = $"Your vendor document creation request failed",
                            errors = error
                        });
                    }

                    //upload file and get string representation of path
                    var fileUploadResponse = _photoAcessor.AddPhoto(dtoModel.File);
                    var file = JsonConvert.SerializeObject(fileUploadResponse);

                    //initialize vendor document for later addition to table
                    var vendorDocument = new VendorDocument
                    {
                        UserId = userExists.Id,
                        Name = dtoModel.File.FileName,
                        Description = dtoModel.Description,
                        File = file,
                        CreatedById = userExists.Id,
                        VendorDocumentTypeId = dtoModel.VendorDocumentTypeId,
                        Status = EVendorDocumentStatus.PENDING
                    };

                    //save new vendor document to the vendor documents table
                    await _vendorDocumentRepository.AddAsync(vendorDocument);
                    await _vendorDocumentRepository.SaveChangesAsync();

                    var vendorDocumentDTO = _mapper.Map<VendorDocumentDTO>(vendorDocument);

                    // add new user activity to user activities table
                    var userActivity = new UserActivity
                    {
                        EventType = "Vendor Document Creation - Registration Section 5",
                        UserId = userExists.Id,
                        ObjectClass = "VENDOR",
                        ObjectId = userExists.Id,
                        IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                    };

                    //save changes made on user activities table
                    await _userActivityRepository.AddAsync(userActivity);
                    await _userActivityRepository.SaveChangesAsync();

                    // convert enum status to string equivalent
                    vendorDocumentDTO.Status.ToString("G");

                    //return successful response
                    return Ok(new SuccessResponse<VendorDocumentDTO>
                    {
                        success = true,
                        message = "Vendor document created successfully",
                        data = vendorDocumentDTO
                    });
                }
                else
                {
                    //return bad request if file is not present in request
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"File not present in request.",
                        errors = new { }
                    });
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to get documents for vendor
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/documents", Name = "GetVendorDocumentsForVendor")]
        [ProducesResponseType(typeof(SuccessResponse<VendorDocumentDTO>), 200)]
        public async Task<IActionResult> GetVendorDocumentsForVendor(Guid id, [FromQuery] VendorDocumentParameter parameter)
        {
            try
            {
                // check if id is equal to null or id is empty
                if (id == null || id.Equals(""))
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                // check if vendor exists and return error message is user does not exist
                var vendorExists = await _userRepository.ExistsAsync(x => x.Id == id);

                if (!vendorExists)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                // get the vendor documents for the userId supplied
                var vendorDocuments = await _vendorProfileRepository.GetDocumentsForVendor(parameter, id);

                // convert vendor document to vendor documents dto
                var vendorDocumentDTOs = _mapper.Map<IEnumerable<VendorDocumentDTO>>(vendorDocuments);

                // pagination variables
                var prevLink = vendorDocuments.HasPrevious
                    ? CreateResourceUri(parameter, "GetVendorDocumentsForVendor", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = vendorDocuments.HasNext ? CreateResourceUri(parameter, "GetVendorDocumentsForVendor", ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(parameter, "GetVendorDocumentsForVendor", ResourceUriType.CurrentPage);

                // create pagination items
                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = vendorDocuments.TotalPages,
                    perPage = vendorDocuments.PageSize,
                    totalEntries = vendorDocuments.TotalCount
                };

                // return success response
                return Ok(new PagedResponse<IEnumerable<VendorDocumentDTO>>
                {
                    success = true,
                    message = "Vendor documents retrieved successfully",
                    data = vendorDocumentDTOs,
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
        /// Endpoint to get vendor document types
        /// </summary>
        /// <returns></returns>
        [HttpGet("documentTypes", Name = "GetVendorDocumentTypes")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<VendorDocumentTypeDTO>>), 200)]
        public async Task<IActionResult> GetVendorDocumentTypes([FromQuery] VendorDocumentTypeParameter parameter)
        {
            try
            {
                // get the vendor document types
                var vendorDocumentTypes = await _vendorDocumentTypeRepository.GetAllVendorDocumentTypes(parameter);

                // convert vendor document to vendor documents dto
                var vendorDocumentTypeDTOs = _mapper.Map<IEnumerable<VendorDocumentTypeDTO>>(vendorDocumentTypes);

                // pagination variables
                var prevLink = vendorDocumentTypes.HasPrevious
                    ? CreateResourceUri(parameter, "GetVendorDocumentTypes", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = vendorDocumentTypes.HasNext ? CreateResourceUri(parameter, "GetVendorDocumentTypes", ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(parameter, "GetVendorDocumentTypes", ResourceUriType.CurrentPage);

                // create pagination items
                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = vendorDocumentTypes.TotalPages,
                    perPage = vendorDocumentTypes.PageSize,
                    totalEntries = vendorDocumentTypes.TotalCount
                };

                // return success response
                return Ok(new PagedResponse<IEnumerable<VendorDocumentTypeDTO>>
                {
                    success = true,
                    message = "Vendor document types retrieved successfully",
                    data = vendorDocumentTypeDTOs,
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
        /// Endpoint to create vendor directors
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/directors")]
        [ProducesResponseType(typeof(SuccessResponse<VendorDirectorDTO>), 200)]
        public async Task<IActionResult> CreateVendorDirectors(Guid id, [FromForm] VendorDirectorForCreationDTO dtoModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = new Dictionary<string, string[]>();
                    foreach (KeyValuePair<string, string[]> item in ModelState.Error())
                    {
                        foreach (var arrayValue in item.Value)
                        {
                            if (arrayValue.Length > 1)
                            {
                                errors.Add(item.Key, item.Value);
                            }
                        }
                    }

                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your vendor director creation request failed",
                        errors = errors
                    });
                }

                // check if userid is equal to null or userid is empty
                if (id == null || id.Equals(""))
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                //Get userClaims from token
                var userClaims = User.UserClaims();

                // check if user exists and return error message is user does not exist
                var userExists = await _userRepository.FirstOrDefault(x => x.Id == id);

                if (userExists == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                //check if country and state exists and return bad request if they don't
                var countryExists = _countryRepository.ExistsAsync(x => x.Name == dtoModel.Country);
                var stateExists = _countryRepository.ExistsAsync(x => x.Name == dtoModel.State);

                if (countryExists == null || stateExists == null)
                {
                    var error = new Dictionary<string, string[]>();

                    if (countryExists == null && stateExists == null)
                    {
                        error.Add("country", new[] { "Country does not exist" });
                        error.Add("state", new[] { "State does not exist" });

                        return NotFound(new ErrorResponse<Dictionary<string, string[]>>
                        {
                            success = false,
                            message = "Your vendor director creation request failed",
                            errors = error
                        });
                    }
                    else if (countryExists == null)
                    {
                        error.Add("country", new[] { "Country does not exist" });

                        return NotFound(new ErrorResponse<Dictionary<string, string[]>>
                        {
                            success = false,
                            message = "Your vendor director creation request failed",
                            errors = error
                        });
                    }
                    else
                    {
                        error.Add("state", new[] { "State does not exist" });

                        return NotFound(new ErrorResponse<Dictionary<string, string[]>>
                        {
                            success = false,
                            message = "Your vendor director creation request failed",
                            errors = error
                        });
                    }
                }


                if ((dtoModel.PassportPhoto != null && dtoModel.PassportPhoto.Length > 0)
                    && (dtoModel.IdentificationFile != null && dtoModel.IdentificationFile.Length > 0))
                {
                    //get string representation of passport photo and identification file
                    var passportPhotoResult = _photoAcessor.AddPhoto(dtoModel.PassportPhoto);
                    var passportPhoto = JsonConvert.SerializeObject(passportPhotoResult);

                    var identificationFileResult = _photoAcessor.AddPhoto(dtoModel.IdentificationFile);
                    var identificationFile = JsonConvert.SerializeObject(identificationFileResult);

                    var vendorDirectorExists = await _vendorDirectorRepository.FirstOrDefault(x => x.LastName == dtoModel.LastName
                                                    && x.FirstName == dtoModel.FirstName && x.UserId == userExists.Id);

                    //if vendor director exists, fetch the id and create vendor director certificate
                    if (vendorDirectorExists == null)
                    {
                        //create record into vendor directors table
                        var vendorDirector = new VendorDirector
                        {
                            UserId = userExists.Id,
                            Title = dtoModel.Title,
                            PhoneNumber = dtoModel.PhoneNumber,
                            AddressLine1 = dtoModel.AddressLine1,
                            AddressLine2 = dtoModel.AddressLine2,
                            City = dtoModel.City,
                            State = dtoModel.State,
                            Country = dtoModel.Country,
                            FirstName = dtoModel.FirstName,
                            LastName = dtoModel.LastName,
                            Email = userExists.Email,
                            PassportPhoto = passportPhoto,
                            IdentificationType = dtoModel.IdentificationType,
                            IdentificationFile = identificationFile,
                            CreateAt = DateTime.Now,
                            CreatedById = userExists.Id
                        };


                        await _vendorDirectorRepository.AddAsync(vendorDirector);
                        await _vendorDirectorRepository.SaveChangesAsync();

                        //save into vendor certificates table
                        //upload vendor ceritifcate files and convert to string
                        if (dtoModel.Certifications != null && dtoModel.Certifications.Count > 0)
                        {
                            //fetch vendor director id of recently created vendor directory
                            vendorDirectorExists = await _vendorDirectorRepository.FirstOrDefault(x => x.LastName == dtoModel.LastName
                                                            && x.FirstName == dtoModel.FirstName && x.UserId == userExists.Id);

                            List<VendorDirectorCertificate> vendorDirectorCertificateList = new List<VendorDirectorCertificate>();
                            foreach (var item in dtoModel.Certifications)
                            {
                                var fileResult = _photoAcessor.AddPhoto(item);
                                string file = JsonConvert.SerializeObject(fileResult);

                                var vendorDirectorCertificate = new VendorDirectorCertificate
                                {
                                    VendorDirectorId = vendorDirectorExists.Id,
                                    CreatedById = userExists.Id,
                                    File = file
                                };
                                //add entity to vendor directo certificates list
                                vendorDirectorCertificateList.Add(vendorDirectorCertificate);
                            }//end foreach

                            //save changes to db
                            await _vendorDirectorCertificateRepository.AddRangeAsync(vendorDirectorCertificateList);
                            await _vendorDirectorCertificateRepository.SaveChangesAsync();
                        }//end if

                    }// end if
                    else
                    {
                        //save into vendor certificates table
                        //upload vendor ceritifcate files and convert to string
                        if (dtoModel.Certifications != null && dtoModel.Certifications.Count > 0)
                        {
                            foreach (var item in dtoModel.Certifications)
                            {
                                var fileResult = _photoAcessor.AddPhoto(item);
                                string file = JsonConvert.SerializeObject(fileResult);

                                var vendorDirectorCertificate = new VendorDirectorCertificate
                                {
                                    VendorDirectorId = vendorDirectorExists.Id,
                                    CreatedById = userExists.Id,
                                    File = file
                                };
                                //add entity to vendor director certificates table
                                await _vendorDirectorCertificateRepository.AddAsync(vendorDirectorCertificate);
                            }//end foreach

                            //save changes to db
                            await _vendorDirectorCertificateRepository.SaveChangesAsync();
                        }//end if
                    }


                    //save into user activities table                 
                    var userActivity = new UserActivity
                    {
                        EventType = "Vendor Director Creation - Registration Section 2",
                        UserId = userExists.Id,
                        ObjectClass = "VENDOR",
                        ObjectId = userExists.Id,
                        IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                    };

                    await _userActivityRepository.AddAsync(userActivity);
                    await _userActivityRepository.SaveChangesAsync();

                    var vendorDirectorToReturn = await _vendorDirectorRepository
                        .GetVendorDirectorWithCertificates(userExists.Id, vendorDirectorExists.Id);

                    //vendor director to return in response
                    var vendorDirectorDto = _mapper.Map<VendorDirectorDTO>(vendorDirectorExists);

                    return Ok(new SuccessResponse<VendorDirectorDTO>
                    {
                        success = true,
                        message = "Vendor director created successfully",
                        data = vendorDirectorDto
                    });
                }
                else
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Passport Photo or Identification File not provided.",
                        errors = new { }
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to get vendor directors
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/directors", Name = "GetVendorDirectors")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<VendorDirectorDTO>>), 200)]
        public async Task<IActionResult> GetVendorDirectors(Guid id, [FromQuery] VendorDirectorParameter parameter)
        {
            try
            {
                // check if id is equal to null or id is empty
                if (id == null || id.Equals(""))
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                //Get userClaims from token
                var userClaims = User.UserClaims();

                // check if vendor exists and return error message is user does not exist
                var vendorExists = await _userRepository.FirstOrDefault(x => x.Id == id && x.UserType == EUserType.VENDOR);

                if (vendorExists == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                // get the directors for the userId supplied
                var vendorDirectors = await _vendorProfileRepository.GetVendorDirectorWithCertificates(parameter, id);

                // convert vendor directors to vendor directors dto
                var vendorDirectorsDto = _mapper.Map<IEnumerable<VendorDirectorDTO>>(vendorDirectors);

                // pagination variables
                var prevLink = vendorDirectors.HasPrevious
                    ? CreateResourceUri(parameter, "GetVendorDirectors", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = vendorDirectors.HasNext ? CreateResourceUri(parameter, "GetVendorDirectors", ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(parameter, "GetVendorDirectors", ResourceUriType.CurrentPage);

                // create pagination items
                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = vendorDirectors.TotalPages,
                    perPage = vendorDirectors.PageSize,
                    totalEntries = vendorDirectors.TotalCount
                };

                // return success response
                return Ok(new PagedResponse<IEnumerable<VendorDirectorDTO>>
                {
                    success = true,
                    message = "Vendor directors retrieved successfully",
                    data = vendorDirectorsDto,
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
        /// Endpoint to get vendor registration category
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/registrationCategory", Name = "GetRegistrationCategory")]
        [ProducesResponseType(typeof(SuccessResponse<VendorRegistrationCategoryDTO>), 200)]
        public async Task<IActionResult> GetRegistrationCategory(Guid id, [FromQuery] RegistrationCategoryParameter parameter)
        {
            try
            {
                // check if id is equal to null or id is empty
                if (id == null || id.Equals(""))
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                //Get userClaims from token
                var userClaims = User.UserClaims();

                // check if vendor exists and return error message is user does not exist
                var vendorExists = await _userRepository.FirstOrDefault(x => x.Id == id && x.UserType == EUserType.VENDOR);

                if (vendorExists == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                // get the registration category sorting by date for the userId supplied
                var vendorRegistrationCategory = await _vendorRegistrationCategoryRepository.GetRegistrationCategoryByDate(id, parameter);

                // convert vendor directors to vendor directors dto
                var vendorRegistrationCategoryDto = _mapper.Map<IEnumerable<VendorRegistrationCategoryDTO>>(vendorRegistrationCategory);

                //get string representation of registration category
                foreach (var item in vendorRegistrationCategoryDto)
                {
                    item.RegistrationPlan = _mapper.Map<RegistrationPlanDTO>(item.RegistrationPlan);
                }

                // pagination variables
                var prevLink = vendorRegistrationCategory.HasPrevious
                    ? CreateResourceUri(parameter, "GetRegistrationCategory", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = vendorRegistrationCategory.HasNext ? CreateResourceUri(parameter, "GetRegistrationCategory", ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(parameter, "GetRegistrationCategory", ResourceUriType.CurrentPage);

                // create pagination items
                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,

                    totalPages = vendorRegistrationCategory.TotalPages,
                    perPage = vendorRegistrationCategory.PageSize,
                    totalEntries = vendorRegistrationCategory.TotalCount
                };

                // return success response
                return Ok(new PagedResponse<IEnumerable<VendorRegistrationCategoryDTO>>
                {
                    success = true,
                    message = "Vendor registration category retrieved successfully",
                    data = vendorRegistrationCategoryDto,
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
        /// Endpoint to update a Vendor's details
        /// Where id = UserId
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<VendorProfileDTO>), 200)]
        public async Task<IActionResult> UpdateVendorProfile(VendorProfileForUpdateDTO vendorModelForUpdate, Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("id");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your Vendor details update failed",
                        errors = ModelState.Error()
                    });
                }

                //Check if companyName exists
                var companyNameExists = await _vendorProfileRepository.ExistsAsync(vp => vp.CompanyName == vendorModelForUpdate.CompanyName);

                if (companyNameExists)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your Vendor details update failed",
                        errors = new
                        {
                            companyName = new string[] { "Company name already exists" }
                        }
                    });
                }


                //Check if State exists
                var stateExists = await _stateRepository.ExistsAsync(vp => vp.Name == vendorModelForUpdate.State && vp.Name == vendorModelForUpdate.Correspondences.State);

                if (!stateExists)
                {
                    var correspondenceError = new Dictionary<string, string[]>();
                    correspondenceError.Add("state", new[] { "State does not exists" });

                    var error = new Dictionary<string, object>();
                    error.Add("state", new object[] { "State does not exist" });
                    error.Add("correspondence[0]", correspondenceError);

                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your Vendor details update failed",
                        errors = error
                    });
                }

                //Check if Country exists
                var countryExists = await _countryRepository.ExistsAsync(vp => vp.Name == vendorModelForUpdate.Country && vp.Name == vendorModelForUpdate.Correspondences.Country);

                if (!countryExists)
                {
                    var correspondenceError = new Dictionary<string, string[]>();
                    correspondenceError.Add("country", new[] { "Country does not exist" });

                    var error = new Dictionary<string, object>();
                    error.Add("country", new object[] { "Country does not exist" });
                    error.Add("correspondence[0]", correspondenceError);

                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your Vendor details update failed",
                        errors = error
                    });
                }

                //Get userClaims from token
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == id && u.UserType == EUserType.VENDOR);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User/Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                var vendorProfile = await _vendorProfileRepository.GetVendorProfile(user.Id);

                if (vendorProfile == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {user.Id} not found",
                        errors = new { }
                    });
                }

                var vendorProfileCorrespondence = await _vendorCorrespondenceRepository.GetVendorCorrespondence(user.Id);

                var vendorProfileCorrespondenceForUpdate = vendorModelForUpdate.Correspondences;

                //map changes to User entity
                _mapper.Map(vendorModelForUpdate, user);

                //map changes to VendorProfile entity
                _mapper.Map(vendorModelForUpdate, vendorProfile);

                //map changes to VendorCorrespondence entity
                _mapper.Map(vendorProfileCorrespondenceForUpdate, vendorProfileCorrespondence);

                vendorProfile.UpdatedAt = DateTime.UtcNow;

                //Update entities
                _userRepository.Update(user);
                _vendorProfileRepository.Update(vendorProfile);
                _vendorCorrespondenceRepository.Update(vendorProfileCorrespondence);

                //Save changes
                await _userRepository.SaveChangesAsync();
                await _vendorProfileRepository.SaveChangesAsync();
                await _vendorCorrespondenceRepository.SaveChangesAsync();

                //Create, add and save new UserActivity
                var userActivity = new UserActivity
                {
                    EventType = $"Vendor Profile Update - Registration Section 1",
                    UserId = user.Id,
                    ObjectClass = "VENDOR",
                    ObjectId = user.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();


                //Map changed entities to response DTO
                var vendorUpdateDTO = _mapper.Map<VendorProfileDTO>(vendorProfile);

                var vendorCorrespondenceDTO = _mapper.Map<VendorCorrespondenceDTO>(vendorProfileCorrespondence);

                vendorUpdateDTO.VendorCorrespondence = vendorCorrespondenceDTO;

                return Ok(new SuccessResponse<VendorProfileDTO>
                {
                    success = true,
                    message = "Vendor details updated successfully",
                    data = vendorUpdateDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to soft delete a Vendor Director
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}/directors/{directorId}")]
        [ProducesResponseType(typeof(SuccessResponse<Object>), 200)]
        public async Task<IActionResult> DeleteVendorDirector(Guid id, Guid directorId)
        {
            try
            {
                //Get userClaims from token
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == id && u.UserType == EUserType.VENDOR);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {id} not found",
                        errors = new { }
                    });
                }

                var vendorProfile = await _vendorProfileRepository.GetVendorProfile(user.Id);

                if (vendorProfile == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                var vendorDirector = await _vendorProfileRepository.GetVendorDirector(directorId);

                if (vendorDirector == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor Director with id {directorId} not found",
                        errors = new { }
                    });
                }

                vendorDirector.Deleted = true;
                vendorDirector.DeletedAt = DateTime.UtcNow;

                await _vendorProfileRepository.SaveChangesAsync();

                //Create, add and save new UserActivity
                var userActivity = new UserActivity
                {
                    EventType = $"Vendor Director Deleted",
                    UserId = user.Id,
                    ObjectClass = "VENDOR-DIRECTOR",
                    ObjectId = vendorDirector.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<VendorProfileDTO>
                {
                    success = true,
                    message = "Vendor director deleted successfully",
                    data = { }
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to soft delete a Vendor document
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}/documents/{documentId}", Name = "DeleteVendorDocument")]
        [ProducesResponseType(typeof(SuccessResponse<VendorDocumentDTO>), 200)]
        public async Task<IActionResult> DeleteVendorDocument(Guid id, Guid documentId)
        {
            try
            {
                //Get userClaims from token
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == id && u.UserType == EUserType.VENDOR);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {id} not found",
                        errors = new { }
                    });
                }

                var vendorDocument = await _vendorDocumentRepository.FirstOrDefault(x => x.Id == documentId);

                if (vendorDocument == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor Document with id {documentId} not found",
                        errors = new { }
                    });
                }

                vendorDocument.Deleted = true;
                vendorDocument.DeletedAt = DateTime.UtcNow;

                await _vendorDocumentRepository.SaveChangesAsync();

                //Create, add and save new UserActivity
                var userActivity = new UserActivity
                {
                    EventType = $"Vendor Document Deleted",
                    UserId = user.Id,
                    ObjectClass = "VENDOR-DOCUMENT",
                    ObjectId = vendorDocument.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                var vendorDocumentDto = _mapper.Map<VendorDocumentDTO>(vendorDocument);

                return Ok(new SuccessResponse<VendorDocumentDTO>
                {
                    success = true,
                    message = "Vendor document deleted successfully",
                    data = vendorDocumentDto
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// Endpoint to update a vendor's registration plan
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}/plans")]
        [ProducesResponseType(typeof(SuccessResponse<Object>), 200)]
        public async Task<IActionResult> UpdateVendorRegistrationPlan([FromRoute] Guid id, VendorProfileRegistrationPlansForUpdateDTO vendorProfileRegistrationPlans)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your vendor signup request failed",
                        errors = ModelState.Error()
                    });
                }

                //Get userClaims from token
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == id && u.UserType == EUserType.VENDOR);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {id} not found",
                        errors = new { }
                    });
                }

                var vendorProfile = await _vendorProfileRepository.GetVendorProfile(user.Id);

                if (vendorProfile == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} not found",
                        errors = new { }
                    });
                }

                vendorProfile.RegistrationPlanId = vendorProfileRegistrationPlans.registrationPlanId;

                //Update and save VendorProfile
                _vendorProfileRepository.Update(vendorProfile);
                await _vendorProfileRepository.SaveChangesAsync();

                //Create, add and save new UserActivity
                var userActivity = new UserActivity
                {
                    EventType = $"Vendor Registration Plan Creation - Registration Section 4",
                    UserId = user.Id,
                    ObjectClass = "VENDOR",
                    ObjectId = user.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<VendorProfileDTO>
                {
                    success = true,
                    message = "Vendor registration plan updated successfully",
                    data = { }
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// Endpoint to update a Vendor Director's details
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}/directors/{directorId}")]
        [ProducesResponseType(typeof(SuccessResponse<VendorDirectorDTO>), 200)]
        public async Task<IActionResult> UpdateVendorDirector([FromForm]VendorDirectorForUpdateDTO vendorDirectorForUpdate, Guid id, Guid directorId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("id");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your vendor director creation request failed",
                        errors = ModelState.Error()
                    });
                }

                //Check if State exists
                var stateExists = await _stateRepository.ExistsAsync(vp => vp.Name == vendorDirectorForUpdate.State);

                if (!stateExists)
                {
                    var error = new Dictionary<string, string[]>();
                    error.Add("state", new[] { "State does not exist" });

                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your vendor director creation request failed",
                        errors = error
                    });
                }

                //Check if Country exists
                var countryExists = await _countryRepository.ExistsAsync(vp => vp.Name == vendorDirectorForUpdate.Country);

                if (!countryExists)
                {
                    var error = new Dictionary<string, string[]>();
                    error.Add("country", new[] { "Country does not exist" });

                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your vendor director creation request failed",
                        errors = error
                    });
                }

                //Get userClaims from token
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == id && u.UserType == EUserType.VENDOR);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {id} not found",
                        errors = new { }
                    });
                }

                var vendorDirector = await _vendorDirectorRepository.SingleOrDefault(vd => vd.Id == directorId && vd.UserId == user.Id);

                if (vendorDirector == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor Director with id {directorId} not found",
                        errors = new { }
                    });
                }

                if (vendorDirectorForUpdate.PassportPhoto != null)
                {
                    //get string representation of passport photo 
                    var passportPhotoResult = _photoAcessor.AddPhoto(vendorDirectorForUpdate.PassportPhoto);
                    var passportPhoto = JsonConvert.SerializeObject(passportPhotoResult);

                    vendorDirector.PassportPhoto = passportPhoto;
                }

                if (vendorDirectorForUpdate.IdentificationFile != null)
                {

                    //get string representation of identification file
                    var identificationFileResult = _photoAcessor.AddPhoto(vendorDirectorForUpdate.IdentificationFile);
                    var identificationFile = JsonConvert.SerializeObject(identificationFileResult);

                    vendorDirector.IdentificationFile = identificationFile;
                }

                //save new certificates into vendor certificates table
                //upload vendor ceritifcate files and convert to string
                if (vendorDirectorForUpdate.Certifications != null && vendorDirectorForUpdate.Certifications.Count > 0)
                {

                    List<VendorDirectorCertificate> vendorDirectorCertificateList = new List<VendorDirectorCertificate>();
                    foreach (var item in vendorDirectorForUpdate.Certifications)
                    {
                        var fileResult = _photoAcessor.AddPhoto(item);
                        string file = JsonConvert.SerializeObject(fileResult);

                        var vendorDirectorCertificate = new VendorDirectorCertificate
                        {
                            VendorDirectorId = vendorDirector.Id,
                            CreatedById = user.Id,
                            File = file
                        };
                        //add entity to vendor directo certificates list
                        vendorDirectorCertificateList.Add(vendorDirectorCertificate);
                    }

                    //save changes to db
                    await _vendorDirectorCertificateRepository.AddRangeAsync(vendorDirectorCertificateList);
                    await _vendorDirectorCertificateRepository.SaveChangesAsync();
                }

                //Update vendor director
                vendorDirector.Title = vendorDirectorForUpdate.Title;
                vendorDirector.PhoneNumber = vendorDirectorForUpdate.PhoneNumber;
                vendorDirector.AddressLine1 = vendorDirectorForUpdate.AddressLine1;
                vendorDirector.AddressLine2 = vendorDirectorForUpdate.AddressLine2;
                vendorDirector.City = vendorDirectorForUpdate.City;
                vendorDirector.State = vendorDirectorForUpdate.State;
                vendorDirector.Country = vendorDirectorForUpdate.Country;
                vendorDirector.FirstName = vendorDirectorForUpdate.FirstName;
                vendorDirector.LastName = vendorDirectorForUpdate.LastName;
                vendorDirector.Email = vendorDirectorForUpdate.Email;
                vendorDirector.UpdatedAt = DateTime.UtcNow;

                //update changes
                _vendorDirectorRepository.Update(vendorDirector);

                await _vendorDirectorRepository.SaveChangesAsync();


                //Create, add and save new UserActivity
                var userActivity = new UserActivity
                {
                    EventType = $"Vendor Director update - Registration Section 2",
                    UserId = user.Id,
                    ObjectClass = "VENDOR",
                    ObjectId = user.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                var vendorDirectorToReturn = await _vendorDirectorRepository.GetVendorDirectorWithCertificates(user.Id, vendorDirector.Id);

                //Map created Vendor Director to Ouput DTO
                var vendorDirectorDTO = _mapper.Map<VendorDirectorDTO>(vendorDirectorToReturn);


                return Ok(new SuccessResponse<VendorDirectorDTO>
                {
                    success = true,
                    message = "Vendor director details updated successfully",
                    data = vendorDirectorDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }




        /// <summary>
        /// Endpoint to Get All Registration Categories in the Organization
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("registrationPlan")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<RegistrationPlan>>), 200)]
        public async Task<IActionResult> GetAllRegistrationCategories([FromQuery] RegistrationPlanParameter parameter)
        {
            try
            {
                var registrationPlan = await _registrationPlanRepository.GetAllRegistrationCategories(parameter);

                // pagination variables
                var prevLink = registrationPlan.HasPrevious
                    ? CreateResourceUri(parameter, "GetAllRegistrationCategories", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = registrationPlan.HasNext ? CreateResourceUri(parameter, "GetAllRegistrationCategories", ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(parameter, "GetAllRegistrationCategories", ResourceUriType.CurrentPage);

                // create pagination items
                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = registrationPlan.TotalPages,
                    perPage = registrationPlan.PageSize,
                    totalEntries = registrationPlan.TotalCount
                };

                // return success response
                return Ok(new PagedResponse<IEnumerable<RegistrationPlan>>
                {
                    success = true,
                    message = "Registration Categories retrieved successfully",
                    data = registrationPlan,
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
        /// Endpoint to Get All States in the Organization
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("states")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<State>>), 200)]
        public async Task<IActionResult> GetAllStates([FromQuery] StateParameter parameter)
        {
            try
            {
                var states = await _stateRepository.GetAllStates(parameter);

                // pagination variables
                var prevLink = states.HasPrevious
                    ? CreateResourceUri(parameter, "GetAllStates", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = states.HasNext ? CreateResourceUri(parameter, "GetAllStates", ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(parameter, "GetAllStates", ResourceUriType.CurrentPage);

                // create pagination items
                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = states.TotalPages,
                    perPage = states.PageSize,
                    totalEntries = states.TotalCount
                };

                // return success response
                return Ok(new PagedResponse<IEnumerable<State>>
                {
                    success = true,
                    message = "States retrieved successfully",
                    data = states,
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
        /// Endpoint to Get All Countries in the organization
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Countries")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<Country>>), 200)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCountries([FromQuery] CountryParameter parameter)
        {
            try
            {
                var countries = await _countryRepository.GetAllCountries(parameter);

                // pagination variables
                var prevLink = countries.HasPrevious
                    ? CreateResourceUri(parameter, "GetAllCountries", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = countries.HasNext ? CreateResourceUri(parameter, "GetAllCountries", ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(parameter, "GetAllCountries", ResourceUriType.CurrentPage);

                // create pagination items
                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = countries.TotalPages,
                    perPage = countries.PageSize,
                    totalEntries = countries.TotalCount
                };

                // return success response
                return Ok(new PagedResponse<IEnumerable<Country>>
                {
                    success = true,
                    message = "Countries retrieved successfully",
                    data = countries,
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
        /// POST endpoint for vendor selected registration categories
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/vendorRegistrationCategory/{registrationPlanId}")]
        [ProducesResponseType(typeof(SuccessResponse<VendorRegistrationCategoryDTO>), 200)]
        public async Task<IActionResult> CreateRegistrationCategory(Guid id, Guid registrationPlanId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your registrationPlan post request failed",
                        errors = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == id && u.UserType == EUserType.VENDOR);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {id} not found",
                        errors = new { }
                    });
                }

                var registrationPlan = await _registrationPlanRepository.SingleOrDefault(r => r.Id == registrationPlanId);

                if (registrationPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Resgistration Plan with id {registrationPlanId} not found",
                        errors = new { }
                    });
                }

                VendorRegistrationCategory vendorRegistrationCategory = new VendorRegistrationCategory();

                var vendorRegCategory = await _vendorRegistrationCategoryRepository.FirstOrDefault(x => x.RegistrationPlanId == registrationPlanId);
                var vendorProfile = await _vendorProfileRepository.FirstOrDefault(v => v.UserId == userClaims.UserId);

                if (vendorRegCategory != null)
                {
                    _vendorRegistrationCategoryRepository.Remove(vendorRegCategory);
                    await _vendorRegistrationCategoryRepository.SaveChangesAsync();

                    vendorRegistrationCategory = new VendorRegistrationCategory
                    {
                        UserId = user.Id,
                        RegistrationPlanId = registrationPlan.Id
                    };


                    await _vendorRegistrationCategoryRepository.AddAsync(vendorRegistrationCategory);
                    await _vendorRegistrationCategoryRepository.SaveChangesAsync();

                }
                else
                {
                    vendorRegistrationCategory = new VendorRegistrationCategory
                    {
                        UserId = user.Id,
                        RegistrationPlanId = registrationPlan.Id
                    };

                    await _vendorRegistrationCategoryRepository.AddAsync(vendorRegistrationCategory);
                    await _vendorRegistrationCategoryRepository.SaveChangesAsync();
                }

                if (vendorProfile != null)
                {
                    vendorProfile.RegistrationPlanId = registrationPlan.Id;
                    _vendorProfileRepository.Update(vendorProfile);
                    await _vendorProfileRepository.SaveChangesAsync();
                }

                //Create, add and save new UserActivity
                var userActivity = new UserActivity
                {
                    EventType = $"Vendor Registration Category",
                    UserId = user.Id,
                    ObjectClass = "VENDOR",
                    ObjectId = user.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                var registrationCategoryDTO = _mapper.Map<VendorRegistrationCategoryDTO>(vendorRegistrationCategory);
                var registrationPlanDTO = _mapper.Map<RegistrationPlanDTO>(registrationPlan);

                registrationCategoryDTO.RegistrationPlan = registrationPlanDTO;

                // return success response
                return Ok(new PagedResponse<VendorRegistrationCategoryDTO>
                {
                    success = true,
                    message = "Registration Category created successfully",
                    data = registrationCategoryDTO
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }





        /// <summary>
        /// Endpoint to Complete All Vendor Registration
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}/completeVendorRegistration", Name = "CompleteVendorRegistration")]
        [ProducesResponseType(typeof(SuccessResponse<VendorProfileDTO>), 200)]
        public async Task<IActionResult> CompleteVendorRegistration(Guid id)
        {
            try
            {
                //Get userClaims from token
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == id && u.UserType == EUserType.VENDOR);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {id} not found",
                        errors = new { }
                    });
                }

                var vendorProfile = await _vendorProfileRepository.FirstOrDefault(x => x.UserId == user.Id);

                if (vendorProfile == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor with id {id} does not have Profile",
                        errors = new { }
                    });
                }

                vendorProfile.IsRegistrationComplete = true;

                _vendorProfileRepository.Update(vendorProfile);
                await _vendorProfileRepository.SaveChangesAsync();

                //Create, add and save new UserActivity
                var userActivity = new UserActivity
                {
                    EventType = $"Vendor Registration Completed",
                    UserId = user.Id,
                    ObjectClass = "VENDOR-DOCUMENT",
                    ObjectId = vendorProfile.Id,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                var vendorProfileDto = _mapper.Map<VendorProfileDTO>(vendorProfile);

                return Ok(new SuccessResponse<VendorProfileDTO>
                {
                    success = true,
                    message = "Vendor Registration Completed successfully",
                    data = vendorProfileDto
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }




        /// <summary>
        /// Endpoint to summarize vendors data
        /// </summary>
        /// <returns></returns>
        [HttpGet("summary", Name = "SummarizeVendorsData")]
        [ProducesResponseType(typeof(SuccessResponse<VendorSummaryDto>), 200)]
        public async Task<IActionResult> SummarizeVendorsData()
        {
            try
            {
                //get summarised details
                var vendorSummary = await _vendorProfileRepository.GetVendorSummaryDetails();

                return Ok(new SuccessResponse<VendorSummaryDto>
                {
                    success = true,
                    message = "Vendors summary retrieved successfully",
                    data = vendorSummary
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// Endpoint to get all vendors in the organization
        /// Status: 1 = Pending, 2 = Approved, 3 = Rejected, 4 = Blacklisted
        /// </summary>
        /// <returns></returns>
        [HttpGet("", Name = "GetAllVendors")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<UserVendorDTO>>), 200)]
        public async Task<IActionResult> GetAllVendors([FromQuery] GetAllVendorParameters parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                //confirm if user is a exists and is a staff
                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId && u.UserType == EUserType.STAFF);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {userClaims.UserId} does not exist or is not a Staff.",
                        errors = new { }
                    });
                }

                // get the users with type of vendors
                var vendorUsers = await _vendorProfileRepository.GetAllVendors(userClaims.UserId, parameters);

                // convert vendor users to user vendor dto
                var vendorUsersDto = _mapper.Map<IEnumerable<UserVendorDTO>>(vendorUsers);

                // pagination variables
                var prevLink = vendorUsers.HasPrevious
                    ? CreateResourceUri(parameters, "GetAllVendors", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = vendorUsers.HasNext ? CreateResourceUri(parameters, "GetAllVendors", ResourceUriType.NextPage) : null;
                var currentLink = CreateResourceUri(parameters, "GetAllVendors", ResourceUriType.CurrentPage);

                // create pagination items
                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = vendorUsers.TotalPages,
                    perPage = vendorUsers.PageSize,
                    totalEntries = vendorUsers.TotalCount
                };

                // return success response
                return Ok(new PagedResponse<IEnumerable<UserVendorDTO>>
                {
                    success = true,
                    message = "Vendors retrieved successfully",
                    data = vendorUsersDto,
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
        /// Endpoint to allow a vendor accept a contract
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AcceptContract/{contractId}")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProjectsDTO>>), 200)]
        public async Task<IActionResult> AcceptContract(Guid contractId)
        {
            try
            {
                //Security checks
                var userClaims = User.UserClaims();

                var vendor = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId && u.UserType == EUserType.VENDOR);

                if (vendor == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {userClaims.UserId} does not exist or is not a Vendor.",
                        errors = new { }
                    });
                }

                var contract = await _contractRepository.SingleOrDefault(c => c.Id == contractId);

                if (contract == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Contract with id {contractId} not found",
                        errors = new { }
                    });
                }

                var projectExists = await _projectRepository.SingleOrDefault(p => p.ContractId == contractId);

                if (projectExists != null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Contract has already been accepted by another vendor",
                        errors = new { }
                    });
                }

                //Change contract status
                contract.Status = EContractStatus.ACCEPTED;

                //Generate projectCode
                string projectCodePrefix = "PR-";

                var projectCount = await _projectRepository.GetTotalCountOfProjects() + 1;

                var projectCode = projectCodePrefix + projectCount;


                //Create a new project based on contract
                var project = new Project
                {
                    Code = projectCode,
                    Contract = contract,
                    ContractId = contract.Id,
                    Title = contract.Title,
                    VendorId = vendor.Id,
                    StartDate = contract.StartDate,
                    EndDate = contract.EndDate,
                    Status = EProjectStatus.ACTIVE,
                    EstimatedValue = contract.EstimatedValue,
                    CreatedBy = contract.User,
                    CreatedById = contract.UserId,
                    CreateAt = DateTime.UtcNow,
                    Description = contract.Description
                };

                await _projectRepository.AddAsync(project);

                var userActivity = new UserActivity
                {
                    ObjectClass = "Project",
                    ObjectId = vendor.Id,
                    EventType = "ACCEPT A CONTRACT AND CREATE A PROJECT",
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS),
                    UserId = userClaims.UserId
                };

                await _userActivityRepository.AddAsync(userActivity);

                await _projectRepository.SaveChangesAsync();

                //Add notifications here
                await _notificationRepository.LogAcceptanceLetterResponseNotification(contractId, EAcceptanceLetterResponse.Accept);

                //update procurement plan activity
                var procurementPlanId = contract.ProcurementPlanId;

                var procurementPlanActivity = await _procurementPlanActivityRepository.SingleOrDefault(pp => pp.ProcurementPlanId == procurementPlanId
                && pp.ProcurementPlanType == EPprocurementPlanTask.PROCUREMENTEXECUTION
                && pp.Index == 1);

                if (procurementPlanActivity != null)
                {
                    procurementPlanActivity.ProcurementPlanActivityStatus = EProcurementPlanActivityStatus.PENDING;
                    _procurementPlanActivityRepository.Update(procurementPlanActivity);
                }

                //Map and return the newly created project
                var projectDTO = _mapper.Map<ProjectsDTO>(project);

                return Ok(new SuccessResponse<ProjectsDTO>
                {
                    success = true,
                    message = "Contract accepted successfully and project created successfully",
                    data = projectDTO
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }




        /// <summary>
        /// POST endpoint for vendor to reject a bid
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("rejectContract/{contractId}")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> RejectContract(Guid contractId)
        {
            try
            {
                var userClaims = User.UserClaims();

                var contract = await _contractRepository.GetByIdAsync(contractId);
                if (contract == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"contract with id {contractId} not found",
                        errors = new { }
                    });
                }

                contract.Status = EContractStatus.REJECTED;
                _contractRepository.Update(contract);
                var activities = await _procurementPlanRepository.GetProcurementPlanActivities(contract.ProcurementPlanId);

                int count = activities.Count();
                ProcurementPlanActivity pendingActivity;
                int index = 1;
                int bidIndex = 1;

                foreach (var activity in activities)
                {
                    if (activity.ProcurementPlanActivityStatus == EProcurementPlanActivityStatus.PENDING)
                    {
                        pendingActivity = activity;
                        index = activity.Index;
                    }
                }
                foreach (var activity in activities)
                {
                    if (activity.Title.ToLower() == "bid evaluation report and recommendation for awards")
                    {
                        activity.ProcurementPlanActivityStatus = EProcurementPlanActivityStatus.PENDING;
                        bidIndex = activity.Index;
                    }
                    if (activity.Index > bidIndex)
                    {
                        activity.ProcurementPlanActivityStatus = EProcurementPlanActivityStatus.INACTIVE;
                        var documents = await _procurementPlanRepository.GetProcurementPlanDocument(activity.Id);
                        foreach (var document in documents)
                        {
                            document.Deleted = true;
                        }
                    }
                }
                await _contractRepository.SaveChangesAsync();

                // Implement Notification 
                await _notificationRepository.LogAcceptanceLetterResponseNotification(contractId, EAcceptanceLetterResponse.Reject);


                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = $"Contract Rejected Successfully",
                    data = new { }
                });


            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// AN endpoint to Update/Track Vendor Registration Stages 
        /// </summary>
        /// <param name="vendorRegStage"></param>
        /// <returns></returns>
        [HttpPut("UpdateVendorRegistrationStage/{vendorRegStage}", Name = "UpdateVendorRegistrationStage")]
        [ProducesResponseType(typeof(SuccessResponse<UserDTO>), 200)]
        public async Task<IActionResult> UpdateVendorRegistrationStage(int vendorRegStage)
        {
            try
            {
                //Get userClaims from token
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId && u.UserType == EUserType.VENDOR);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {userClaims.UserId} not found",
                        errors = new { }
                    });
                }

                user.VendorRegStage = vendorRegStage;

                _userRepository.Update(user);
                await _vendorProfileRepository.SaveChangesAsync();

                var userDto = _mapper.Map<UserDTO>(user);

                return Ok(new SuccessResponse<UserDTO>
                {
                    success = true,
                    message = "Vendor Registration Stage updated successfully",
                    data = userDto
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// POST endpoint to upload a vendor contract signing document
        /// </summary>
        /// <param name="vendorContractUploadDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("vendorContractSigningDocument")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProcurementPlanDocumentDTO>>), 200)]
        public async Task<IActionResult> UploadVendorContractSigningDocument([FromForm] VendorContractUploadDTO vendorContractUploadDTO)
        {
            try
            {
                var userClaims = User.UserClaims();

                //get procurement plan
                var procurementPlan = await _procurementPlanRepository.GetProcurementPlan(vendorContractUploadDTO.ProcurementPlanId);
                if (procurementPlan == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {vendorContractUploadDTO.ProcurementPlanId} not found",
                        errors = new { }
                    });
                }

                if (procurementPlan.ProcurementPlanActivities.Count() == 0)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {vendorContractUploadDTO.ProcurementPlanId} doesn't contain procurement activities",
                        errors = new { }
                    });
                }

                var contract = await _contractRepository.SingleOrDefault(x => x.ProcurementPlanId == procurementPlan.Id && x.Status == EContractStatus.ACCEPTED);
                if (contract == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"procurement Plan with id {vendorContractUploadDTO.ProcurementPlanId} doesn't have a contract",
                        errors = new { }
                    });
                }

                // upload document 
                var documentDto = new GenericProcurementPlanDocumentDto()
                {
                    UserId = userClaims.UserId,
                    Documents = vendorContractUploadDTO.Documents,
                    ObjectId = vendorContractUploadDTO.ProcurementPlanActivityId,
                    Status = EDocumentStatus.OTHER,
                    ObjectType = EDocumentObjectType.VENDOR
                };

                var procurementPlanDocuments = await _procurementService.CreateGenericDocument(documentDto);
                await _procurementPlanRepository.AddProcurementPlanDocument(procurementPlanDocuments);
                //get the last approved procurement activity and then get the activity after that and change the status to pending
                var lastApprovedActivity = procurementPlan.ProcurementPlanActivities.LastOrDefault(x => x.ProcurementPlanActivityStatus == EProcurementPlanActivityStatus.APPROVED);
                var nextActivity = procurementPlan.ProcurementPlanActivities.SingleOrDefault(x => x.Index == lastApprovedActivity.Index + 1 && x.ProcurementPlanType == EPprocurementPlanTask.PROCUREMENTEXECUTION);

                if (nextActivity != null)
                {
                    nextActivity.ProcurementPlanActivityStatus = EProcurementPlanActivityStatus.PENDING;
                    _procurementPlanActivityRepository.Update(nextActivity);
                }

                contract.SignatureStatus = ESignatureStatus.SIGNED;
                _contractRepository.Update(contract);

                await _procurementPlanRepository.SaveChangesAsync();

                var userActivity = new UserActivity
                {
                    ObjectClass = "VENDOR UPLOADS CONTRACT SIGNING DOCUMENT",
                    EventType = "Vendor uploads contract signing document",
                    UserId = userClaims.UserId,
                    ObjectId = userClaims.UserId
                };

                await _userActivityRepository.AddAsync(userActivity);

                await _userActivityRepository.SaveChangesAsync();
                var procurementPlanDocumentDto = _mapper.Map<IEnumerable<ProcurementPlanDocumentDTO>>(procurementPlanDocuments);

                await _notificationRepository.LogContractSigningNotice(procurementPlan.Id);

                return Ok(new SuccessResponse<IEnumerable<ProcurementPlanDocumentDTO>>
                {
                    success = true,
                    message = $"vendor contract signing document uploaded Successfully",
                    data = procurementPlanDocumentDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An endpoint to get all the activities for a particular vendor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId}/Activities")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<UserActivityDTO>>>), 200)]
        public async Task<IActionResult> VendorActivities(Guid userId, [FromQuery]ResourceParameters parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId && u.UserType == Domain.Enums.EUserType.STAFF);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"user with {userClaims.UserId} not found or is not a staff",
                        errors = new { }
                    });
                }

                var vendor = await _userRepository.SingleOrDefault(u => u.Id == userId);


                var vendorActivities = await _userActivityRepository.GetVendorActivities(userId, parameters);

                //map vendorActivities to UserActivitiesDTO
                var vendorActivitiesDto = _mapper.Map<IEnumerable<UserActivityDTO>>(vendorActivities);

                var prevLink = vendorActivities.HasPrevious
                    ? CreateResourceUri(parameters, "GetVendorProjects", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = vendorActivities.HasNext
                    ? CreateResourceUri(parameters, "GetVendorProjects", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "GetVendorProjects", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = vendorActivities.TotalPages,
                    perPage = vendorActivities.PageSize,
                    totalEntries = vendorActivities.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<UserActivityDTO>>
                {
                    success = true,
                    message = "Activities retrieved successfully",
                    data = vendorActivitiesDto,
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
        /// An endpoint to search and get all vendors in a ministry
        /// </summary>
        /// <param name="parameters"></param>
        [HttpGet("search", Name = nameof(SearchVendors))]
        [ProducesResponseType(typeof(PagedResponse<IEnumerable<VendorProfile>>), 200)]
        public async Task<IActionResult> SearchVendors([FromQuery] VendorParameters parameters)
        {
            try
            {

                var vendorProfiles = await _vendorProfileRepository.SearchVendor(parameters);

                var prevLink = vendorProfiles.HasPrevious
                    ? CreateResourceUri(parameters, nameof(SearchVendors), ResourceUriType.PreviousPage)
                    : null;
                var nextLink = vendorProfiles.HasNext
                    ? CreateResourceUri(parameters, nameof(SearchVendors), ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, nameof(SearchVendors), ResourceUriType.CurrentPage);



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
        /// Endpoint to initiate vendor registration payment
        /// </summary>
        /// <returns></returns>
        [HttpGet("initiatePayment", Name = "InitiatePayment")]
        [ProducesResponseType(typeof(SuccessResponse<InitiatePaymentDTO>), 200)]
        public async Task<IActionResult> InitiatePayment([FromQuery] InitiatePaymentParameter parameter)
        {
            try
            {
                //Get userClaims from token
                var userClaims = User.UserClaims();
                var publickey = Configuration["XPRESSPAY_PUBLIC_KEY"];

                // check if vendor exists and return error message is user does not exist
                var vendorExists = await _vendorProfileRepository.FirstOrDefault(x => x.UserId == userClaims.UserId);

                if (vendorExists == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor profile with user id {userClaims.UserId} not found",
                        errors = new { }
                    });
                }

                // get the payment details
                var paymentDetails = await _vendorProfileRepository.InitiatePayment(vendorExists.Id, userClaims.UserId, publickey, parameter.CallbackUrl);
                paymentDetails.LogoUrl = Configuration["PAYMENT_LOGO_URL"];

                if (paymentDetails == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Failed to create payment details due to failure to update transaction Id",
                        errors = new { }
                    });
                }

                // return success response
                return Ok(new SuccessResponse<InitiatePaymentDTO>
                {
                    success = true,
                    message = "Payment details retrieved successfully",
                    data = paymentDetails,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to update vendor registration payment
        /// Payment Status: 1 = PAID, 2 = PENDING
        /// </summary>
        /// <returns></returns>
        [HttpPost("updatePayment", Name = "UpdatePayment")]
        [ProducesResponseType(typeof(SuccessResponse<UpdatePaymentDTO>), 200)]
        public async Task<IActionResult> UpdatePayment([FromBody] UpdateVendorRegPayStatusCreationDTO payment)
        {
            try
            {
                //Get userClaims from token
                var userClaims = User.UserClaims();
                var publickey = Configuration["XPRESSPAY_PUBLIC_KEY"];
                var requeryUrl = Configuration["XPRESSPAY_REQUERY"];

                // check if vendor exists and return error message is user does not exist
                var vendor = await _vendorProfileRepository.FirstOrDefault(x => x.UserId == userClaims.UserId);
                var user = await _userRepository.FirstOrDefault(u => u.Id == userClaims.UserId);

                if (user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"User with id {userClaims.UserId} not found",
                        errors = new { }
                    });
                }

                if (vendor == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Vendor profile with user id {userClaims.UserId} not found",
                        errors = new { }
                    });
                }

                //verify payment details from Xpresspay
                VerifyPaymentDTO paymentDetails = new VerifyPaymentDTO();
                paymentDetails = await _vendorProfileRepository.VerifyPayment(vendor, publickey, requeryUrl, payment.TransactionId);

                if (paymentDetails == null || paymentDetails.data == null || paymentDetails.data.payment == null || paymentDetails.data.payment.paymentResponseCode == null)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Payment verification with Xpresspay failed. Retry transaction",
                        errors = new { }
                    });
                }

                UpdatePaymentDTO detailsToReturn = null;
                if (paymentDetails.data.payment.paymentResponseCode.Equals("000") || paymentDetails.data.payment.paymentResponseMessage.ToLower().Equals("successful"))
                {
                    // update the payment details
                    detailsToReturn = await _vendorProfileRepository.UpdatePayment(vendor, user, payment.TransactionId);

                    if (detailsToReturn == null)
                    {
                        return BadRequest(new ErrorResponse<object>
                        {
                            success = false,
                            message = $"Failed to update payment status.",
                            errors = new { }
                        });
                    }
                }

                // return success response
                return Ok(new SuccessResponse<UpdatePaymentDTO>
                {
                    success = true,
                    message = "Payment details retrieved successfully",
                    data = detailsToReturn,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to verify vendor registration payment
        /// </summary>
        /// <returns></returns>
        //[HttpGet("verifyPayment", Name = "VerifyPayment")]
        //[ProducesResponseType(typeof(SuccessResponse<VerifyPaymentDTO>), 200)]
        //public async Task<IActionResult> VerifyPayment()
        //{
        //    try
        //    {
        //        //Get userClaims from token
        //        var userClaims = User.UserClaims();
        //        var publickey = Configuration["XPRESSPAY_PUBLIC_KEY"];
        //        var requeryUrl = Configuration["XPRESSPAY_REQUERY"];

        //        // check if vendor exists and return error message is user does not exist
        //        var vendor = await _vendorProfileRepository.FirstOrDefault(x => x.UserId == userClaims.UserId);
        //        var user = await _userRepository.FirstOrDefault(u => u.Id == userClaims.UserId);

        //        if (user == null)
        //        {
        //            return NotFound(new ErrorResponse<object>
        //            {
        //                success = false,
        //                message = $"User with id {userClaims.UserId} not found",
        //                errors = new { }
        //            });
        //        }

        //        if (vendor == null)
        //        {
        //            return NotFound(new ErrorResponse<object>
        //            {
        //                success = false,
        //                message = $"Vendor profile with user id {userClaims.UserId} not found",
        //                errors = new { }
        //            });
        //        }

        //        // call Xpresspay to verify the payment details
        //        var paymentDetails = await _vendorProfileRepository.VerifyPayment(vendor, publickey, requeryUrl);

        //        if (paymentDetails == null)
        //        {
        //            return BadRequest(new ErrorResponse<object>
        //            {
        //                success = false,
        //                message = $"Failed to retrieve payment status.",
        //                errors = new { }
        //            });
        //        }

        //        // return success response
        //        return Ok(new SuccessResponse<VerifyPaymentDTO>
        //        {
        //            success = true,
        //            message = "Payment details retrieved successfully",
        //            data = paymentDetails,
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, Response<string>.InternalError(ex.Message));
        //    }
        //}

        #region CreateResource
        private string CreateResourceUri(ResourceParameters parameters, string routeName, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(routeName,
                        new
                        {
                            page = parameters.PageNumber - 1
                        });

                case ResourceUriType.NextPage:
                    return Url.Link(routeName,
                        new
                        {
                            page = parameters.PageNumber + 1,
                        });

                default:
                    return Url.Link(routeName,
                        new
                        {
                            parameters.PageNumber,
                        });
            }

        }
        #endregion


    }
}

