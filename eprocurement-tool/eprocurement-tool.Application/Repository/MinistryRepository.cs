using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EGPS.Application.Repository
{
    public class MinistryRepository : Repository<Ministry>, IMinistryRepository
    {
        public MinistryRepository(EDMSDBContext context) : base(context)
        {

        }



        public Task<PagedList<Ministry>> GetAllMinistriesByUserId(MinistryParameters parameter, Guid userId)
        {
            var query = _context.Ministries as IQueryable<Ministry>;

            //check if parameter values are null or empty and add them to query if they aren't
            if (!string.IsNullOrEmpty(parameter.name))
            {
                var name = parameter.name.Trim();
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }

            if (parameter.code != null)
            {
                query = query.Where(x => x.Code == parameter.code);
            }

            if (parameter.estimatedValueId != null)
            {
                query = query.Where(x => x.EstimatedValueId == parameter.estimatedValueId);
            }

            if (parameter.bidLowerThanId != null)
            {
                query = query.Where(x => x.BidLowerThanId == parameter.bidLowerThanId);
            }




            var ministryQuery = query.Where(x => !x.Deleted);

            var ministries = PagedList<Ministry>.Create(ministryQuery, parameter.PageNumber, parameter.PageSize);

            return ministries;
        }

        public async Task<PagedList<VendorProfile>> GetVendors(Guid ministryId, VendorParameters parameter)
        {

            var query = _context.VendorProfiles.Where(a => a.User.MinistryId == ministryId);
            if (!string.IsNullOrEmpty(parameter.Name))
            {
                query = query.Where(a => a.User.FirstName.ToLower()
                                             .Contains(parameter.Name)
                                         || a.User.LastName.ToLower()
                                             .Contains(parameter.Name
                                             )
                                         || a.CompanyName.ToLower()
                                             .Contains(parameter.Name
                                             )
                          );
            }

            if (!string.IsNullOrEmpty(parameter.RegisterId))
            {
                query = query.Where(a => a.CACRegistrationNumber.ToLower()
                        .Contains(parameter.RegisterId))
                    ;
            }
            var ministries = await PagedList<VendorProfile>.Create(query, parameter.PageNumber, parameter.PageSize);
            return ministries;
        }

        public async Task<PagedList<User>> GetUsersByMinistry(Guid ministryId, ResourceParameters parameter)
        {
            var query = _context.Users.Where(a => a.MinistryId == ministryId)
                    .Include(a => a.VendorProfile)

                ;
            var users = await PagedList<User>.Create(query, parameter.PageNumber, parameter.PageSize);
            return users;
        }


        public async Task<PagedList<MinistryDTO>> TotalBidsForMinistry(IEnumerable<MinistryDTO> ministries, MinistryParameters parameter)
        {
            var bidQuery = _context.VendorBids as IQueryable<VendorBid>;
            List<MinistryDTO> ministriesDto = new List<MinistryDTO>();

            foreach (var ministry in ministries)
            {
                var query = _context.ProcurementPlans as IQueryable<ProcurementPlan>;

                query = query.Where(p => p.MinistryId == ministry.Id);
                int bidCount = 0;
                double estimatedValue = 0;

                foreach (var procurementPlan in query)
                {
                    var count = bidQuery.Where(p => p.ProcurementPlanId == procurementPlan.Id).CountAsync();
                    estimatedValue = estimatedValue + procurementPlan.EstimatedAmountInNaira;
                    bidCount = bidCount + await count;
                }
                ministry.TotalBids = bidCount;
                ministry.EstimatedValue = estimatedValue;

                if (parameter.estimatedValue != null)
                {
                    if (ministry.EstimatedValue >= parameter.estimatedValue)
                    {
                        ministriesDto.Add(ministry);
                    }
                    continue;
                }
                ministriesDto.Add(ministry);
            }

            return new PagedList<MinistryDTO>(ministriesDto, ministriesDto.Count, parameter.PageNumber, parameter.PageSize);
        }
    }
}
