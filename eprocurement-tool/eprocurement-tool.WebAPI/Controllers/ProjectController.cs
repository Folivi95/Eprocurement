using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet.Actions;
using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EGPS.WebAPI.Controllers
{
    /// <summary>
    /// Projects controller
    /// </summary>
    [Route("api/v1/projects")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectMileStoneRepository _projectMileStoneRepository;
        private readonly IMileStoneTaskRepository _mileStoneTaskRepository;
        private readonly IMinistryRepository _ministryRepository;
        private const string USER_IP_ADDRESS = "User-IP-Address";

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="userActivityRepository"></param>
        /// <param name="projectRepository"></param>
        /// <param name="projectMileStoneRepository"></param>
        /// <param name="mileStoneTaskRepository"></param>
        /// <param name="ministryRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="userRepository"></param>
        public ProjectController(IMapper mapper,
            IUserRepository userRepository,
            IUserActivityRepository userActivityRepository,
            IProjectRepository projectRepository,
            IProjectMileStoneRepository projectMileStoneRepository,
            IMileStoneTaskRepository mileStoneTaskRepository,
            IMinistryRepository ministryRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _projectMileStoneRepository = projectMileStoneRepository ?? throw new ArgumentNullException(nameof(projectMileStoneRepository));
            _mileStoneTaskRepository = mileStoneTaskRepository ?? throw new ArgumentNullException(nameof(mileStoneTaskRepository));
            _ministryRepository = ministryRepository ?? throw new ArgumentNullException(nameof(ministryRepository));
        }

        /// <summary>
        /// Endpoint to summarize projects
        /// </summary>
        /// <returns></returns>
        [HttpGet("summary", Name = "SummarizeProjects")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProjectsSummaryDTO>>), 200)]
        public async Task<IActionResult> SummarizeProjects()
        {
            try
            {
                var userClaims = User.UserClaims();

                //get summarised details
                var projectSummary = await _projectRepository.GetProjectsSummary();

                return Ok(new SuccessResponse<ProjectsSummaryDTO>
                {
                    success = true,
                    message = "Projects summary retrieved successfully",
                    data = projectSummary
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// Endpoint to summarize projects
        /// </summary>
        /// <returns></returns>
        [HttpGet("summary/vendor/{vendorId}", Name = "SummarizeProjectsForVendor")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProjectsSummaryDTO>>), 200)]
        public async Task<IActionResult> SummarizeProjectsByVendor([FromRoute]Guid vendorId)
        {
            try
            {
                var userClaims = User.UserClaims();

                //get summarised details
                var projectSummary = await _projectRepository.GetProjectsSummaryForVendor(vendorId);

                return Ok(new SuccessResponse<ProjectsSummaryDTO>
                {
                    success = true,
                    message = "Projects summary retrieved successfully",
                    data = projectSummary
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpGet]
        [Route("mileStone/{mileStoneId}/tasks/{id}/completed")]
        [ProducesResponseType(typeof(SuccessResponse<SuccessResponse<MilestoneTaskDTO>>), 200)]
        public async Task<IActionResult> MarkTaskAsCompletedAsync([FromRoute] Guid id, [FromRoute]Guid mileStoneId)
        {
            try
            {
                var milestone = await _projectMileStoneRepository.GetByIdAsync(mileStoneId);

                if(milestone == null)
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Milestone with id {id} not found",
                        errors = new { }
                    });


                var mileStoneTask = await _mileStoneTaskRepository.GetByIdAsync(id);

                if(mileStoneTask == null)
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Task with id {id} not found",
                        errors = new { }
                    });

                mileStoneTask.Status = Domain.Enums.EMilestoneTaskStatus.DONE;
                _mileStoneTaskRepository.Update(mileStoneTask);

                var mileStoneTasks = await _mileStoneTaskRepository.FindAsync(x => x.MileStoneId == mileStoneId);
                bool allDone = true;

                foreach (var item in mileStoneTasks)
                {
                    if (item.Status == Domain.Enums.EMilestoneTaskStatus.DONE)
                        continue;
                    else
                        allDone = false;
                }

                if (allDone)
                {
                    milestone.Status = Domain.Enums.EMilestoneStatus.DONE;
                    _projectMileStoneRepository.Update(milestone);
                }

                var milestoneTaskDTO = _mapper.Map<MilestoneTaskDTO>(mileStoneTask);

                return Ok(new SuccessResponse<MilestoneTaskDTO>
                {
                    success = true,
                    message = "Operation completed successfully",
                    data = milestoneTaskDTO
                });




            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// Endpoint to get milestones associated with a project
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/mileStone")]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<ProjectMileStoneDTO>>), 200)]
        public async Task<IActionResult> GetMileStonesAsync([FromRoute] Guid id)
        {
            try
            {
                var project = await _projectRepository.GetProject(id);

                if(project == null)
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"project with id {id} not found",
                        errors = new { }
                    });


                var projectMileStoneDTO = _mapper.Map<IEnumerable<ProjectMileStoneDTO>>(project.ProjectMileStones);
                return Ok(new SuccessResponse<IEnumerable<ProjectMileStoneDTO>>
                {
                    success = true,
                    message = "Project milestone retrieved successfully",
                    data = projectMileStoneDTO
                });


            }
            catch (Exception ex)
            {

                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to update a project milestone
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="MileStoneId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}/mileStone/{mileStoneId}")]
        [ProducesResponseType(typeof(SuccessResponse<ProjectMileStoneDTO>), 200)]
        public async Task<IActionResult> EditMileStoneAsync([FromRoute]Guid id, [FromBody]UpdateProjectMileStone model, [FromRoute]Guid MileStoneId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your project milestone creation request failed",
                        errors = ModelState.Error()
                    });
                }

                var project = await _projectRepository.GetProject(id);
                if (project == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"project with id {id} not found",
                        errors = new { }
                    });
                }


                var mileStone = _projectRepository.GetProjectMileStoneById(MileStoneId);

                if (mileStone == null)
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Milestone with id {id} not found",
                        errors = new { }
                    });



                if (model.EstimatedValue.HasValue)
                {
                    //If the total cost of the task is greater the new estimate -> reject update
                    var taskCost = mileStone.MilestoneTasks.Where(x => x.Deleted == false).Sum(x => x.EstimatedValue);
                    if (model.EstimatedValue.Value < taskCost)
                        return BadRequest(new ErrorResponse<object> {
                        
                        success  = false,
                        message = $"New Milestone cost can't be lower than total tasks cost, adjust and try again",
                        errors =  new {}
                        
                        });

                    
                    if(model.EstimatedValue.Value > project.EstimatedValue)
                        return BadRequest(new ErrorResponse<object>
                        {

                            success = false,
                            message = $"MileStone Cost can not be greater than project's cost",
                            errors = new { }

                        });


                    var sum = project.ProjectMileStones.Where(x => x.Deleted == false).Sum(x => x.EstimatedValue) + model.EstimatedValue.Value;

                    if(sum > project.EstimatedValue)
                        return BadRequest(new ErrorResponse<object>
                        {

                            success = false,
                            message = $"MileStone Cost can not be greater than project's cost",
                            errors = new { }

                        });

                    mileStone.EstimatedValue = model.EstimatedValue.Value;

                }

                if (model.StartDate.HasValue)
                {
                    if (model.StartDate.Value < project.StartDate)
                        return BadRequest(new ErrorResponse<object>
                        {
                            success = false,
                            message = $"New start date can not be before the project start date",
                            errors = new { }

                        });

                    var first = mileStone.MilestoneTasks.OrderBy(x => x.StartDate).FirstOrDefault();

                   if(model.StartDate.Value > first.StartDate.Date)
                        return BadRequest(new ErrorResponse<object>
                        {
                            success = false,
                            message = $"New start date can not be before the first task's start date",
                            errors = new { }

                        });

                    mileStone.StartDate = model.StartDate.Value;

                }

                if (model.EndDate.HasValue)
                {

                    var last = mileStone.MilestoneTasks.OrderByDescending(x => x.EndDate.Date).FirstOrDefault();

                    if (last != null)
                    {
                        if (last.EndDate.Date > model.EndDate.Value.Date)
                            return BadRequest(new ErrorResponse<object>
                            {
                                success = false,
                                message = $"New end date can not be less than the last task's end date",
                                errors = new { }
                            });
                    }


                    if(model.EndDate.Value > project.EndDate)
                        return BadRequest(new ErrorResponse<object>
                        {
                            success = false,
                            message = $"New end date can not be after the project end date",
                            errors = new { }

                        });

                    mileStone.EndDate = model.EndDate.Value;
                }


                mileStone.Title = model.Title ?? mileStone.Title;
                mileStone.Description = model.Description ?? mileStone.Description;
                mileStone.UpdatedAt = DateTime.Now;

                _projectMileStoneRepository.Update(mileStone);
                await _projectMileStoneRepository.SaveChangesAsync();

                var projectMileStoneDTO = _mapper.Map<ProjectMileStoneDTO>(mileStone);
                return Ok(new SuccessResponse<ProjectMileStoneDTO>
                {
                    success = true,
                    message = "Project milestone created successfully",
                    data = projectMileStoneDTO
                });

            }
            catch (Exception ex)
            {

                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to delete a project milestone
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mileStoneId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}/mileStone/{mileStoneId}")]
        [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
        public async Task<IActionResult> DeleteMileStoneAsync([FromQuery]Guid id,  [FromQuery]Guid mileStoneId)
        {
            try
            {
                var mileStone = await _projectMileStoneRepository.FirstOrDefault(x => x.Id == mileStoneId);

                if (mileStone == null)
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Milestone with {mileStoneId} not found"
                    });

                mileStone.Deleted = true;
                _projectMileStoneRepository.Update(mileStone);
                await _projectMileStoneRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<string>
                {
                    success = true,
                    message = "Project milestone created successfully",
                    data = ""
                });


            }
            catch (Exception ex)
            {

                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to create a project milestone
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectMileStoneForCreation"></param>
        /// <returns></returns>
        [HttpPost("{id}/mileStone")]
        [ProducesResponseType(typeof(SuccessResponse<ProjectMileStoneDTO>), 200)]
        public async Task<IActionResult> CreateProjectMileStone(Guid id, ProjectMileStoneForCreationDTO projectMileStoneForCreation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your project milestone creation request failed",
                        errors  = ModelState.Error()
                    });
                }

                var project = await _projectRepository.GetProject(id);
                if (project == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"project with id {id} not found",
                        errors = new { }
                    });
                }

                var dateComparisonResult = DateTime.Compare(projectMileStoneForCreation.EndDate.Value, projectMileStoneForCreation.StartDate.Value);

                if (dateComparisonResult < 0)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"start date should be within the project duration",
                        errors = new { }
                    });
                }

                //check if milestone start date and end date is within procurement
                //if(projectMileStoneForCreation.StartDate.Value.Date < project.StartDate.Value.Date || projectMileStoneForCreation.StartDate.Value.Date > project.EndDate.Value.Date)
                //{
                //    return BadRequest(new ErrorResponse<object>
                //    {
                //        success = false,
                //        message = $"start date should be within the project duration",
                //        errors = new { }
                //    });
                //}

                if(projectMileStoneForCreation.EndDate.Value.Date > project.EndDate.Value.Date || projectMileStoneForCreation.EndDate.Value.Date < project.StartDate.Value.Date)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"End date should be within the project duration",
                        errors = new { }
                    });
                }

                var userClaims = User.UserClaims();

                var user = await _userRepository.GetByIdAsync(userClaims.UserId);

                if (projectMileStoneForCreation.EstimatedValue > 0)
                {
                    
                    if(projectMileStoneForCreation.EstimatedValue > project.EstimatedValue)
                        return BadRequest(new ErrorResponse<object>
                        {

                            success = false,
                            message = $"MileStone Cost can not be greater than project's cost",
                            errors = new { }

                        });


                    var sum = project.ProjectMileStones.Sum(x => x.EstimatedValue) + projectMileStoneForCreation.EstimatedValue;

                    if(sum > project.EstimatedValue)
                        return BadRequest(new ErrorResponse<object>
                        {

                            success = false,
                            message = $"MileStone Cost can not be greater than project's cost",
                            errors = new { }

                        });
                }

                var projectMileStone = _mapper.Map<ProjectMileStone>(projectMileStoneForCreation);
                projectMileStone.ProjectId = id;
                projectMileStone.CreatedById = user.Id;

                await _projectRepository.AddProjectMileStone(projectMileStone);
                await _projectRepository.SaveChangesAsync();

                var projectMileStoneDTO = _mapper.Map<ProjectMileStoneDTO>(projectMileStone);

                return Ok(new SuccessResponse<ProjectMileStoneDTO>
                {
                    success = true,
                    message = "Project milestone created successfully",
                    data    = projectMileStoneDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }
        
        /// <summary>
        /// An endpoint to filter and retrieve all Projects
        /// 1 = Active, 2 = Inactive, 3 = Completed
        /// </summary>
        /// <returns></returns>
        [HttpGet("", Name = "GetProjects")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<ProjectsDTO>>>), 200)]
        public async Task<IActionResult> GetProjects([FromQuery] ProjectParameters parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                //get list of all projects
                var projects = await _projectRepository.GetProjects(parameters, userClaims);


                //map projects to projects dto
                var projectsDto = _mapper.Map<IEnumerable<ProjectsDTO>>(projects);

                if (projectsDto == null || projectsDto.FirstOrDefault() == null)
                {
                    projectsDto = Enumerable.Empty<ProjectsDTO>();
                }

                var prevLink = projects.HasPrevious
                    ? CreateProjectResourceUri(parameters, "GetProjects", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = projects.HasNext
                    ? CreateProjectResourceUri(parameters, "GetProjects", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateProjectResourceUri(parameters, "GetProjects", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = projects.TotalPages,
                    perPage = projects.PageSize,
                    totalEntries = projects.TotalCount
                };
                 
                return Ok(new PagedResponse<IEnumerable<ProjectsDTO>>
                {
                    success = true,
                    message = "Projects retrieved successfully",
                    data = projectsDto,
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
        /// An endpoint to get a project by contractId
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("contract/{contractId}")]
        [ProducesResponseType(typeof(SuccessResponse<ProjectDetailsDTO>), 200)]
        public async Task<IActionResult> GetProjectByContractId(Guid contractId)
         {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your retrieve project request failed",
                        errors  = ModelState.Error()
                    });
                }

                var userClaims = User.UserClaims();

                var user = await _userRepository.GetByIdAsync(userClaims.UserId);

                var project = await _projectRepository.GetProjectByContractId(contractId);
                    
                var projectDTO = _mapper.Map<ProjectDetailsDTO>(project);

                return Ok(new SuccessResponse<ProjectDetailsDTO>
                {
                    success = true,
                    message = "Project retrieved successfully",
                    data    = projectDTO
                 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to fetch a milestone for a project
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="milestoneId"></param>
        /// <returns></returns>
        [HttpGet("{projectId}/mileStone/{milestoneId}", Name = "GetMileStoneForProject")]
        [ProducesResponseType(typeof(SuccessResponse<ProjectMileStoneDTO>), 200)]
        public async Task<IActionResult> GetMileStoneForProject(Guid projectId, Guid milestoneId)
        {
            try
            {
                var userClaims = User.UserClaims();

                //check if project exists and return not found if it doesn't exists
                var project = await _projectRepository.FirstOrDefault(x => x.Id == projectId);
                var ministry = await _ministryRepository.FirstOrDefault(m => m.Id == project.MinistryId);

                if (project == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Project not found",
                        errors = new { }
                    });
                }

                var mileStone = await _projectMileStoneRepository.FirstOrDefault(x => x.Id == milestoneId);

                mileStone.MilestoneTasks = (ICollection<MilestoneTask>) await _mileStoneTaskRepository.GetMileStoneTasks(milestoneId);
                

                //map milestone to milestones dto
                var milestoneDto = _mapper.Map<ProjectMileStoneDTO>(mileStone);
                milestoneDto.Project.Ministry = _mapper.Map<MinistryDTO>(ministry);
                milestoneDto.MilestoneTasks = _mapper.Map<IEnumerable<MilestoneTaskDTO>>(mileStone.MilestoneTasks);

                milestoneDto.PercentageCompleted =
                    await _projectMileStoneRepository.GetPercentageComplete(milestoneDto.Id);

                return Ok(new SuccessResponse<ProjectMileStoneDTO>
                {
                    success = true,
                    message = "Project milestone retrieved successfully",
                    data = milestoneDto,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to Delete a MileStone Task
        /// </summary>
        /// <param name="milestoneId"></param>
        /// <param name="mileStoneTaskId"></param>
        /// <returns></returns>
        [HttpDelete("{milestoneId}/mileStoneTask/{mileStoneTaskId}", Name = "DeleteMileStoneTask")]
        [ProducesResponseType(typeof(SuccessResponse<object>), 200)]
        public async Task<IActionResult> DeleteMileStoneTask(Guid milestoneId, Guid mileStoneTaskId)
        {
            try
            {
                var userClaims = User.UserClaims();

                //check if milestone exists and return not found if it doesn't exists
                var milestone = await _projectMileStoneRepository.ExistsAsync(x => x.Id == milestoneId);

                if (!milestone)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Project milestone not found",
                        errors = new { }
                    });
                }

                var milestoneTask = await _mileStoneTaskRepository.FirstOrDefault(x => x.Id == mileStoneTaskId);

                //check if milestone tasks exists and return not found if it doesn't
                if (milestoneTask == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Milestone tasks not found",
                        errors = new { }
                    });
                }

                //soft delete milestone tasks
                milestoneTask.Deleted = true;
                milestoneTask.DeletedAt = DateTime.Now;

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Milestone task deleted successfully",
                    data = new { },
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }




        /// <summary>
        /// An endpoint to fetch a task for a milestone
        /// </summary>
        /// <param name="milestoneId"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("mileStone/{milestoneId}/mileStoneTasks", Name = nameof(GetMileStoneTask))]
        [ProducesResponseType(typeof(SuccessResponse<IEnumerable<MilestoneTaskDTO>>), 200)]
        public async Task<IActionResult> GetMileStoneTask(Guid milestoneId, [FromQuery] MileStoneTaskParameter parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                //check if project exists and return not found if it doesn't exists
                var findMileStone = await _projectMileStoneRepository.ExistsAsync(a => a.Id == milestoneId);

                if (!findMileStone)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Milestone not found",
                        errors = new { }
                    });
                }

                var tasks = await _mileStoneTaskRepository.GetMileStoneTasks(milestoneId, parameters);

                var tasksDTO = _mapper.Map<IEnumerable<MilestoneTaskDTO>>(tasks);



                var page = PageUtility<MilestoneTask>.CreateResourcePageUrl(parameters, nameof(GetMileStoneTask), tasks , Url);
                var response = new PagedResponse<IEnumerable<MilestoneTaskDTO>>
                {
                    success = true,
                    message = "Milestone tasks retrieved successfully",
                    data = tasksDTO,
                    meta = new Meta
                    {
                        pagination = page
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
        /// Endpoint to create milestone task
        /// </summary>
        /// <returns></returns>
        [HttpPost("{projectMileStoneId}/mileStoneTask", Name = "CreateMilestoneTask")]
        [ProducesResponseType(typeof(SuccessResponse<MilestoneTaskDTO>), 200)]
        public async Task<IActionResult> CreateMileStoneTask(Guid projectMileStoneId, [FromBody] MilestoneTaskForCreateDTO milestoneDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("projectMileStoneId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your mile task creation request failed",
                        errors = ModelState.Error()
                    });
                }

                if (!await _projectRepository.CheckIfMileStoneExists(projectMileStoneId))
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Project Milestone with id {projectMileStoneId} not found",
                        errors = new { }
                    });


                if (await _projectRepository.CheckIfMileStoneTitleExists(milestoneDTO.Title, projectMileStoneId))
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your mile task creation request failed",
                        errors = new
                        {
                            name = new string[] { "Duplicate Name" }
                        }
                    });

                var mileStone = await _projectMileStoneRepository.SingleOrDefault(x => x.Id == projectMileStoneId);

                if (mileStone == null)
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Milestone with id {projectMileStoneId} not found",
                        errors = new { }
                    });

                var taskEstimatedSumValue = _mileStoneTaskRepository.FindAsync(x => x.MileStoneId == projectMileStoneId)
                                            .Result.Sum(x => x.EstimatedValue) + milestoneDTO.EstimatedValue;

                if (taskEstimatedSumValue > mileStone.EstimatedValue)
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your mile task update request failed",
                        errors = new
                        {
                            name = new string[] { "The Sum of task estimated value cannot exceed the milestone estimated value" }
                        }
                    });

                var user = User.UserClaims();
                var mileStoneTask = _mapper.Map<MilestoneTask>(milestoneDTO);
                mileStoneTask.CreatedById = user.UserId;
                mileStoneTask.MileStoneId = projectMileStoneId;
                await _projectRepository.CreateMilestoneTask(mileStoneTask);

                var userActivity = new UserActivity
                {
                    AccountId = user.AccountId,
                    EventType = "Project MileStone Task Creation",
                    ObjectClass = "VENDOR",
                    ObjectId = user.UserId,
                    UserId = user.UserId,
                    CreatedAt = DateTime.Now,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<MilestoneTaskDTO>
                {
                    success = true,
                    message = "Project MileStone Task created successfully",
                    data = _mapper.Map<MilestoneTaskDTO>(mileStoneTask)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }




        /// <summary>
        /// An endpoint to get a milestone task by its ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("{mileStoneId}/mileStoneTasks/{Id}", Name = nameof(GetMileStoneTaskById))]
        [ProducesResponseType(typeof(SuccessResponse<MilestoneTaskDTO>), 200)]
        public async Task<IActionResult> GetMileStoneTaskById(Guid Id, Guid mileStoneId)
        {
            try
            {
                var userClaims = User.UserClaims();

                var mileStone = await _projectMileStoneRepository.SingleOrDefault(p => p.Id == mileStoneId);

                if (mileStone == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Milestone not found",
                        errors = new { }
                    });
                }


                //check if mileStoneTask exists and return not found if it doesn't exists
                var mileStoneTask = await _mileStoneTaskRepository.SingleOrDefault(a => a.Id == Id);

                if (mileStoneTask == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "MilestoneTask not found",
                        errors = new { }
                    });
                }

                //check if task exist in milestone
                var mileStoneTasks = await _projectMileStoneRepository.GetMilestoneTasks(mileStoneId);

                var mileStoneTaskExist = mileStoneTasks.Contains(mileStoneTask);

                if (!mileStoneTaskExist)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "MilestoneTask des not exist within this mileStone",
                        errors = new { }
                    });
                }


                var mileStoneTaskDto = _mapper.Map<MilestoneTaskDTO>(mileStoneTask);

                return Ok(new SuccessResponse<MilestoneTaskDTO>
                {
                    success = true,
                    message = "Mile Stone Task retrieved successfully",
                    data = mileStoneTaskDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }




        /// <summary>
        /// Endpoint to Update milestone task
        /// </summary>
        /// <returns></returns>
        [HttpPut("{mileStoneId}/mileStoneTask/{mileStoneTaskId}")]
        [ProducesResponseType(typeof(SuccessResponse<MilestoneTaskDTO>), 200)]
        public async Task<IActionResult> UpdateMileStoneTask(Guid mileStoneTaskId, Guid mileStoneId, [FromBody] MilestoneTaskForCreateDTO milestoneDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.Remove("mileStoneId");
                    ModelState.Remove("mileStoneTaskId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your mile task update request failed",
                        errors = ModelState.Error()
                    });
                }

                var mileStoneTask = await _mileStoneTaskRepository.SingleOrDefault(x => x.Id == mileStoneTaskId);

                if (mileStoneTask == null)
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Milestone task with id {mileStoneTaskId} not found",
                        errors = new { }
                    });

                if (await _mileStoneTaskRepository
                    .ExistsAsync(x => x.Id != mileStoneTaskId &&
                    x.Title.ToLower().Trim() == milestoneDTO.Title.ToLower().Trim()))
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your mile task update request failed",
                        errors = new
                        {
                            name = new string[] { "Duplicate Name" }
                        }
                    });

                var mileStone = await _projectMileStoneRepository.SingleOrDefault(x => x.Id == mileStoneId);

                if (mileStone == null)
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Milestone with id {mileStoneId} not found",
                        errors = new { }
                    });

                var taskEstimatedSumValue = _mileStoneTaskRepository.FindAsync(x => x.MileStoneId == mileStoneId)
                                            .Result.Sum(x => x.EstimatedValue) + milestoneDTO.EstimatedValue;

                if (taskEstimatedSumValue > mileStone.EstimatedValue)
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your mile task update request failed",
                        errors = new
                        {
                            name = new string[] { "The Sum of task estimated value cannot exceed the milestone estimated value" }
                        }
                    });

                _mapper.Map(milestoneDTO, mileStoneTask);

                var user = User.UserClaims();

                var userActivity = new UserActivity
                {
                    AccountId = user.AccountId,
                    EventType = "Project MileStone Task Update",
                    ObjectClass = "VENDOR",
                    ObjectId = user.UserId,
                    UserId = user.UserId,
                    CreatedAt = DateTime.Now,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<MilestoneTaskDTO>
                {
                    success = true,
                    message = "Project MileStone Task updated successfully",
                    data = _mapper.Map<MilestoneTaskDTO>(mileStoneTask)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// An endpoint to get milestone invoice
        /// 1 = Paid, 2 = Pending
        /// <param name="milestoneId"></param>
        /// </summary>
        /// <returns></returns>
        [HttpGet("{milestoneId}/milestoneInvoice", Name = "GetMilestoneInvoice")]
        [ProducesResponseType(typeof(SuccessResponse<MilestoneInvoiceDTO>), 200)]
        public async Task<IActionResult> GetMilestoneInvoice(Guid milestoneId)
        {
            try
            {
                var userClaims = User.UserClaims();

                //get milestone invoice
                var milestoneInvoice = await _projectMileStoneRepository.GetMilestoneInvoice(milestoneId);

                if (milestoneInvoice == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Project Milestone invoice not found",
                        errors = new { }
                    });
                }

                //map milestone invoice to milestone invoice dto
                var milestoneInvoiceDto = _mapper.Map<MilestoneInvoiceDTO>(milestoneInvoice);

                return Ok(new SuccessResponse<MilestoneInvoiceDTO>
                {
                    success = true,
                    message = "Milestone invoice retrieved successfully",
                    data = milestoneInvoiceDto,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to retrieve a project
        /// 1 = Active, 2 = Inactive, 3 = Completed
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("{projectId}", Name = "GetProject")]
        [ProducesResponseType(typeof(SuccessResponse<ProjectsDTO>), 200)]
        public async Task<IActionResult> GetProject(Guid projectId)
        {
            try
            {
                var userClaims = User.UserClaims();

                //check if project exists and return not found if it doesn't exists
                var project = await _projectRepository.FirstOrDefault(x => x.Id == projectId);
                project.Ministry = await _ministryRepository.FirstOrDefault(m => m.Id == project.MinistryId);

                if (project == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Project not found",
                        errors = new { }
                    });
                }

                //map project to projects dto
                var projectsDto = _mapper.Map<ProjectsDTO>(project);
                projectsDto.Ministry = _mapper.Map<MinistryDTO>(project.Ministry);
                projectsDto.ProjectMileStones = _mapper.Map<ICollection<ProjectMileStoneDTO>>(project.ProjectMileStones);
                projectsDto.PercentageCompleted = await _projectRepository.GetPercentageComplete(projectsDto.Id);
                return Ok(new SuccessResponse<ProjectsDTO>
                {
                    success = true,
                    message = "Project milestone retrieved successfully",
                    data = projectsDto,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to retrieve a milestone task by milestoneTaskId
        /// </summary>
        /// <param name="milestoneTaskId"></param>
        /// <returns></returns>
        [HttpGet("milestone/{milestoneTaskId}")]
        [ProducesResponseType(typeof(SuccessResponse<MilestoneTaskDTO>), 200)]
        public async Task<IActionResult> GetMilestoneTask(Guid milestoneTaskId)
        {
            try
            {
                var milestoneTask = await _mileStoneTaskRepository.SingleOrDefault(x => x.Id == milestoneTaskId);

                if (milestoneTask == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Milestone task with id={milestoneTaskId} not found",
                        errors = new { }
                    });
                }

                return Ok(new SuccessResponse<MilestoneTaskDTO>
                {
                    success = true,
                    message = "Mile Stone Task retrieved successfully",
                    data = _mapper.Map<MilestoneTaskDTO>(milestoneTask),
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        /// <summary>
        /// An endpoint to get all projects for a particular vendor
        /// </summary>
        /// <returns></returns>
        [HttpGet("{userId}/projects", Name = "GetVendorProjects")]
        [ProducesResponseType(typeof(SuccessResponse<PagedResponse<IEnumerable<ProjectsDTO>>>), 200)]
        public async Task<IActionResult> GetVendorProjects(Guid userId, [FromQuery]ResourceParameters parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                var user = await _userRepository.SingleOrDefault(u => u.Id == userClaims.UserId);

                if(user == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"user with {userClaims.UserId} not found or is not a staff",
                        errors = new { }
                    });
                }

                var vendorProjects = await _projectRepository.GetAllVendorProjects(userId, parameters);

                //map vendorProjects to projectsDto
                var vendorProjectsDto = _mapper.Map<IEnumerable<ProjectsDTO>>(vendorProjects);

                var prevLink = vendorProjects.HasPrevious
                    ? CreateResourceUri(parameters, "GetVendorProjects", ResourceUriType.PreviousPage)
                    : null;
                var nextLink = vendorProjects.HasNext
                    ? CreateResourceUri(parameters, "GetVendorProjects", ResourceUriType.NextPage)
                    : null;
                var currentLink = CreateResourceUri(parameters, "GetVendorProjects", ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = vendorProjects.TotalPages,
                    perPage = vendorProjects.PageSize,
                    totalEntries = vendorProjects.TotalCount
                };

                return Ok(new PagedResponse<IEnumerable<ProjectsDTO>>
                {
                    success = true,
                    message = "Projects retrieved successfully",
                    data = vendorProjectsDto,
                    meta = new Meta
                    {
                        pagination = pagination
                    }
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }


        /// <summary>
        /// Endpoint to create milestone invoice
        /// Document Status: 1 = MANDATORY, 2 = SUPPORTING, 3 = REVIEW, 4 = PAYMENT, 5 = OTHER
        /// <param name="milestoneId"></param>
        /// <param name="milestoneInvoice"></param>
        /// </summary>
        /// <returns></returns>
        [HttpPost("{milestoneId}/milestoneInvoice", Name = "CreateMilestoneInvoice")]
        [ProducesResponseType(typeof(SuccessResponse<MilestoneTaskDTO>), 200)]
        public async Task<IActionResult> CreateMilestoneInvoice(Guid milestoneId, [FromBody] MilestoneInvoiceForCreation milestoneInvoice)
        {
            try
            {
                var userClaims = User.UserClaims();

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
                        message = "Your milestone invoice request failed",
                        errors = errors
                    });
                }

                if (milestoneId == null || milestoneId.Equals(""))
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Please supply milestone Id",
                        errors = new { }
                    });
                }

                //check if due date is greater than or equal to today's date
                var dueDateComparison = DateTime.Compare(milestoneInvoice.DueDate, DateTime.Now);

                if (dueDateComparison <= 0)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Due date {milestoneInvoice.DueDate.Date:dd/MM/yyyy} should be greater than today's date",
                        errors = new { }
                    });
                }

                //check if milestone exists
                var milestone = await _projectMileStoneRepository.FirstOrDefault(b => b.Id == milestoneId);

                if (milestone == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Project milestone with id {milestoneId} not found",
                        errors = new { }
                    });
                }

                //call repository to create milestone invoice
                var milestoneCreated = await _projectMileStoneRepository.CreateMilestoneInvoice(milestone, milestoneInvoice);

                if (!milestoneCreated)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Milestone invoice not created",
                        errors = new { }
                    });
                }

                var userActivity = new UserActivity
                {
                    AccountId = userClaims.AccountId,
                    EventType = "Milestone Invoice Creation",
                    ObjectClass = "VENDOR",
                    ObjectId = userClaims.UserId,
                    UserId = userClaims.UserId,
                    CreatedAt = DateTime.Now,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                //get milestone created to add to the response
                var invoiceResponse = await _projectMileStoneRepository.GetMilestoneInvoice(milestoneId);

                return Ok(new SuccessResponse<MilestoneInvoiceDTO>
                {
                    success = true,
                    message = "Milestone Invoice created successfully",
                    data = _mapper.Map<MilestoneInvoiceDTO>(invoiceResponse)
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

        #region CreateProjectResource
        private string CreateProjectResourceUri(ProjectParameters parameters, string name, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(name,
                        new
                        {
                            PageNumber = parameters.PageNumber - 1,
                            parameters.PageSize,
                            parameters.Category,
                            parameters.Title,
                            parameters.StartDate,
                            parameters.ExpiryDate
                        });

                case ResourceUriType.NextPage:
                    return Url.Link(name,
                        new
                        {
                            PageNumber = parameters.PageNumber + 1,
                            parameters.PageSize,
                            parameters.Category,
                            parameters.Title,
                            parameters.StartDate,
                            parameters.ExpiryDate
                        });

                default:
                    return Url.Link(name,
                        new
                        {
                            parameters.PageNumber,
                            parameters.PageSize,
                            parameters.Category,
                            parameters.Title,
                            parameters.StartDate,
                            parameters.ExpiryDate
                        });
            }

        }
        #endregion



    }
}
