using EGPS.Domain.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IResourceRepository : IRepository<Resource>
    {
        Task<IEnumerable<Resource>> GetResources();
        Task<IEnumerable<Resource>> GetResources(string name, string searchQuery);
    }
}
