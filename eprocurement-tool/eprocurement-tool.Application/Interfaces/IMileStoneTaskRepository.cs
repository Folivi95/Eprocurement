using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EGPS.Application.Models;

namespace EGPS.Application.Interfaces
{
    public interface IMileStoneTaskRepository : IRepository<MilestoneTask>
    {
        Task<PagedList<MilestoneTask>> GetMileStoneTasks(Guid milestoneId, MileStoneTaskParameter parameter); 
        Task<IEnumerable<MilestoneTask>> GetMileStoneTasks(Guid milestoneId);
    }
}
