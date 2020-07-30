using FootballApp.API.Models;

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
    }
}