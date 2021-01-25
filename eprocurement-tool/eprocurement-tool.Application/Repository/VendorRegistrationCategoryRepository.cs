using EGPS.Domain.Entities;
using EGPS.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Infrastructure.Data.Context;
using System.Threading.Tasks;
using EGPS.Application.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EGPS.Application.Repository
{
    public class VendorRegistrationCategoryRepository : Repository<VendorRegistrationCategory>, IVendorRegistrationCategoryRepository
    {
        public VendorRegistrationCategoryRepository(EDMSDBContext context) : base(context)
        {

        }

        public Task<PagedList<VendorRegistrationCategory>> GetRegistrationCategoryByDate(Guid userId, RegistrationCategoryParameter parameter)
        {
            var vendorRegistrationCategoryQuery = _context.VendorRegistrationCategories.Where(x => x.UserId == userId).OrderByDescending(x => x.CreateAt).Include(x => x.RegistrationPlan);

            var vendorRegistrationCategory = PagedList<VendorRegistrationCategory>.Create(vendorRegistrationCategoryQuery, parameter.PageNumber, parameter.PageSize);

            return vendorRegistrationCategory;
        }
    }
}
