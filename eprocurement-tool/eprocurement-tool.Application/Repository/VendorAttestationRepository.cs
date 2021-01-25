using EGPS.Application.Interfaces;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;

namespace EGPS.Application.Repository
{
    public class VendorAttestationRepository : Repository<VendorAttestation>, IVendorAttestationRepository
    {
        public VendorAttestationRepository(EDMSDBContext context)
            : base(context)
        {

        }
    }
}
