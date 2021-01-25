using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IWorkflowRepository : IRepository<Workflow>
    {
        Task<bool> StageTitleExistInWorkflow(string title, Guid workflowId);

        Task<bool> IndexStageExistUnderWorkflow(int index, Guid workflowId);

        Task AddStage(Stage stage);

        Task<PagedList<Stage>> GetStagesUnderWorflow(Guid workflowId, StageParameter parameter);

        Task<PagedList<Workflow>> GetWorkflows(Guid accountId, WorkflowParameter parameters);
        Task<Stage> StageExistUnderWorkflow(Guid stageId, Guid workflowId);
        void UpdateStage(Stage stage);
        void UpdateStageUnderWorkflow(Stage stage);
        void UpdateWorkflow(Workflow workflow);
        Task<Workflow> GetWorkflowById(Guid workflowId, Guid accountId);
    }
}
