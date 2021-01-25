using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGPS.Application.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(EDMSDBContext context)
            : base(context)
        {

        }

        public async Task CreateUserInvitation(UserInvitation userInvitation)
        {
            await _context.UserInvitations.AddAsync(userInvitation);
        }

        public async Task<UserInvitation> GetUserInvitation(string email, string token)
        {
            return await _context.UserInvitations.FirstOrDefaultAsync(x => x.Email == email
                && x.Token == token
                && x.Status);
        }

        public void UpdateUserInvitation(UserInvitation userInvitation)
        {
            // No implementation
        }

        public async Task<PagedList<User>> GetUsers(UserPageModel model, Guid accountId)
        {
            var query = _context.Users as IQueryable<User>;

            if (!string.IsNullOrEmpty(model.Search))
            {
                var search = model.Search.Trim();
                query = query.Where(x => x.FirstName.ToLower().Contains(search.ToLower()) ||
                                         x.LastName.ToLower().Contains(search.ToLower()));
            }

            if (model.userIds != null && model.userIds.Length > 0)
            {
                query = query.Where(x => model.userIds.Contains(x.Id));
            }

            query = query.Where(u => u.AccountId == accountId)
                         .Include(u => u.DepartmentMembers).ThenInclude(u => u.Department)
                         .Include(u => u.UnitMembers).ThenInclude(u => u.Unit)
                         .Include(u => u.UserRoles).ThenInclude(u => u.Role)
                         .OrderByDescending(u => u.CreateAt);
            var users = await PagedList<User>.Create(query, model.PageNumber, model.PageSize);
            return users;
        }

        public async Task CreatePasswordReset(PasswordReset passwordReset)
        {
            await _context.PasswordResets.AddAsync(passwordReset);
        }

        public async Task<PasswordReset> GetUserResetPassword(UserResetPasswordForCreationDTO userResetPassword)
        {
            return await _context.PasswordResets.FirstOrDefaultAsync(
                x => x.Email == userResetPassword.Email
                && x.Token == userResetPassword.Token
                && x.Status);
        }

        public void UpdateUserResetPassword(PasswordReset passwordReset)
        {
            // No implementation
        }

        public async Task<string[]> ActiveUsersAsync(string[] emails, Guid accountId)
        {
            var users = await _context.Users
                .Where(x => x.AccountId == accountId && emails.Contains(x.Email) && x.Status == EStatus.ENABLED)
                .Select(c => c.Email)
                .ToArrayAsync();

            return users;
        }

        public async Task<string[]> PendingUsersAsync(string[] emails, Guid accountId)
        {
            var users = await _context.Users
                .Where(x => x.AccountId == accountId && emails.Contains(x.Email) && x.Status == EStatus.DISABLED)
                .Select(c => c.Email)
                .ToArrayAsync();

            return users;
        }

        public async Task AddUsers(IEnumerable<User> users)
        {
            await _context.Users.AddRangeAsync(users);
        }

        public async Task CreateMultipleUsersInvitation(IList<UserInvitation> userInvitations)
        {
            await _context.UserInvitations.AddRangeAsync(userInvitations);
        }

        public async Task<User> GetUserById(Guid userId, Guid accountId)
        {
            var user = await _context.Users
                .Where(x => x.AccountId == accountId && x.Id == userId)
                .Include(u => u.DepartmentMembers).ThenInclude(u => u.Department)
                .Include(u => u.UnitMembers).ThenInclude(u => u.Unit)
                .Include(u => u.UserRoles).ThenInclude(u => u.Role)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> IsVendor(Guid userId)
        {
            bool isVendor = await _context.Users.Where(x => x.Id == userId && x.UserType == EUserType.VENDOR).AsNoTracking().AnyAsync();

            return isVendor;
        }

        public async Task<User> GetUserDetail(string email)
        {
            var user =await _context.Users.Where(x => x.Email == email)
                                    .Include(u => u.UserRoles)
                                    .ThenInclude(u => u.Role).SingleOrDefaultAsync();
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async  Task<PagedList<User>> GetUsers( ResourceParameters parameter, string search="")
        {
            var query = _context.Users           
                .OrderByDescending(a => a.CreateAt)
                .Include(a => a.VendorProfile).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.FirstName.ToLower().Contains(search.ToLower())
                                         || a.LastName.ToLower().Contains(search.ToLower()));
            }
            var users = await PagedList<User>.Create(query, parameter.PageNumber, parameter.PageSize);
            return users;
        } 


        public async Task<IEnumerable<User>> GetCheckers(Guid ministryId)
        {
            var users  = await _context.Users.Where(x => x.MinistryId == ministryId && (x.Role == ERole.COMMISSIONER || x.Role ==  ERole.EXECUTIVE || x.Role == ERole.PERMANENTSECRETARY))
                                    .Include(x => x.Ministry).ToListAsync();
            return users;
        }
    }
}
