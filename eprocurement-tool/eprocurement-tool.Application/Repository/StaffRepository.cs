using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Application.Models.StaffModels;
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
    public class StaffRepository : Repository<StaffProfile>, IStaffRepository
    {
        public StaffRepository(EDMSDBContext context) : base(context)
        {

        }

        public async Task<bool> IsStaff(Guid userId)
        {
            bool isStaff = await _context.Users.Where(x => x.Id == userId && x.UserType == EUserType.STAFF)
                                            .AnyAsync();

            return isStaff;
        }

        public async Task<PagedList<User>> GetAllStaffs(StaffParameters parameters, Guid userId)
        {
            var query = _context.Users as IQueryable<User>;

            //check if parameter values are null or empty and add them to query if they aren't
            if (!string.IsNullOrEmpty(parameters.name))
            {
                var name = parameters.name.Trim();
                query = query.Where(x => x.FirstName.ToLower().Contains(name.ToLower()) 
                || x.LastName.ToLower().Contains(name.ToLower()));
            }

            if (parameters.ministryId != null)
            {
                query = query.Where(x => x.MinistryId == parameters.ministryId);
            }

            if (parameters.roleId != null)
            {
                query = query.Where(x => x.UserRoleId == parameters.roleId);
            }

            if (parameters.status == EStatus.ENABLED || parameters.status == EStatus.DISABLED)
            {
                query = query.Where(x => x.Status == parameters.status);
            }

            var staffProfileQuery = query.Where(x => x.UserType == EUserType.STAFF)
                .Include(x => x.UserRoles)
                .Include(a=>a.Ministry)
                .OrderByDescending(a=>a.CreateAt)
                ;
      
            var staffProfiles =  await PagedList<User>.Create(staffProfileQuery, parameters.PageNumber, parameters.PageSize);

            if (staffProfiles.Count > 0)
            {
                foreach (var item in staffProfiles)
                {
                    item.Ministry = await _context.Ministries.FirstOrDefaultAsync(m => m.Id == item.MinistryId);
                }
            }

            return staffProfiles;
        }

        public Task<PagedList<User>> GetStaffDetails(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<StaffSummaryDto> GetStaffSummaryDetails()
        {
            var query = _context.Users.Where(x => x.UserType == EUserType.STAFF).AsNoTracking();

            var totalStaffs = await query.CountAsync();
            var totalActiveStaffs = await query.Where(x => x.Status == EStatus.ENABLED).CountAsync();
            var totalPendingStaffs = await query.Where(x => x.Status == EStatus.DISABLED).CountAsync();

            var summaryDetails = new StaffSummaryDto()
            {
                Total = totalStaffs,
                ActiveTotal = totalActiveStaffs,
                PendingTotal = totalPendingStaffs
            };

            return summaryDetails;
        }
    }
}
