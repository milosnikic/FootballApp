using FootballApp.API.Models;

namespace FootballApp.API.Data.Locations
{
    public class LocationsRepository : Repository<Location>, ILocationsRepository
    {
        public LocationsRepository(DataContext context)
            : base(context)
        {

        }

        public DataContext DataContext
        {
            get
            { return Context as DataContext; }
        }
    }
}