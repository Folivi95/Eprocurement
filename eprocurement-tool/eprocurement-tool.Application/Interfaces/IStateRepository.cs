using System.Threading.Tasks;
using EGPS.Application.Models;
using EGPS.Domain.Entities;

namespace EGPS.Application.Interfaces
{
    public interface IStateRepository : IRepository<State>
    {
         Task<PagedList<State>> GetAllStates(StateParameter parameters);
    }
}
