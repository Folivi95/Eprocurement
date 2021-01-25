using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EGPS.Application.Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(EDMSDBContext context)
            : base(context)
        {

        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            var roles =  await _context.Roles.ToListAsync();

            return roles;
        }

        public async Task<Role> GetRoleById(Guid roleId, Guid accountId)
        {
            var role = await _context.Roles
                .Where(x => x.AccountId == accountId && x.Id == roleId)
                .Include(x => x.Resources)
                .ThenInclude(x => x.Resource)
                .FirstOrDefaultAsync();

            return role;
        }

        public void UpdateRoleResource(RoleResource resource)
        {
            _context.RoleResources.Update(resource);
        }

        public async Task<RoleResource> GetResourceById(Guid resourceId)
        {
            var query = _context.RoleResources as IQueryable<RoleResource>;
            query = query.Where(x => x.ResourceId == resourceId).Include(x => x.Resource);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Resource> ResourceExists(Guid resourceId)
        {
            return await _context.Resources.Where(x => x.Id == resourceId).FirstOrDefaultAsync();
        }

        public async Task AddRoleResource(RoleResource resource)
        {
            await _context.RoleResources.AddAsync(resource);
        }
    }
}
