using EGPS.Application.Interfaces;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EGPS.Application.Repository
{
    public class VendorCorrespondenceRepository : Repository<VendorCorrespondence>, IVendorCorrespondenceRepository
    {
        public VendorCorrespondenceRepository(EDMSDBContext context)
            : base(context)
        {

        }

        public async Task<VendorCorrespondence> GetVendorCorrespondence(Guid id)
        {
            return await _context.VendorCorrespondences.FirstOrDefaultAsync(vc => vc.UserID == id);
        }
    }
}
