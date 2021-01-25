using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace EGPS.Application.Interfaces
{
    public interface IMinistryRepository : IRepository<Ministry>
    {
        Task<PagedList<Ministry>> GetAllMinistriesByUserId(MinistryParameters parameter, Guid userId);
        Task<PagedList<VendorProfile>> GetVendors(Guid ministryId, VendorParameters parameter);
        Task<PagedList<User>> GetUsersByMinistry(Guid ministryId, ResourceParameters parameter);
        Task<PagedList<MinistryDTO>> TotalBidsForMinistry(IEnumerable<MinistryDTO> ministries, MinistryParameters parameter);
    }
}
