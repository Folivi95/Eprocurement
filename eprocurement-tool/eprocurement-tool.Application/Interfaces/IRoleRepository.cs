using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<IEnumerable<Role>> GetRoles();
        void UpdateRoleResource(RoleResource resource);
        Task<RoleResource> GetResourceById(Guid resourceId);
        Task AddRoleResource(RoleResource resource);
        Task<Resource> ResourceExists(Guid resourceId);
        Task<Role> GetRoleById(Guid roleId, Guid accountId);
    }
}
