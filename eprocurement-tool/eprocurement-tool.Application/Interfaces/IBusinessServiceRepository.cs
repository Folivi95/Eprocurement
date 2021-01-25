using System.Threading.Tasks;
using EGPS.Application.Models;
using EGPS.Domain.Entities;

namespace EGPS.Application.Interfaces
{
    public interface IBusinessServiceRepository : IRepository<BusinessService>
    {
         Task<PagedList<BusinessService>> GetAllBusinessServices(BusinessServicesParameter parameters);
    }
}
