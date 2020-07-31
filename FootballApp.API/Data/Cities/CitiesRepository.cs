using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Cities
{
    public class CitiesRepository : Repository<City>, ICitiesRepository
    {
        public CitiesRepository(DataContext context)
            : base(context)
        {

        }

        public DataContext DataContext
        {
            get
            {
                return Context as DataContext;
            }
        }

        public async Task<ICollection<City>> GetAllCitiesForCountry(int id)
        {
            var cities = await DataContext.Cities.Where(c => c.CountryId == id).ToListAsync();
            return cities;
        }
    }
}