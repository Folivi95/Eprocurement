using System.Linq;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;

namespace EGPS.Application.Repository
{
    public class UserActivityRepository : Repository<UserActivity> , IUserActivityRepository
    {
        public UserActivityRepository(EDMSDBContext context)
            : base(context)
        {

        }

        public Task<PagedList<UserActivitiesDTO>> GetAudits(AuditParameters parameters, Guid accountId)
        {
            var query = _context.UserActivities.Where(x => x.AccountId == accountId)
                                .Select(x => new UserActivitiesDTO
                                {
                                    Id = x.Id,
                                    EventType = x.EventType,
                                    ObjectClass = x.ObjectClass,
                                    ObjectId = x.ObjectId,
                                    UserId = x.UserId,
                                    AccountId = x.AccountId,
                                    Details = x.Details,
                                    CreatedAt = x.CreatedAt,
                                    IpAddress = x.IpAddress,
                                    User = new AuditUserDTO
                                    {
                                        Id = x.User.Id,
                                        FirstName = x.User.FirstName,
                                        LastName = x.User.LastName,
                                        ProfilePicture = string.IsNullOrEmpty(x.User.ProfilePicture) ?               null : JsonConvert.DeserializeObject(x.User.ProfilePicture)
                                    }
                                });

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                var search = parameters.Search.Trim();
                query = query.Where(x => x.User.FirstName.ToLower().Contains(search.ToLower()) || x.User.LastName.ToLower().Contains(search.ToLower()));
            }

            if (parameters.StartDate.HasValue && parameters.EndDate.HasValue)
            {
                query = query.Where(x => x.CreatedAt >= parameters.StartDate.Value.Date && x.CreatedAt <= parameters.EndDate.Value.Date);
            }

            var userActivities = PagedList<UserActivitiesDTO>.Create(query, parameters.PageNumber, parameters.PageSize);


            return userActivities;
        }

        public async Task<PagedList<UserActivity>> GetVendorActivities(Guid userId, ResourceParameters parameters)
        {
            var query =  _context.UserActivities.Where(x => x.UserId == userId).OrderByDescending(c => c.CreatedAt);


            var vendorActivities = await PagedList<UserActivity>.Create(query, parameters.PageNumber, parameters.PageSize);

            return vendorActivities;
        }
    }
}
