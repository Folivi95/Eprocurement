using EGPS.Application.Interfaces;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;

namespace EGPS.Application.Repository
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(EDMSDBContext context)
            : base(context)
        {
 
        }
    }
}
