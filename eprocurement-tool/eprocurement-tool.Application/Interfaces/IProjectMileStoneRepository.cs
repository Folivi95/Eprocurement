using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IProjectMileStoneRepository : IRepository<ProjectMileStone>
    {
        Task<MilestoneInvoice> GetMilestoneInvoice(Guid milestoneId);
        Task<bool> CreateMilestoneInvoice(ProjectMileStone milestone, MilestoneInvoiceForCreation milestoneInvoice);

        Task<decimal> GetPercentageComplete(Guid projectMileStoneId);

        Task<IEnumerable<MilestoneTask>> GetMilestoneTasks(Guid mileStoneId);
    }
}
