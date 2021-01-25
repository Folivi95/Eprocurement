using EGPS.Application.Interfaces;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Repository
{
    public class VendorDirectorCertificateRepository : Repository<VendorDirectorCertificate>, IVendorDirectorCertificateRepository
    {
        public VendorDirectorCertificateRepository(EDMSDBContext context): base(context)
        {

        }
    }
}
