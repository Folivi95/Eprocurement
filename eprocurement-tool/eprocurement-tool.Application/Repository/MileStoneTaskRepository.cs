using EGPS.Application.Interfaces;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using EGPS.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace EGPS.Application.Repository
{
    public class MileStoneTaskRepository : Repository<MilestoneTask>, IMileStoneTaskRepository
    {
        public MileStoneTaskRepository(EDMSDBContext context) : base(context)
        {
        }

        public async Task<PagedList<MilestoneTask>> GetMileStoneTasks(Guid milestoneId, MileStoneTaskParameter parameter)
        {
            var query = _context.MilestoneTasks.Where(a => !a.Deleted && a.MileStoneId == milestoneId).OrderByDescending(a => a.CreateAt); 
            return await PagedList<MilestoneTask>.Create(query, parameter.PageNumber, parameter.PageSize);
        }

        public async Task<IEnumerable<MilestoneTask>> GetMileStoneTasks(Guid milestoneId)
        {
            return await _context.MilestoneTasks.Where(a => a.MileStoneId == milestoneId).ToListAsync();
        }
     }
}
