using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<PagedList<DepartmentsDTO>> GetDepartments(DepartmentParameters parameters, Guid accountId);
        Task<bool> UserExistInDepartment(Guid userId, Guid departmentId);
        Task AddDepartmentMemeber(Guid userId, Guid departmentId);
        void RemoveUserFromDepartment(DepartmentMember departmentMember);
        Task<DepartmentMember> GetDepartmentMember(Guid departmentId, Guid userId);
        Task<PagedList<User>> GetMembers(Guid departmentId, DepartmentMembersParameter parameters);
        Task<int> GetMembersCount(Guid departmentId);
    }
}
