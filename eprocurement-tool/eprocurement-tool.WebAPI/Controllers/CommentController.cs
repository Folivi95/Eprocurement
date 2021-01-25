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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EGPS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/v1/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private const string USER_IP_ADDRESS = "User-IP-Address";

        public CommentController(
            IMapper mapper, 
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IUserActivityRepository userActivityRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
        }

        /// <summary>
        /// An endpoint to create a comment or suggestion or compliant
        /// </summary>
        /// <param name="commentForCreation"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponse<CommentDTO>), 200)]
        public async Task<IActionResult> CreateComment(CommentForCreation commentForCreation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your comment creation request failed",
                        errors  = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();

                var user = await _userRepository.GetByIdAsync(userClaims.UserId);

                var comment = _mapper.Map<Comment>(commentForCreation);

                await _commentRepository.AddAsync(comment);
                await _commentRepository.SaveChangesAsync();

                var commentDto = _mapper.Map<CommentDTO>(comment);

                return Ok(new SuccessResponse<CommentDTO>
                {
                    success = true,
                    message = "Comment created successfully",
                    data    = commentDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<CommentDTO>), 200)]
        public async Task<IActionResult> GetCommentById(Guid id)
        {
            try
            {
                var comment = await _commentRepository.GetByIdAsync(id);

                if (comment == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Comment with id {id} not found",
                        errors = new { }
                    });
                }

                var commentDto = _mapper.Map<CommentDTO>(comment);

                return Ok(new SuccessResponse<CommentDTO>
                {
                    success = true,
                    message = "Comment retrieved successfully",
                    data = commentDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetComments")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<CommentDTO>>>), 200)]
        public async Task<IActionResult> GetComments([FromQuery]Commentparameters commentparameters)
        {
            try
            {
                var comments = await _commentRepository.GetComments(commentparameters);

                var prevLink = comments.HasPrevious
                    ? CreateResourceUri(commentparameters, ResourceUriType.PreviousPage)
                    : null;
                var nextLink = comments.HasNext
                    ? CreateResourceUri(commentparameters, ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(commentparameters, ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = comments.TotalPages,
                    perPage = comments.PageSize,
                    totalEntries = comments.TotalCount
                };

                var commentDto = _mapper.Map<IEnumerable<CommentDTO>>(comments);

                return Ok(new PagedResponse<IEnumerable<CommentDTO>>
                {
                    success = true,
                    message = "Comments retrieved successfully",
                    data = commentDto,
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
        private string CreateResourceUri(Commentparameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetComments",
                        new
                        {
                            PageNumber = parameters.PageNumber - 1,
                            parameters.PageSize,
                            parameters.Type,
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetComments",
                        new
                        {
                            PageNumber = parameters.PageNumber + 1,
                            parameters.PageSize,
                            parameters.Type
                        });

                default:
                    return Url.Link("GetComments",
                        new
                        {
                            parameters.PageNumber,
                            parameters.PageSize,
                            parameters.Type
                        });
            }

        }
        #endregion
    }
}
