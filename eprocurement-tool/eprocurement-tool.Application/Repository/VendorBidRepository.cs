using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Repository
{
    public class VendorBidRepository : Repository<VendorBid>, IVendorBidRepository
    {
        public VendorBidRepository(EDMSDBContext context) :
            base(context)
        {

        }

        public async Task<bool> HasBided(Guid procurementPlanId, Guid vendorId)
        {
            var vendorBidsQuery = await _context.VendorBids.Where(x => x.VendorId == vendorId && x.ProcurementPlanId == procurementPlanId && x.Type == Domain.Enums.EVendorContractStatus.PROCESSING).ToListAsync();

            return vendorBidsQuery.Count > 0;
        }

        public Task<PagedList<VendorBid>> GetAllVendorBidsOfProcurementPlan(Guid procurementPlanId, VendorBidParameter parameters)
        {
            var vendorBidQuery = _context.VendorBids.Where(x => x.ProcurementPlanId == procurementPlanId && (x.Type != Domain.Enums.EVendorContractStatus.INTERESTED || x.Type != Domain.Enums.EVendorContractStatus.NOTSTARTED))
                .Include(x => x.ProcurementPlan);

            var vendorBids = PagedList<VendorBid>.Create(vendorBidQuery, parameters.PageNumber, parameters.PageSize);

            return vendorBids;

        }

        public async Task<IEnumerable<VendorBid>> GetNotStartedBids(Guid procurementPlanId)
        {
            var vendorBidsQuery = await _context.VendorBids.Where(x => x.ProcurementPlanId == procurementPlanId && x.Type == Domain.Enums.EVendorContractStatus.NOTSTARTED).ToListAsync();

            return vendorBidsQuery;
        }

        public async Task<VendorBid> GetVendorBid(Guid vendorId, Guid procurementPlanId)
        {
            var vendorBidsQuery = await _context.VendorBids.SingleOrDefaultAsync(x => x.VendorId == vendorId && x.ProcurementPlanId == procurementPlanId);

            return vendorBidsQuery;
        }

    }
}
