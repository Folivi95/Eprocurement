using System.Threading.Tasks;
using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EGPS.Application.Repository
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(EDMSDBContext context)
            : base(context)
        {
            
        }

        public Task<PagedList<Country>> GetAllCountries(CountryParameter parameters)
        {
            var countriesQuery = _context.Countries.AsNoTracking();
            var Countries = PagedList<Country>.Create(countriesQuery, parameters.PageNumber, parameters.PageSize);

            return Countries;
        }
    }
}
