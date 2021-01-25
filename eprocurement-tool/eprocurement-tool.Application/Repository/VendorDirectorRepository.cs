using EGPS.Application.Interfaces;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;


namespace EGPS.Application.Repository
{
    public class VendorDirectorRepository : Repository<VendorDirector>, IVendorDirectorRepository
    {
        public VendorDirectorRepository(EDMSDBContext context)
            :base(context)
        {

        }

        public Task<VendorDirector> GetVendorDirectorWithCertificates(Guid userId, Guid directorId)
        {
            var vendorDirectors = _context.VendorDirectors.Where(x => x.UserId == userId && x.Id == directorId)
                .Include(x => x.Certifications)
                .AsNoTracking().FirstOrDefaultAsync();

            return vendorDirectors;
        }
    }
}
