using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IVendorBidRepository : IRepository<VendorBid>
    {
        Task<PagedList<VendorBid>> GetAllVendorBidsOfProcurementPlan(Guid procurementPlanId, VendorBidParameter parameters);

        Task<bool> HasBided(Guid procurementPlanId, Guid vendorId);
        Task<IEnumerable<VendorBid>> GetNotStartedBids(Guid procurementPlanId);
        Task<VendorBid> GetVendorBid(Guid vendorId, Guid procurementPlanId);
    }
}
