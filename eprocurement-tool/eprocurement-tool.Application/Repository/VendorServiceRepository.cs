using EGPS.Application.Interfaces;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;

namespace EGPS.Application.Repository
{
    public class VendorServiceRepository : Repository<VendorService>, IVendorServiceRepository
    {
        public VendorServiceRepository(EDMSDBContext context) : base(context)
        {

        }
    }
}
