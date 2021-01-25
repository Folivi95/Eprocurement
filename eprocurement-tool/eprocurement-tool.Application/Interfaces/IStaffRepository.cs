using EGPS.Application.Models;
using EGPS.Application.Models.StaffModels;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IStaffRepository : IRepository<StaffProfile>
    {
        Task<bool> IsStaff(Guid userId);
        Task<PagedList<User>> GetAllStaffs(StaffParameters parameters, Guid userId);
        Task<PagedList<User>> GetStaffDetails(Guid accountId);
        Task<StaffSummaryDto> GetStaffSummaryDetails();
    }
}
