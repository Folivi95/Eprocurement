using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Application.Validators;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EGPS.WebAPI.Controllers
{
    [Authorize]
    [Route("api/v1/workflows")]
    [ApiController]
    public class WorkflowsController : ControllerBase
    {
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserActivityRepository _userActivityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private const string USER_IP_ADDRESS = "User-IP-Address";

        public WorkflowsController(IWorkflowRepository workflowRepository,
            IUnitRepository unitRepository,
            IMapper mapper,
            IHttpContextAccessor accessor,
            IUserActivityRepository userActivityRepository,
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository)
        {
            _workflowRepository = workflowRepository ?? throw new ArgumentNullException(nameof(workflowRepository));
            _unitRepository = unitRepository ?? throw new ArgumentNullException(nameof(unitRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            _userActivityRepository = userActivityRepository ?? throw new ArgumentNullException(nameof(userActivityRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        }

        [HttpPost("{id}/stages")]
        [ProducesResponseType(typeof(SuccessResponse<StageDTO>), 200)]
        public async Task<IActionResult> CreateStage(Guid id, StageForCreationDTO stageForCreation)
        {
            try
            {
                var userClaims = User.UserClaims();
                var workflow = await _workflowRepository.SingleOrDefault(w => w.Id == id && w.AccountId == userClaims.AccountId);

                var stageExists = await _workflowRepository.StageTitleExistInWorkflow(stageForCreation.Title, id);

                var indexExists = await _workflowRepository.IndexStageExistUnderWorkflow(stageForCreation.Index, id);

                if (workflow == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Workflow with id {id} not found",
                        errors = new { }
                    });
                }

                if (stageExists)
                {
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your stage creation request failed",
                        errors = new
                        {
                            title = new string[] { "Duplicate Title" }
                        }
                    });
                }

                if (!ModelState.IsValid)
                {
                    ModelState.Remove("workflowId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your stage creation request failed",
                        errors = ModelState.Error()
                    });
                }

                if (indexExists)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your stage creation request failed",
                        errors = new
                        {
                            Index = new string[] { "Index already exist" }
                        }
                    });
                }

                var (stageForCreationDto, errorDictionary) = await ValidateStageForCreation(stageForCreation, userClaims);

                if (errorDictionary.Values != null && errorDictionary.Values.Count > 0)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, List<string>>>
                    {
                        success = false,
                        message = "Your stage creation request failed",
                        errors = errorDictionary
                    });
                }

                var stage = _mapper.Map<Stage>(stageForCreationDto);
                stage.AccountId = userClaims.AccountId;
                stage.WorkFlowId = id;
                stage.CreatedById = userClaims.UserId;

                await _workflowRepository.AddStage(stage);
                await _workflowRepository.SaveChangesAsync();

                var stageDto = _mapper.Map<StageDTO>(stage);

                var userActivity = new UserActivity
                {
                    EventType = "Stage Created",
                    UserId = userClaims.UserId,
                    ObjectClass = "STAGE",
                    ObjectId = stage.Id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<StageDTO>
                {
                    success = true,
                    message = "Stage created successfully",
                    data = stageDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        private async Task<(StageForCreationDTO, Dictionary<string, List<string>>)> ValidateStageForCreation(StageForCreationDTO stageForCreation, UserClaims userClaims)
        {
            var errors = new List<string>();
            var errorDictionary = new Dictionary<string, List<string>>();
            var stage = new Stage();
            if (stageForCreation.UserType.ToUpper() == "GROUP")
            {
                int memberCount = 0;
                switch (stageForCreation.GroupClass.ToUpper())
                {
                    case "DEPARTMENT":
                        foreach (var departmentId in stageForCreation.GroupIds)
                        {
                            var department = await _departmentRepository.SingleOrDefault(d => d.Id == departmentId && d.AccountId == userClaims.AccountId);

                            if (department == null)
                            {
                                errors.Add($"Department with id {departmentId} not found");
                                errorDictionary.Add("groupsId", errors);
                                break;
                            }
                            else
                            {
                                var departmentMembersCount = await _departmentRepository.GetMembersCount(departmentId);

                                memberCount += departmentMembersCount;
                            }
                        }

                        if (stageForCreation.MinimumPass > memberCount && errors.Count <= 0)
                        {
                            errors.Add("Minimum pass can not be greater than the size of the group");
                            errorDictionary.Add("minimumPass", errors);
                        }

                        break;

                    case "UNIT":
                        foreach (var unitId in stageForCreation.GroupIds)
                        {
                            var unit = await _unitRepository.SingleOrDefault(d => d.Id == unitId && d.AccountId == userClaims.AccountId);

                            if (unit == null)
                            {
                                errors.Add($"Unit with id {unitId} not found");
                                errorDictionary.Add("groupsId", errors);
                                break;
                            }
                            else
                            {
                                var unitMembersCount = await _unitRepository.GetMembersCount(unitId);
                                memberCount += unitMembersCount;
                            }
                        }

                        if (stageForCreation.MinimumPass > memberCount && errors.Count <= 0)
                        {
                            errors.Add("Minimum pass can not be greater than the size of the group");
                            errorDictionary.Add("minimumPass", errors);
                        }

                        break;
                }

                return (stageForCreation, errorDictionary);
            }

            foreach (var userId in stageForCreation.AssigneeIds)
            {
                var user = await _userRepository.SingleOrDefault(d => d.Id == userId && d.AccountId == userClaims.AccountId);
                if (user == null)
                {
                    errors.Add($"User with id {userId} not found");
                    errorDictionary.Add("assigneeIds", errors);
                    break;
                }
            }

            if (stageForCreation.MinimumPass > stageForCreation.AssigneeIds.Length && errors.Count <= 0)
            {
                errors.Add("Minimum pass can not be greater than the size of the group");
                errorDictionary.Add("minimumPass", errors);
            }

            return (stageForCreation, errorDictionary);
        }

        [HttpGet("{id}/stages", Name = "GetStages")]
        [ProducesResponseType(typeof(SuccessResponse<StageDTO>), 200)]
        public async Task<IActionResult> GetStagesUnderWorkflow(Guid id, [FromQuery] StageParameter parameter)
        {
            try
            {
                var userClaims = User.UserClaims();
                var workflow = await _workflowRepository.SingleOrDefault(w => w.Id == id && w.AccountId == userClaims.AccountId);

                if (workflow == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Workflow with id {id} not found",
                        errors = new { }
                    });
                }

                var stages = await _workflowRepository.GetStagesUnderWorflow(id, parameter);

                var prevLink = stages.HasPrevious
                    ? CreateStageResource(parameter, ResourceUriType.PreviousPage)
                    : null;
                var nextLink = stages.HasNext ? CreateStageResource(parameter, ResourceUriType.NextPage) : null;
                var currentLink = CreateStageResource(parameter, ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = stages.TotalPages,
                    perPage = stages.PageSize,
                    totalEntries = stages.TotalCount
                };

                var stageDto = _mapper.Map<IEnumerable<StageDTO>>(stages);

                return Ok(new PagedResponse<IEnumerable<StageDTO>>
                {
                    success = true,
                    message = "Stages retrieved successfully",
                    data = stageDto,
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

        [HttpGet(Name = "GetWorkflows")]
        [ProducesResponseType(typeof(SuccessResponse<WorkflowsDTO>), 200)]
        public async Task<IActionResult> GetWorkFlows([FromQuery] WorkflowParameter parameters)
        {
            try
            {
                var userClaims = User.UserClaims();

                var workflows = await _workflowRepository.GetWorkflows(userClaims.AccountId, parameters);

                var prevLink = workflows.HasPrevious
                    ? CreateWorkflowResource(parameters, ResourceUriType.PreviousPage)
                    : null;

                var nextLink = workflows.HasNext ? CreateWorkflowResource(parameters, ResourceUriType.NextPage) : null;

                var currentLink = CreateWorkflowResource(parameters, ResourceUriType.CurrentPage);

                var pagination = new Pagination
                {
                    currentPage = currentLink,
                    nextPage = nextLink,
                    previousPage = prevLink,
                    totalPages = workflows.TotalPages,
                    perPage = workflows.PageSize,
                    totalEntries = workflows.TotalCount
                };

                var workflowsDTO = _mapper.Map<IEnumerable<WorkflowsDTO>>(workflows);

                return Ok(new PagedResponse<IEnumerable<WorkflowsDTO>>
                {
                    success = true,
                    message = "Workflows retrieved successfully",
                    data = workflowsDTO,
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkFlow(Guid id, WorkflowForUpdateDTO workflowForUpdate)
        {
            var userClaims = User.UserClaims();

            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Your workflow update request failed",
                    errors = new
                    {
                        stages = new Dictionary<string, string[]>[] { ModelState.Error() }
                    }
                });
            }

            var workflowTitleExist = await _workflowRepository.ExistsAsync(w => w.Title == workflowForUpdate.Title 
                && w.AccountId == userClaims.AccountId);

            if (workflowTitleExist)
            {
                return Conflict(new ErrorResponse<object>
                {
                    success = false,
                    message = "Your workflow update request failed",
                    errors = new
                    {
                        title = new string[] { "Duplicate Title" }
                    }
                });
            }

            var workflow = await _workflowRepository.SingleOrDefault(w => w.Id == id && w.AccountId == userClaims.AccountId);

            if (workflow == null)
            {
                return NotFound(new ErrorResponse<object>
                {
                    success = false,
                    message = $"Workflow with id {id} not found",
                    errors = new { }
                });
            }

            if (workflowForUpdate.Stages != null && workflowForUpdate.Stages.Count > 0)
            {
                var errors = new List<string>();
                var duplicateError = new Dictionary<string, List<string>>();

                foreach (var stage in workflowForUpdate.Stages)
                {
                    var stageToUpdate = await _workflowRepository.StageExistUnderWorkflow(stage.Id, workflow.Id);

                    if(stageToUpdate == null)
                    {
                        return NotFound(new
                        {
                            success = false,
                            message = $"Stage with id {stage.Id} not found",
                            errors = new { }
                        });
                    }

                    var stageValidator = new StageForUpdateDtoUnderWorkflowValidator();
                    var stageError = stageValidator.Validate(stage);
                    var errorList = new Dictionary<string, List<string>>();
                    if (!stageError.IsValid)
                    {
                        foreach (var err in stageError.Errors)
                        {
                            if (errorList.ContainsKey(err.PropertyName))
                            {
                                var error = errorList[err.PropertyName];
                                error.Add(err.ErrorMessage);
                                errorList[err.PropertyName] = error;
                            }
                            else
                            {
                                var messageError = new List<string>();
                                messageError.Add(err.ErrorMessage);
                                errorList[err.PropertyName] = messageError;
                            }

                        }

                        return BadRequest(new
                        {
                            success = false,
                            message = "Your workflow update request failed",
                            errors = new
                            {
                                stages = new Dictionary<string, List<string>>[] { errorList }
                            }
                        });
                    }

                    var duplicateTitleExists = workflowForUpdate.Stages.Where(s => s.Title == stage.Title).Count();
                    if (duplicateTitleExists > 1)
                    {
                        errors.Add("Duplicate title");
                        duplicateError.Add("title", errors);
                        return BadRequest(new
                        {
                            success = false,
                            message = "Your workflow update request failed",
                            errors = new
                            {
                                stages = new Dictionary<string, List<string>>[] { duplicateError }
                            }
                        });
                    }

                    var duplicateIndexExists = workflowForUpdate.Stages.Where(s => s.Index == stage.Index).Count();

                    if (duplicateIndexExists > 1)
                    {
                        errors.Add("Duplicate index");
                        duplicateError.Add("index", errors);
                        return BadRequest(new
                        {
                            success = false,
                            message = "Your workflow update request failed",
                            errors = new
                            {
                                stages = new Dictionary<string, List<string>>[] { duplicateError }
                            }
                        });
                    }

                    var (stageDto, errorDictionary) = await ValidateUpdateForStageUnderWorkflow(stage, userClaims);

                    if (errorDictionary.Values != null && errorDictionary.Values.Count > 0)
                    {
                        return BadRequest(new
                        {
                            success = false,
                            message = "Your workflow update request failed",
                            errors = new
                            {
                                stages = new Dictionary<string, List<string>>[] { errorDictionary }
                            }
                        });
                    }

                    _mapper.Map(stage, stageToUpdate);
                    stageToUpdate.UpdatedAt = DateTime.Now;
                    _workflowRepository.UpdateStageUnderWorkflow(stageToUpdate);
                }
            }

            workflow.Title = string.IsNullOrEmpty(workflowForUpdate.Title) ? workflow.Title : workflowForUpdate.Title;
            _workflowRepository.UpdateWorkflow(workflow);
            await _workflowRepository.SaveChangesAsync();

            var workflowDto = _mapper.Map<WorkflowDTO>(workflow);

            var userActivity = new UserActivity
            {
                EventType = "Workflow Updated",
                UserId = userClaims.UserId,
                ObjectClass = "WORKFLOW",
                ObjectId = workflow.Id,
                AccountId = userClaims.AccountId,
                IpAddress = Request.GetHeader(USER_IP_ADDRESS)
            };

            await _userActivityRepository.AddAsync(userActivity);

            foreach (var stage in workflow.Stages)
            {
                var stageUserActivity = new UserActivity
                {
                    EventType = "Stage Created",
                    UserId = userClaims.UserId,
                    ObjectClass = "STAGE",
                    ObjectId = stage.Id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(stageUserActivity);
                await _userActivityRepository.SaveChangesAsync();
            }


            return Ok(new SuccessResponse<WorkflowDTO>
            {
                success = true,
                message = "Workflow updated successfully",
                data = workflowDto
            });
        }

        #region CreateStageResource

        private string CreateStageResource(StageParameter parameter, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetStages",
                        new
                        {
                            PageNumber = parameter.PageNumber - 1,
                            parameter.PageSize
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetStages",
                        new
                        {
                            PageNumber = parameter.PageNumber + 1,
                            parameter.PageSize
                        });

                default:
                    return Url.Link("GetStages",
                        new
                        {
                            parameter.PageNumber,
                            parameter.PageSize
                        });
            }
        }

        #endregion CreateStageResource

        #region CreateWorkflowResource

        private string CreateWorkflowResource(WorkflowParameter parameter, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetWorkflows",
                        new
                        {
                            PageNumber = parameter.PageNumber - 1,
                            parameter.PageSize,
                            parameter.Search
                        });

                case ResourceUriType.NextPage:
                    return Url.Link("GetWorkflows",
                        new
                        {
                            PageNumber = parameter.PageNumber + 1,
                            parameter.PageSize,
                            parameter.Search
                        });

                default:
                    return Url.Link("GetWorkflows",
                        new
                        {
                            parameter.PageNumber,
                            parameter.PageSize,
                            parameter.Search
                        });
            }
        }

        #endregion CreateWorkflowResource

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResponse<WorkflowDTO>), 200)]
        public async Task<IActionResult> CreateWorkflow(WorkflowForCreationDTO workflowForCreation)
        {
            try
             {
            if (!ModelState.IsValid)
            {
                return BadRequest(new 
                {
                    success = false,
                    message = "Your workflow creation request failed",
                    errors = new
                    {
                        stages = new Dictionary<string, string[]>[] {ModelState.Error()}
                    }
                });
            }
                var userClaims = User.UserClaims();

                var workflowTitleExist = await _workflowRepository.ExistsAsync(w => w.Title == workflowForCreation.Title 
                    && w.AccountId == userClaims.AccountId);

                if (workflowTitleExist)
                {
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your workflow creation request failed",
                        errors = new
                        {
                            title = new string[] {"Duplicate Title"}
                        }
                    });
                }

                if (workflowForCreation.Stages != null && workflowForCreation.Stages.Length > 0)
                {
                    var errors = new List<string>();
                    var duplicateError = new Dictionary<string, List<string>>();
                    foreach (var stage in workflowForCreation.Stages)
                    {

                        var stageValidator = new StageForCreationDtoValidator();
                        var stageError = stageValidator.Validate(stage);
                        var errorList = new Dictionary<string, List<string>>();
                        if (!stageError.IsValid)
                        {
                            foreach (var err in stageError.Errors)
                            {
                                if (errorList.ContainsKey(err.PropertyName))
                                {
                                    var error = errorList[err.PropertyName];
                                    error.Add(err.ErrorMessage);
                                    errorList[err.PropertyName] = error;
                                }
                                else
                                {
                                    var messageError = new List<string>
                                    {
                                        err.ErrorMessage
                                    };
                                    errorList[err.PropertyName] = messageError;
                                }
                                
                            }

                            return BadRequest(new
                            {
                                success = false,
                                message = "Your workflow creation request failed",
                                errors = new
                                {
                                    stages = new Dictionary<string, List<string>>[] {errorList}
                                }
                            });
                        }

                        var duplicateTitleExists = workflowForCreation.Stages.Where(s => s.Title == stage.Title).Count();
                        if (duplicateTitleExists > 1)
                        {
                            errors.Add("Duplicate title");
                            duplicateError.Add("title", errors);
                            return BadRequest(new
                            {
                                success = false,
                                message = "Your workflow creation request failed",
                                errors = new
                                {
                                    stages = new Dictionary<string, List<string>>[] {duplicateError}
                                }
                            });
                        }

                        var duplicateIndexExists = workflowForCreation.Stages.Where(s => s.Index == stage.Index).Count();

                        if (duplicateIndexExists > 1)
                        {
                            errors.Add("Duplicate index");
                            duplicateError.Add("index", errors);
                            return BadRequest(new
                            {
                                success = false,
                                message = "Your workflow creation request failed",
                                errors = new
                                {
                                    stages = new Dictionary<string, List<string>>[] {duplicateError}
                                }
                            });
                        }

                        var (stageDto, errorDictionary) = await ValidateStageForCreation(stage, userClaims);

                        if (errorDictionary.Values != null && errorDictionary.Values.Count > 0)
                        {
                            return BadRequest(new 
                            {
                                success = false,
                                message = "Your workflow creation request failed",
                                errors  = new
                                {
                                    stages = new Dictionary<string, List<string>>[] {errorDictionary}
                                }
                            });
                        }
                    }
                }

                var workflow = _mapper.Map<Workflow>(workflowForCreation);
                workflow.AccountId = userClaims.AccountId;
                workflow.CreatedById = userClaims.UserId;
                foreach (var workflowStage in workflow.Stages)
                {
                    workflowStage.AccountId   = userClaims.AccountId;
                    workflowStage.CreatedById = userClaims.UserId;
                }

                await _workflowRepository.AddAsync(workflow);
                await _workflowRepository.SaveChangesAsync();

                var workflowDto = _mapper.Map<WorkflowDTO>(workflow);

                var userActivity = new UserActivity
                {
                    EventType   = "Workflow Created",
                    UserId      = userClaims.UserId,
                    ObjectClass = "WORKFLOW",
                    ObjectId    = workflow.Id,
                    AccountId   = userClaims.AccountId,
                    IpAddress   = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                foreach (var stage in workflow.Stages)
                {
                    var StageuserActivity = new UserActivity
                    {
                        EventType   = "Stage Created",
                        UserId      = userClaims.UserId,
                        ObjectClass = "STAGE",
                        ObjectId    = stage.Id,
                        AccountId   = userClaims.AccountId,
                        IpAddress   = Request.GetHeader(USER_IP_ADDRESS)
                    };

                    await _userActivityRepository.AddAsync(StageuserActivity);
                    await _userActivityRepository.SaveChangesAsync();
                }

                return Ok(new SuccessResponse<WorkflowDTO>
                {
                    success = true,
                    message = "Workflow created successfully",
                    data    = workflowDto
                });
             }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpPut("{id}/stages/{stageId}")]
        [ProducesResponseType(typeof(SuccessResponse<StageDTO>), 200)]
        public async Task<IActionResult> UpdateStage(Guid id, Guid stageId, StageForUpdateDTO stageForUpdate)
        {
            try
            {
                var userClaims = User.UserClaims();
                var workflow = await _workflowRepository.SingleOrDefault(w => w.Id == id && w.AccountId == userClaims.AccountId);

                var stageExists = await _workflowRepository.StageTitleExistInWorkflow(stageForUpdate.Title, id);

                var indexExists = await _workflowRepository.IndexStageExistUnderWorkflow(stageForUpdate.Index, id);

                var stage =await _workflowRepository.StageExistUnderWorkflow(stageId, id);

                if (workflow == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Workflow with id {id} not found",
                        errors = new { }
                    });
                }

                if (stageExists)
                {
                    return Conflict(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your stage update request failed",
                        errors = new
                        {
                            title = new string[] { "Duplicate Title" }
                        }
                    });
                }


                if (stage == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Stage with id {stageId} not found",
                        errors  = new { }
                    });
                }

                if (!ModelState.IsValid)
                {
                    ModelState.Remove("workflowId");
                    ModelState.Remove("stageId");
                    return BadRequest(new ErrorResponse<Dictionary<string, string[]>>
                    {
                        success = false,
                        message = "Your stage update request failed",
                        errors = ModelState.Error()
                    });
                }

                if (indexExists)
                {
                    return BadRequest(new ErrorResponse<object>
                    {
                        success = false,
                        message = "Your stage update request failed",
                        errors = new
                        {
                            Index = new string[] { "Index already exist" }
                        }
                    });
                }

                var (stageForUpdateDto, errorDictionary) = await ValidateStageForUpdate(stageForUpdate, userClaims);

                if (errorDictionary.Values != null && errorDictionary.Values.Count > 0)
                {
                    return BadRequest(new ErrorResponse<Dictionary<string, List<string>>>
                    {
                        success = false,
                        message = "Your stage update request failed",
                        errors = errorDictionary
                    });
                }

                _mapper.Map(stageForUpdate, stage);
                stage.UpdatedAt = DateTime.Now;
                _workflowRepository.UpdateStage(stage);
                await _workflowRepository.SaveChangesAsync();

                var stageDto = _mapper.Map<StageDTO>(stage);

                var userActivity = new UserActivity
                {
                    EventType = "Stage Updated",
                    UserId = userClaims.UserId,
                    ObjectClass = "STAGE",
                    ObjectId = stage.Id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<StageDTO>
                {
                    success = true,
                    message = "Stage updated successfully",
                    data = stageDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        #region ValidateUpdateForCreation

        private async Task<(StageForUpdateDTO, Dictionary<string, List<string>>)> ValidateStageForUpdate(StageForUpdateDTO stageForUpdate, UserClaims userClaims)
        {
            var errors = new List<string>();
            var errorDictionary = new Dictionary<string, List<string>>();
            var stage = new Stage();
            if (stageForUpdate.UserType.ToUpper() == "GROUP")
            {
                int memberCount = 0;
                switch (stageForUpdate.GroupClass.ToUpper())
                {
                    case "DEPARTMENT":
                        foreach (var departmentId in stageForUpdate.GroupIds)
                        {
                            var department = await _departmentRepository.SingleOrDefault(d => d.Id == departmentId && d.AccountId == userClaims.AccountId);

                            if (department == null)
                            {
                                errors.Add($"Department with id {departmentId} not found");
                                errorDictionary.Add("groupsId", errors);
                                break;
                            }
                            else
                            {
                                var departmentMembersCount = await _departmentRepository.GetMembersCount(departmentId);

                                memberCount += departmentMembersCount;
                            }
                        }

                        if (stageForUpdate.MinimumPass > memberCount && errors.Count <= 0)
                        {
                            errors.Add("Minimum pass can not be greater than the size of the group");
                            errorDictionary.Add("minimumPass", errors);
                        }

                        break;

                    case "UNIT":
                        foreach (var unitId in stageForUpdate.GroupIds)
                        {
                            var unit = await _unitRepository.SingleOrDefault(d => d.Id == unitId && d.AccountId == userClaims.AccountId);

                            if (unit == null)
                            {
                                errors.Add($"Unit with id {unitId} not found");
                                errorDictionary.Add("groupsId", errors);
                                break;
                            }
                            else
                            {
                                var unitMembersCount = await _unitRepository.GetMembersCount(unitId);
                                memberCount += unitMembersCount;
                            }
                        }

                        if (stageForUpdate.MinimumPass > memberCount && errors.Count <= 0)
                        {
                            errors.Add("Minimum pass can not be greater than the size of the group");
                            errorDictionary.Add("minimumPass", errors);
                        }

                        break;
                }

                return (stageForUpdate, errorDictionary);
            }

            foreach (var userId in stageForUpdate.AssigneeIds)
            {
                var user = await _userRepository.SingleOrDefault(d => d.Id == userId && d.AccountId == userClaims.AccountId);
                if (user == null)
                {
                    errors.Add($"User with id {userId} not found");
                    errorDictionary.Add("assigneeIds", errors);
                    break;
                }
            }

            if (stageForUpdate.MinimumPass > stageForUpdate.AssigneeIds.Length && errors.Count <= 0)
            {
                errors.Add("Minimum pass can not be greater than the size of the group");
                errorDictionary.Add("minimumPass", errors);
            }

            return (stageForUpdate, errorDictionary);
        }


        #endregion

        #region ValidateUpdateForStageUnderWorkflow
        private async Task<(StageForUnderWorkflowUpdateDTO, Dictionary<string, List<string>>)> ValidateUpdateForStageUnderWorkflow(StageForUnderWorkflowUpdateDTO stageForUpdate, UserClaims userClaims)
        {
            var errors = new List<string>();
            var errorDictionary = new Dictionary<string, List<string>>();
            var stage = new Stage();
            if (stageForUpdate.UserType.ToUpper() == "GROUP")
            {
                int memberCount = 0;
                switch (stageForUpdate.GroupClass.ToUpper())
                {
                    case "DEPARTMENT":
                        foreach (var departmentId in stageForUpdate.GroupIds)
                        {
                            var department = await _departmentRepository.SingleOrDefault(d => d.Id == departmentId && d.AccountId == userClaims.AccountId);

                            if (department == null)
                            {
                                errors.Add($"Department with id {departmentId} not found");
                                errorDictionary.Add("groupsId", errors);
                                break;
                            }
                            else
                            {
                                var departmentMembersCount = await _departmentRepository.GetMembersCount(departmentId);

                                memberCount += departmentMembersCount;
                            }
                        }

                        if (stageForUpdate.MinimumPass > memberCount && errors.Count <= 0)
                        {
                            errors.Add("Minimum pass can not be greater than the size of the group");
                            errorDictionary.Add("minimumPass", errors);
                        }

                        break;

                    case "UNIT":
                        foreach (var unitId in stageForUpdate.GroupIds)
                        {
                            var unit = await _unitRepository.SingleOrDefault(d => d.Id == unitId && d.AccountId == userClaims.AccountId);

                            if (unit == null)
                            {
                                errors.Add($"Unit with id {unitId} not found");
                                errorDictionary.Add("groupsId", errors);
                                break;
                            }
                            else
                            {
                                var unitMembersCount = await _unitRepository.GetMembersCount(unitId);
                                memberCount += unitMembersCount;
                            }
                        }

                        if (stageForUpdate.MinimumPass > memberCount && errors.Count <= 0)
                        {
                            errors.Add("Minimum pass can not be greater than the size of the group");
                            errorDictionary.Add("minimumPass", errors);
                        }

                        break;
                }

                return (stageForUpdate, errorDictionary);
            }

            foreach (var userId in stageForUpdate.AssigneeIds)
            {
                var user = await _userRepository.SingleOrDefault(d => d.Id == userId && d.AccountId == userClaims.AccountId);
                if (user == null)
                {
                    errors.Add($"User with id {userId} not found");
                    errorDictionary.Add("assigneeIds", errors);
                    break;
                }
            }

            if (stageForUpdate.MinimumPass > stageForUpdate.AssigneeIds.Length && errors.Count <= 0)
            {
                errors.Add("Minimum pass can not be greater than the size of the group");
                errorDictionary.Add("minimumPass", errors);
            }

            return (stageForUpdate, errorDictionary);
        }
        #endregion

        [HttpDelete("{id}/stages/{stageId}")]
        [ProducesResponseType(typeof(SuccessResponse<StageDTO>), 200)]
        public async Task<IActionResult> SoftDeleteStage(Guid id, Guid stageId)
        {
            try
            {
                var userClaims = User.UserClaims();

                var workflow = await _workflowRepository.SingleOrDefault(w => w.Id == id && w.AccountId == userClaims.AccountId);

                var stage = await _workflowRepository.StageExistUnderWorkflow(stageId, id);

                if (workflow == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Workflow with id {id} not found",
                        errors = new { }
                    });
                }

                if (stage == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Stage with id {stageId} not found",
                        errors = new { }
                    });
                }

                stage.Deleted = true;
                stage.DeletedAt = DateTime.Now;
                _workflowRepository.UpdateStage(stage);
                await _workflowRepository.SaveChangesAsync();

                var userActivity = new UserActivity
                {
                    EventType = "Stage Deleted",
                    UserId = userClaims.UserId,
                    ObjectClass = "STAGE",
                    ObjectId = stage.Id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();

                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Stage deleted successfully",
                    data = new { }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(SuccessResponse<StageDTO>), 200)]
        public async Task<IActionResult> SoftDeleteWorkflow(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();

                var workflow = await _workflowRepository.SingleOrDefault(w => w.Id == id && w.AccountId == userClaims.AccountId);

                if (workflow == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Workflow with id {id} not found",
                        errors = new { }
                    });
                }

                workflow.Deleted = true;
                workflow.DeletedAt = DateTime.Now; ;
                _workflowRepository.Update(workflow);
                await _workflowRepository.SaveChangesAsync();

                var userActivity = new UserActivity
                {
                    EventType = "Workflow Deleted",
                    UserId = userClaims.UserId,
                    ObjectClass = "WORKFLOW",
                    ObjectId = workflow.Id,
                    AccountId = userClaims.AccountId,
                    IpAddress = Request.GetHeader(USER_IP_ADDRESS)
                };

                await _userActivityRepository.AddAsync(userActivity);
                await _userActivityRepository.SaveChangesAsync();
                return Ok(new SuccessResponse<object>
                {
                    success = true,
                    message = "Workflow deleted successfully",
                    data = new { }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkflowById(Guid id)
        {
            try
            {
                var userClaims = User.UserClaims();

                var workflow = await _workflowRepository.GetWorkflowById(id, userClaims.AccountId);

                if (workflow == null)
                {
                    return NotFound(new ErrorResponse<object>
                    {
                        success = false,
                        message = $"Workflow with id {id} not found",
                        errors = new { }
                    });
                }

                var workflowDTO = _mapper.Map<WorkflowForGetDTO>(workflow);

                foreach (var stage in workflowDTO.Stages)
                {
                    if (!string.IsNullOrEmpty(stage.GroupClass) && stage.GroupClass.ToUpper() == "UNIT")
                    {
                        var unit = await _unitRepository.GetByIdAsync(Guid.Parse(stage.GroupIds[0]));
                        var department = await _departmentRepository.GetByIdAsync(unit.DepartmentId);

                        stage.Department = _mapper.Map<DepartmentDTO>(department);
                    }
                }

                return Ok(new SuccessResponse<WorkflowForGetDTO>
                {
                    success = true,
                    message = "Workflow retrieved successfully",
                    data = workflowDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<string>.InternalError(ex.Message));
            }
        }
    }
}
