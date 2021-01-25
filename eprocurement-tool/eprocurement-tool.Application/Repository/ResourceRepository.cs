using EGPS.Application.Interfaces;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Repository
{
    public class ResourceRepository : Repository<Resource>, IResourceRepository
    {
        public ResourceRepository(EDMSDBContext context)
            : base(context)
        {

        }

        public async Task<IEnumerable<Resource>> GetResources()
        {
            return await _context.Resources.ToListAsync();
        }

        public async Task<IEnumerable<Resource>> GetResources(string name, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetResources();
            }

            var resources = _context.Resources as IQueryable<Resource>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                resources = resources.Where(n => n.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                resources = resources.Where(s => s.Name.Contains(searchQuery));
            }

            return await resources.ToListAsync();
        }
    }
}
