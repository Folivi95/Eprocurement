using EGPS.Application.Helpers;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<ProjectsSummaryDTO> GetProjectsSummary();
        Task<PagedList<Project>> GetProjects(ProjectParameters parameters, UserClaims userClaims);
        Task AddProjectMileStone(ProjectMileStone projectMileStone);
        Task<Project> GetProject(Guid projectId);
        Task<Project> GetProjectByContractId(Guid contractId);
        Task<MilestoneTask> CreateMilestoneTask(MilestoneTask milestoneTask);
        Task<bool> CheckIfMileStoneExists(Guid projectMileStoneId);
        Task<bool> CheckIfMileStoneTitleExists(string Title, Guid projectMileStoneId);
        Task<int> GetTotalCountOfProjects();     
        Task DeleteProjectMileStone(Guid id);
        ProjectMileStone GetProjectMileStoneById(Guid Id);

        Task<decimal> GetPercentageComplete(Guid projectId);
        Task<ProjectsSummaryDTO> GetProjectsSummaryForVendor(Guid id);

        Task<PagedList<Project>> GetAllVendorProjects(Guid vendorId, ResourceParameters parameters);
        Task<ProjectsSummaryDTO> GetProjectsSummaryByMinistry(Guid ministryId);



    }
}
