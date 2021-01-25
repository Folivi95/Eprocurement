using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        void UpdateUserInvitation(UserInvitation userInvitation);
        Task<UserInvitation> GetUserInvitation(string email, string token);
        Task CreateUserInvitation(UserInvitation userInvitation);
        Task CreateMultipleUsersInvitation(IList<UserInvitation> userInvitations);
        Task<PagedList<User>> GetUsers(UserPageModel model, Guid accountId);
        Task CreatePasswordReset(PasswordReset passwordReset);
        Task<PasswordReset> GetUserResetPassword(UserResetPasswordForCreationDTO userResetPassword);
        void UpdateUserResetPassword(PasswordReset passwordReset);
        Task<string[]> ActiveUsersAsync(string[] emails, Guid accountId);
        Task<string[]> PendingUsersAsync(string[] emails, Guid accountId);
        Task AddUsers(IEnumerable<User> users);
        Task<User> GetUserById(Guid userId, Guid accountId);
        Task<bool> IsVendor(Guid userId);
        Task<User> GetUserDetail(string email);

        Task<PagedList<User>> GetUsers(ResourceParameters parameter, string search = "");
        Task<IEnumerable<User>> GetCheckers(Guid ministryId);
    }
}
