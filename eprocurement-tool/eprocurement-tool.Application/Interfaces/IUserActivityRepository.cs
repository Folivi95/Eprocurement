using System;
using System.Threading.Tasks;
using EGPS.Application.Models;
using EGPS.Domain.Entities;

namespace EGPS.Application.Interfaces
{
    public interface IUserActivityRepository : IRepository<UserActivity>
    {
        Task<PagedList<UserActivitiesDTO>> GetAudits(AuditParameters parameters, Guid accountId);

        Task<PagedList<UserActivity>> GetVendorActivities(Guid userId, ResourceParameters parameters);
    }
}
