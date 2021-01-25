using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Repository
{
    public class UserInvitationRepository: Repository<UserInvitation>, IUserInvitationRepository
    {
        public UserInvitationRepository(EDMSDBContext context) : base(context) { }

        public async Task<PagedList<UserInvitation>> GetPendingInvites(ResourceParameters parameters)
        {
            var query = _context.UserInvitations as IQueryable<UserInvitation>;

            var userInvitationsQuery = query.Where(x => x.Status == true).Include(x => x.Account).DefaultIfEmpty();

            var userInvitations = await PagedList<UserInvitation>.Create(userInvitationsQuery, parameters.PageNumber, parameters.PageSize);

            return userInvitations;
        }
    }
}
