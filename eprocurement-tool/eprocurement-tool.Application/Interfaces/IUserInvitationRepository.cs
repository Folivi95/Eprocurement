using EGPS.Application.Models;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IUserInvitationRepository: IRepository<UserInvitation>
    {
        Task<PagedList<UserInvitation>> GetPendingInvites(ResourceParameters parameters);
    }
}
