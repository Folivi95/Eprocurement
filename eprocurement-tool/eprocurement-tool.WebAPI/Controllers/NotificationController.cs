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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;



namespace EGPS.WebAPI.Controllers
{
    /// <summary>
    /// Ministries controller
    /// </summary>
    [Route("api/v1/notifications")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IStaffRepository _staffRepository;
        private const string USER_IP_ADDRESS = "User-IP-Address";
        public IConfiguration Configuration { get; }

        public NotificationController(IMapper mapper,
            IJwtAuthenticationManager jwtAuthenticationManager,
            IConfiguration configuration,
            INotificationRepository notificationRepository,
            IStaffRepository staffRepository,
            IUserRepository userRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _jwtAuthenticationManager = jwtAuthenticationManager ?? throw new ArgumentNullException(nameof(jwtAuthenticationManager));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
           _notificationRepository = notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));
            _staffRepository = staffRepository ?? throw new ArgumentNullException(nameof(staffRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }



        [HttpPatch]
        [Route("read")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<Notification>>), 200)]
        public async Task<IActionResult> Mark([FromBody] List<Guid> id)
        {
            try
            {
                var result = await _notificationRepository.MarkAsRead(id);
                return Ok(new SuccessResponse<IEnumerable<Notification>>
                {
                    success = true,
                    message = "operation completed successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {

                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpGet(Name ="GetNotifications")]
        public async Task<IActionResult> GetNotificationsAsync([FromQuery]NotificationParameters parameter)
        {
            try
            {
                var currentUserId = User.UserClaims().UserId;

                var notification = await _notificationRepository.GetNotifications(currentUserId, parameter);

                var prevLink = notification.HasPrevious
                   ? CreateResourceUri(parameter, ResourceUriType.PreviousPage)
                   : null;
                var nextLink = notification.HasNext
                    ? CreateResourceUri(parameter, ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameter, ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = notification.TotalPages,
                    perPage = notification.PageSize,
                    totalEntries = notification.TotalCount
                };

                
                var notificationModel = _mapper.Map<NotificationModel[]>(notification);

                return Ok(new PagedResponse<IEnumerable<NotificationModel>>
                {
                    success = true,
                    message = "Notifications retrieved successfully",
                    data = notificationModel,
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
        private string CreateResourceUri(NotificationParameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetNotifications",
                        new
                        {
                            PageNumber = parameters.PageNumber - 1,
                            parameters.PageSize,
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetNotifications",
                        new
                        {
                            PageNumber = parameters.PageNumber + 1,
                            parameters.PageSize,
                            
                        });

                default:
                    return Url.Link("GetNotifications",
                        new
                        {
                            parameters.PageNumber,
                            parameters.PageSize,
                            
                        });
            }

        }
        #endregion
    }
}
