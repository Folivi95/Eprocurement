using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EGPS.Application.Repository
{
    public class UnitRepository: Repository<Unit>, IUnitRepository
    {
        public UnitRepository(EDMSDBContext context): base(context)
        {
            
        }

        public async Task AddUnitMemeber(Guid userId, Guid unitId)
        {
            var unitMemeber = new UnitMember
            {
                UnitId = unitId,
                UserId = userId
            };
            await _context.UnitMembers.AddAsync(unitMemeber);
        }

        public Task<PagedList<UnitsDTO>> GetUnits(UnitParameters parameters, Guid accountId)
        {
            var query = _context.Units as IQueryable<Unit>;
            if (!string.IsNullOrEmpty(parameters.Search))
            {
                var search = parameters.Search.Trim();
                query = query.Where(x => x.Name.ToLower().Contains(search.ToLower()));
            }

            if (parameters.DepartmentId != null)
            {
                query = query.Where(x => x.DepartmentId == parameters.DepartmentId);
            }

            query = query.Where(x => x.AccountId == accountId && !x.Department.Deleted)
                         .Include(x => x.UnitMembers)
                         .ThenInclude(x => x.User)
                         .OrderByDescending(d => d.CreateAt);
            var unitQuery = query.Where(x => x.AccountId == accountId)
                                 .Select(x => new UnitsDTO
                                 {
                                     Id = x.Id,
                                     Name = x.Name,
                                     LeadId = x.LeadId,
                                     Description = x.Description,
                                     Website = x.Website,
                                     AccountId = x.AccountId,
                                     CreatedAt = x.CreateAt,
                                     CreatedById = x.CreatedById,
                                     Deleted = x.Deleted,
                                     DeletedAt = x.DeletedAt,
                                     DepartmentId = x.DepartmentId,
                                     UpdatedAt = x.UpdatedAt,
                                     Members = x.UnitMembers.Select(u => new UnitUserDTO
                                     {
                                         Id = u.User.Id,
                                         FirstName = u.User.FirstName,
                                         LastName = u.User.LastName,
                                         ProfilePicture = string.IsNullOrEmpty(u.User.ProfilePicture)
                                             ? null
                                             : JsonConvert.DeserializeObject(u.User.ProfilePicture)
                                     }).ToList(),
                                     TotalMembers = x.UnitMembers.Select(x => x.UnitId == x.Id).Count()
                                 });

            var units = PagedList<UnitsDTO>.Create(unitQuery, parameters.PageNumber, parameters.PageSize);

            return units;
        }

        public async Task<bool> UserExistInUnit(Guid userId, Guid unitId)
        {
            return await _context.UnitMembers.AnyAsync(d => d.UserId == userId && d.UnitId == unitId);
        }
        public void RemoveUserFromUnit(UnitMember unitMember)
        {
            _context.UnitMembers.Remove(unitMember);
        }

        public async Task<UnitMember> GetUnitMember(Guid unitId, Guid userId)
        {
            var unitMember = await _context.UnitMembers.SingleOrDefaultAsync(d => d.UserId == userId && d.UnitId == unitId);
            return unitMember;
        }

        public async Task<PagedList<User>> GetMembers(Guid unitId, UnitMembersParameter parameters)
        {
            var query = _context
                        .UnitMembers
                        .Join(_context.Users, unit => unit.UserId, user => user.Id,
                                      (unit, user) => new { unit, user })
                        .Where(x => x.unit.UnitId == unitId);

            var users = query.Select(x => x.user);
            if (!string.IsNullOrEmpty(parameters.Search))
            {
                var search = parameters.Search.Trim();
                users = users.Where(x => x.FirstName.ToLower().Contains(search.ToLower()) ||
                                         x.LastName.ToLower().Contains(search.ToLower()));
            }
            var userPageList = await PagedList<User>.Create(users, parameters.PageNumber, parameters.PageSize);

            return userPageList;
        }

        public async Task<int> GetMembersCount(Guid unitId)
        {
            var count = await _context.UnitMembers.Where(x => x.UnitId == unitId).CountAsync();

            return count;
        }
    }
}
