using System;
using System.Linq;
using System.Threading.Tasks;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EGPS.Application.Repository
{
    public class RegistrationPlanRepository : Repository<RegistrationPlan>, IRegistrationPlanRepository
    {
        public RegistrationPlanRepository(EDMSDBContext context)
            : base(context)
        {

        }

        public Task<PagedList<RegistrationPlan>> GetAllRegistrationCategories(RegistrationPlanParameter parameters)
        {
            var registrationPlansQuery = _context.RegistrationPlans.AsQueryable();
            var registrationPlans = PagedList<RegistrationPlan>.Create(registrationPlansQuery, parameters.PageNumber, parameters.PageSize);

            return registrationPlans;
        }

        public Task<PagedList<RegistrationPlan>> GetRegistrationCategoryForVendor(Guid userId, RegistrationPlanParameter parameter)
        {
            var registrationCategoryForVendorQuery = _context.RegistrationPlans.Where(x => x.CreatedBy == userId);

            var registrationCategoryForVendor = PagedList<RegistrationPlan>.Create(registrationCategoryForVendorQuery, parameter.PageNumber, parameter.PageSize);

            return registrationCategoryForVendor;
        }
    }
}
