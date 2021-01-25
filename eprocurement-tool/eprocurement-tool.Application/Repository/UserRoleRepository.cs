using EGPS.Application.Interfaces;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Repository
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(EDMSDBContext context)
            : base(context)
        {

        }
    }
}
