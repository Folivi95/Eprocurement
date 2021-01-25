using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EGPS.Application.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(EDMSDBContext context)
            : base(context)
        {

        }

        public Task<PagedList<DepartmentsDTO>> GetDepartments(DepartmentParameters parameters, Guid accountId)
        {
            var query = _context.Departments as IQueryable<Department>;

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                var search = parameters.Search.Trim();
                query = query.Where(x => x.Name.ToLower().Contains(search.ToLower()));
            }
           

            var departmentQuery = query.Where(x => x.AccountId == accountId)
                         .Select(x => new DepartmentsDTO
                         {
                             Id = x.Id,
                             Name = x.Name,
                             LeadId = x.LeadId,
                             Description = x.Description,
                             Website = x.Website,
                             DeletedAt = x.DeletedAt,
                             AccountId = x.AccountId,
                             CreatedById = x.CreatedById,
                             Deleted = x.Deleted,
                             TotalMembers = x.DepartmentMembers.Select(x => x.DepartmentId == x.Id).Count(),
                             CreatedAt = x.CreateAt,
                             UpdatedAt = x.UpdatedAt,
                             Members = x.DepartmentMembers.Select(u => new DepartmentUserDTO()
                             {
                                 Id = u.User.Id,
                                 FirstName = u.User.FirstName,
                                 LastName = u.User.LastName,
                                 ProfilePicture = string.IsNullOrEmpty(u.User.ProfilePicture)
                                     ? null
                                     : JsonConvert.DeserializeObject(u.User.ProfilePicture)
                             }).ToList()
                         });

            var departments = PagedList<DepartmentsDTO>.Create(departmentQuery, parameters.PageNumber, parameters.PageSize);

            return departments;
        }

        public async Task<bool> UserExistInDepartment(Guid userId, Guid departmentId)
        {
            return await _context.DepartmentMembers.AnyAsync(d => d.UserId == userId && d.DepartmentId == departmentId);
        }

        public async Task AddDepartmentMemeber(Guid userId, Guid departmentId)
        {
            var departmentMember = new DepartmentMember
            {
                DepartmentId = departmentId,
                UserId = userId
            };
            await _context.DepartmentMembers.AddAsync(departmentMember);
        }
        public void RemoveUserFromDepartment(DepartmentMember departmentMember)
        {
           _context.DepartmentMembers.Remove(departmentMember);
        }

        public async Task<DepartmentMember> GetDepartmentMember(Guid departmentId, Guid userId)
        {
            var departmentMember = await _context.DepartmentMembers.SingleOrDefaultAsync(d => d.UserId == userId && d.DepartmentId == departmentId);
            return departmentMember;
        }

        public async Task<PagedList<User>> GetMembers(Guid departmentId, DepartmentMembersParameter parameters)
        {
            var query = _context
                        .DepartmentMembers
                        .Join(_context.Users, d => d.UserId, user => user.Id, 
                                      (d, user) => new { d, user })
                        .Where(x => x.d.DepartmentId == departmentId);

            var users = query.Select(x => x.user);
            if (!string.IsNullOrEmpty(parameters.Search))
            {
                var search = parameters.Search.Trim();
                users = users.Where(x => x.FirstName.ToLower().Contains(search.ToLower()) ||
                                         x.LastName.ToLower().Contains(search.ToLower()));
            }
            users = users.Include(x => x.UnitMembers)
                              .ThenInclude(x => x.Unit);
            var user = await PagedList<User>.Create(users, parameters.PageNumber, parameters.PageSize);

            return user;
        }

        public async Task<int> GetMembersCount(Guid departmentId)
        {
            var count = await _context.DepartmentMembers.Where(x => x.DepartmentId == departmentId).CountAsync();

            return count;

        }
    }
}
