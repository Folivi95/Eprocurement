using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IVendorDirectorRepository : IRepository<VendorDirector>
    {
        Task<VendorDirector> GetVendorDirectorWithCertificates(Guid userId, Guid directorId);
    }
}
