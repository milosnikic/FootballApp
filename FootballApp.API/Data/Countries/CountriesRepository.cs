using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Countries
{
    public class CountriesRepository : Repository<Country>, ICountriesRepository
    {
        public CountriesRepository(DataContext context)
            : base(context)
        {

        }

        public async Task<bool> Exists(string name)
        {
            return await DataContext.Countries.Where(c => c.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync() != null;
        }

        public async Task<ICollection<Country>> GetAllCountriesWithCities()
        {
            var countries = await DataContext.Countries
                                       .Include(c => c.Cities)
                                       .ToListAsync();
            return countries;
        }

        public DataContext DataContext
        {
            get
            {
                return Context as DataContext;
            }
        }
    }
}