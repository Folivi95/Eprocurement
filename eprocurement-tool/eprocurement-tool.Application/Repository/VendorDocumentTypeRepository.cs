using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Repository
{
    public class VendorDocumentTypeRepository : Repository<VendorDocumentType>, IVendorDocumentTypeRepository
    {
        public VendorDocumentTypeRepository(EDMSDBContext context) : base(context)
        {

        }

        public Task<PagedList<VendorDocumentType>> GetAllVendorDocumentTypes(VendorDocumentTypeParameter parameter)
        {
            var vendorDocumentTypesQuery = _context.VendorDocumentTypes.AsQueryable();

            var vendorDocumentTypes = PagedList<VendorDocumentType>.Create(vendorDocumentTypesQuery, parameter.PageNumber, parameter.PageSize);

            return vendorDocumentTypes;
        }
    }
}
