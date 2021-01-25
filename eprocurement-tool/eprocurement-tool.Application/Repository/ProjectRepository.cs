using EGPS.Application.Helpers;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Repository
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(EDMSDBContext context) : base(context)
        {

        }

        public async Task<PagedList<Project>> GetProjects(ProjectParameters parameters, UserClaims userClaims)
        {

            var query = _context.Projects as IQueryable<Project>;

            if (userClaims.UserType == EUserType.VENDOR)
                query = query.Where(x => x.VendorId == userClaims.UserId);

            //if (userClaims.Role != ERole.Vendor && userClaims.Role != ERole.EXECUTIVE)
            //{
            //    var user = _context.Users.SingleOrDefault(x => x.Id == userClaims.UserId);
            //    query = query.Where(x => x.MinistryId == user.MinistryId);
            //}

            if (!String.IsNullOrEmpty(parameters.Title))
            {
                query = query.Where(x => x.Title.Contains(parameters.Title));
            }

            if (!String.IsNullOrEmpty(parameters.Category))
            {
                query = query.Where(x => x.Contract.RegistrationPlan.Title.Contains(parameters.Category));
            }

            if (parameters.StartDate.GetHashCode() != 0 && parameters.StartDate != null)
            {
                query = query.Where(x => x.StartDate == parameters.StartDate);
            }

            if (parameters.ExpiryDate.GetHashCode() != 0 && parameters.ExpiryDate != null)
            {
                query = query.Where(x => x.EndDate.Value.Date <= parameters.ExpiryDate.Value.Date);
            }

            var projectsQuery = query.Include(x => x.Contract.RegistrationPlan).DefaultIfEmpty().AsNoTracking();

            var projects = await PagedList<Project>.Create(projectsQuery, parameters.PageNumber, parameters.PageSize);

            return projects;
        }

        public async Task<ProjectsSummaryDTO> GetProjectsSummary()
        {
            var query = _context.Projects as IQueryable<Project>;

            int total = await query.AsNoTracking().CountAsync();
            int active = await query.Where(x => x.Status == EProjectStatus.ACTIVE).AsNoTracking().CountAsync();
            int inactive = await query.Where(x => x.Status == EProjectStatus.INACTIVE).AsNoTracking().CountAsync();
            int completed = await query.Where(x => x.Status == EProjectStatus.COMPLETED).AsNoTracking().CountAsync();

            var summary = new ProjectsSummaryDTO()
            {
                Total = total,
                Active = active,
                Inactive = inactive,
                Completed = completed,
            };

            return summary;
        }

        public async Task AddProjectMileStone(ProjectMileStone projectMileStone)
        {
            await _context.ProjectMileStones.AddAsync(projectMileStone);
        }

        public async Task<Project> GetProject(Guid projectId)
        {
            var project = await _context.Projects
                .Where(x => x.Id == projectId)
                .Include(x => x.Contract)
                .Include(x => x.ProjectMileStones)
                .SingleOrDefaultAsync();

            return project;
        }

        public async Task<Project> GetProjectByContractId(Guid contractId)
        {
            var project = _context.Projects
                .Where(x => x.ContractId == contractId)
                .Include(x => x.Contract)
                .Include(x => x.ProjectMileStones);

            return await project.SingleOrDefaultAsync();
        }

        public async Task<MilestoneTask> CreateMilestoneTask(MilestoneTask milestoneTask)
        {
            await _context.MilestoneTasks.AddAsync(milestoneTask);
            await _context.SaveChangesAsync();

            return milestoneTask;
        }

        public async Task<bool> CheckIfMileStoneExists(Guid projectMileStoneId)
        {
            return await _context.ProjectMileStones.AnyAsync(x => x.Id == projectMileStoneId);
        }

        public async Task<bool> CheckIfMileStoneTitleExists(string Title, Guid projectMileStoneId)
        {
            return await _context.MilestoneTasks.AnyAsync(x =>
                    x.Title.ToLower().Trim() == Title.ToLower().Trim() && x.MileStoneId == projectMileStoneId);
        }

        public async Task<int> GetTotalCountOfProjects()
        {
            return await _context.Projects.CountAsync();
        }

        public Task DeleteProjectMileStone(Guid id)
        {
            throw new NotImplementedException();
        }

        public ProjectMileStone GetProjectMileStoneById(Guid Id)
        {
            return _context.ProjectMileStones.Where(x => x.Id == Id).FirstOrDefault();
        }

        public async Task<PagedList<Project>> GetAllVendorProjects(Guid userId, ResourceParameters parameters)
        {
            var query = _context.Projects.Where(x => x.VendorId == userId);
            var vendorProjects = await PagedList<Project>.Create(query, parameters.PageNumber, parameters.PageSize);
            return vendorProjects;
        }

        public async Task<ProjectsSummaryDTO> GetProjectsSummaryForVendor(Guid id)
        {
            var query = _context.Projects.Where(x => x.VendorId == id) as IQueryable<Project>;

            int total = await query.AsNoTracking().CountAsync();
            int active = await query.Where(x => x.Status == EProjectStatus.ACTIVE).AsNoTracking().CountAsync();
            int inactive = await query.Where(x => x.Status == EProjectStatus.INACTIVE).AsNoTracking().CountAsync();
            int completed = await query.Where(x => x.Status == EProjectStatus.COMPLETED).AsNoTracking().CountAsync();

            var summary = new ProjectsSummaryDTO()
            {
                Total = total,
                Active = active,
                Inactive = inactive,
                Completed = completed,
            };

            return summary;
        }

        public async Task<decimal> GetPercentageComplete(Guid projectId)
        {
            var totalTask = (decimal)(await _context.ProjectMileStones
                .LongCountAsync(a => a.ProjectId == projectId));

            var completedTask = (decimal)(await _context.ProjectMileStones
                .LongCountAsync(a => a.ProjectId == projectId && a.Status == EMilestoneStatus.DONE));

            totalTask = (totalTask == 0 ? 1 : totalTask);
            var percentage = (completedTask / totalTask) * 100;
            return percentage;
        }

        public async Task<ProjectsSummaryDTO> GetProjectsSummaryByMinistry(Guid ministryId)
        {
            var query = _context.Projects as IQueryable<Project>;

            query = query.Where(p => p.MinistryId == ministryId);

            int total = await query.AsNoTracking().CountAsync();
            int active = await query.Where(x => x.Status == EProjectStatus.ACTIVE).AsNoTracking().CountAsync();
            int inactive = await query.Where(x => x.Status == EProjectStatus.INACTIVE).AsNoTracking().CountAsync();
            int completed = await query.Where(x => x.Status == EProjectStatus.COMPLETED).AsNoTracking().CountAsync();

            var summary = new ProjectsSummaryDTO()
            {
                Total = total,
                Active = active,
                Inactive = inactive,
                Completed = completed,
            };

            return summary;
        }

    }
}
