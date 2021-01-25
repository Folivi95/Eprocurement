using EGPS.Application.Interfaces;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;

namespace EGPS.Application.Repository
{
    public class VendorContactRepository : Repository<VendorContact>, IVendorContactRepository
    {
        public VendorContactRepository(EDMSDBContext context)
            : base(context)
        {

        }
    }
}
