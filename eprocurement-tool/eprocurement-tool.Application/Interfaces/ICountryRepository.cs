using System.Threading.Tasks;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;

namespace EGPS.Application.Interfaces
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<PagedList<Country>> GetAllCountries(CountryParameter parameters);
    }
}
