using System.Linq;
using System.Threading.Tasks;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EGPS.Application.Repository
{
    public class StateRepository : Repository<State>, IStateRepository
    {
        public StateRepository(EDMSDBContext context)
            : base(context)
        {
 
        }

        public Task<PagedList<State>> GetAllStates(StateParameter parameters)
        {
            var statesQuery = _context.States.AsNoTracking();
            var states = PagedList<State>.Create(statesQuery, parameters.PageNumber, parameters.PageSize);

            return states;
        }
    }
}
