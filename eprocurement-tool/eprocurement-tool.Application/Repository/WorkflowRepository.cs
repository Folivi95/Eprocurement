using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EGPS.Application.Repository
{
    public class WorkflowRepository : Repository<Workflow>, IWorkflowRepository
    {
        public WorkflowRepository(EDMSDBContext context)
            : base(context) { }

        public async Task<bool> StageTitleExistInWorkflow(string title, Guid workflowId)
        {
            var exist = await _context.Stages.AnyAsync(s => s.Title == title && s.WorkFlowId == workflowId);
            return exist;
        }

        public async Task<bool> IndexStageExistUnderWorkflow(int index, Guid workflowId)
        {
            var query = _context.Workflows
                                .Join(_context.Stages, w => w.Id, s => s.WorkFlowId,
                                      (w, s) => new { w, s })
                                .Where(x => x.w.Id == workflowId && x.s.Index == index);
            var result = await query.Select(x => x.w).AnyAsync();

            return result;
        }

        public async Task AddStage(Stage stage)
        {
            await _context.Stages.AddAsync(stage);
        }

        public void UpdateStageUnderWorkflow(Stage stage)
        {
            _context.Stages.Update(stage);
        }

        public void UpdateWorkflow(Workflow workflow)
        {
            _context.Workflows.Update(workflow);
        }

        public async Task<PagedList<Stage>> GetStagesUnderWorflow(Guid workflowId, StageParameter parameter)
        {
            var query = _context.Workflows
                                .Join(_context.Stages, w => w.Id, s => s.WorkFlowId,
                                      (w, s) => new { w, s })
                                .Where(x => x.w.Id == workflowId);
            var stages = query.Select(x => x.s);

            var stagesPageList = await PagedList<Stage>.Create(stages, parameter.PageNumber, parameter.PageSize);

            return stagesPageList;
        }

        public async Task<PagedList<Workflow>> GetWorkflows(Guid accountId, WorkflowParameter parameters)
        {
            var query = _context.Workflows as IQueryable<Workflow>;

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                var search = parameters.Search.Trim();
                query = query.Where(x => x.Title.ToLower().Contains(search.ToLower()));
            }

            query = query.Where(x => x.AccountId == accountId).Include(x => x.Stages);
            return await PagedList<Workflow>.Create(query, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Stage> StageExistUnderWorkflow(Guid stageId, Guid workflowId)
        {
            var stage = await _context.Stages.SingleOrDefaultAsync(s => s.Id == stageId && s.WorkFlowId == workflowId);

            return stage;
        }

        public void UpdateStage(Stage stage)
        {
            
        }

        public async Task<Workflow> GetWorkflowById(Guid workflowId, Guid accountId)
        {
            if(workflowId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(workflowId));
            }

            if(accountId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(accountId));
            }

            return await _context.Workflows
                .Where(x => x.AccountId == accountId && x.Id == workflowId)
                .Include(s => s.Stages)
                .FirstOrDefaultAsync();
        }
    }
}
