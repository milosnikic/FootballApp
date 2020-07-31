using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Locations
{
    public class LocationsRepository : Repository<Location>, ILocationsRepository
    {
        public LocationsRepository(DataContext context)
            : base(context)
        {

        }

        public async Task<ICollection<Location>> GetAllLocationsWithInclude()
        {
            var locations = await DataContext.Locations
                                       .Include(l => l.City)
                                       .Include(l => l.Country)
                                       .ToListAsync();
            return locations;
        }

        public async Task<Location> GetByName(string name)
        {
            var location = await DataContext.Locations.Where(l => l.Name == name).FirstOrDefaultAsync();
            return location;
        }

        public DataContext DataContext
        {
            get
            { return Context as DataContext; }
        }

    }
}