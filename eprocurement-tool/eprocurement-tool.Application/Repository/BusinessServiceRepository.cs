using System.Linq;
using System.Threading.Tasks;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EGPS.Application.Repository
{
    public class BusinessServiceRepository : Repository<BusinessService>, IBusinessServiceRepository
    {
        public BusinessServiceRepository(EDMSDBContext context)
            : base(context)
        {
 
        }

        public Task<PagedList<BusinessService>> GetAllBusinessServices(BusinessServicesParameter parameters)
        {
            //get business services using the userId supplied in the API request
            var businessServicesQuery = _context.BusinessServices.Include(c => c.BusinessCategory).AsNoTracking();
            //return business services.
            var businessServices = PagedList<BusinessService>.Create(businessServicesQuery, parameters.PageNumber, parameters.PageSize);
            return businessServices;
        }

    }
}
