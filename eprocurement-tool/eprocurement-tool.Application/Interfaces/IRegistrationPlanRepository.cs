using System;
using System.Threading.Tasks;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;

namespace EGPS.Application.Interfaces
{
    public interface IRegistrationPlanRepository : IRepository<RegistrationPlan>
    {
         Task<PagedList<RegistrationPlan>> GetAllRegistrationCategories(RegistrationPlanParameter parameters);

         Task<PagedList<RegistrationPlan>> GetRegistrationCategoryForVendor(Guid userId, RegistrationPlanParameter parameter);
    }
}
