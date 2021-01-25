using EGPS.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IVendorCorrespondenceRepository : IRepository<VendorCorrespondence>
    {
        Task<VendorCorrespondence> GetVendorCorrespondence(Guid id);
    }
}
