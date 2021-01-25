using EGPS.Application.Interfaces;
using EGPS.Application.Models;
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
    public class BidRepository : Repository<Bid>, IBidRepository
    {
        public BidRepository(EDMSDBContext context) : base(context)
        {

        }

        public async Task<PagedList<Bid>> GetAllBids(BidsParameters parameters)
        {
            var query = _context.Bids as IQueryable<Bid>;

            if (!String.IsNullOrEmpty(parameters.Title))
            {
                query = query.Where(x => x.Title.Contains(parameters.Title));
            }

            if (!String.IsNullOrEmpty(parameters.Process))
            {
                query = query.Where(x => x.ProcurementProcess.Name.Contains(parameters.Process));
            }

            if (parameters.Status == EBidStatus.ACTIVE || parameters.Status == EBidStatus.CLOSED
                || parameters.Status == EBidStatus.NOTSTARTED || parameters.Status == EBidStatus.PROCESSING)
            {
                query = query.Where(x => x.Status == parameters.Status);
            }

            if (parameters.ExpiryDate.GetHashCode() != 0 && parameters.ExpiryDate != null)
            {
                query = query.Where(x => x.DueDate.Date <= parameters.ExpiryDate.Value.Date);
            }

            var bidsQuery = query.Include(x => x.ProcurementProcess).AsNoTracking();

            var bids = await PagedList<Bid>.Create(bidsQuery, parameters.PageNumber, parameters.PageSize);

            return bids;
        }

        public async Task<BidSummaryDTO> GetBidSummary()
        {
            IQueryable<VendorBid> query = _context.VendorBids.AsNoTracking();

            int total = await query.CountAsync();
            int approved = await query.Where(x => x.Type == EVendorContractStatus.RECOMMENDED).CountAsync();
            int processing = await query.Where(x => x.Type == EVendorContractStatus.PROCESSING).CountAsync();
            int rejected = await query.Where(x => x.Type == EVendorContractStatus.REJECTED).CountAsync();
            int notStarted = await query.Where(x => x.Type == EVendorContractStatus.NOTSTARTED).CountAsync();
            int evaluated = await query.Where(x => x.Type == EVendorContractStatus.EVALUATED).CountAsync();


            BidSummaryDTO bidSummary = new BidSummaryDTO()
            {
                Total = total,
                Approved = approved,
                Processing = processing,
                Rejected = rejected + evaluated,
                NotStarted = notStarted,
            };

            return bidSummary;
        }

        public async Task<BidSummaryDTO> GetVendorBidSummary(Guid? userId)
        {
            IQueryable<VendorBid> query = _context.VendorBids.AsNoTracking();

            if (userId.HasValue)
                query = query.Where(x => x.VendorId == userId.Value && x.Type != EVendorContractStatus.INTERESTED);

            int total = await query.CountAsync();
            int approved = await query.Where(x => x.Type == EVendorContractStatus.RECOMMENDED).CountAsync();
            int processing = await query.Where(x => x.Type == EVendorContractStatus.PROCESSING).CountAsync();
            int rejected = await query.Where(x => x.Type == EVendorContractStatus.REJECTED).CountAsync();
            int notStarted = await query.Where(x => x.Type == EVendorContractStatus.NOTSTARTED).CountAsync();

            BidSummaryDTO bidSummary = new BidSummaryDTO()
            {
                Total = total,
                Approved = approved,
                Processing = processing,
                Rejected = rejected,
                NotStarted = notStarted,
            };

            return bidSummary;
        }

        public async Task<PagedList<VendorBid>> GetRecentBidsForVendor(Guid? userId, ResourceParameters parameters)
        {
            var bids = _context.VendorBids as IQueryable<VendorBid>;

            bids = bids.OrderByDescending(x => x.ExpiryDate);

            if (userId.HasValue)
                bids = bids.Where(x => x.VendorId == userId.Value);

            bids = bids.Include(x => x.ProcurementPlan);

            var vendorBids = await PagedList<VendorBid>.Create(bids, parameters.PageNumber, parameters.PageSize);
            return vendorBids;
        }

        public Task<VendorBid> GetABid(Guid VendorBidId)
        {
            var VendorBid = _context.VendorBids.Where(x => x.Id == VendorBidId).Include(x => x.ProcurementPlan).FirstOrDefaultAsync();

            return VendorBid;
        }
    }
}
