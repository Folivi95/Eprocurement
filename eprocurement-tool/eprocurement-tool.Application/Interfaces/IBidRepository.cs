using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IBidRepository : IRepository<Bid>
    {
        Task<BidSummaryDTO> GetBidSummary();
        Task<PagedList<Bid>> GetAllBids(BidsParameters parameters);
        Task<PagedList<VendorBid>> GetRecentBidsForVendor(Guid? userId, ResourceParameters parameters);


        Task<BidSummaryDTO> GetVendorBidSummary(Guid? userId);
        Task<VendorBid> GetABid(Guid VendorBidId);
    }
}
