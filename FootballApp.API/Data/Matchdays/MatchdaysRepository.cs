using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Matchdays
{
    public class MatchdaysRepository : Repository<Matchday>, IMatchdaysRepository
    {
        public MatchdaysRepository(DataContext context)
            : base(context)
        {

        }

        public async Task<Matchday> GetMatchdayWithLocation(int matchId)
        {
            var match = await DataContext.Matchdays
                                        .Include(m => m.Location)
                                        .ThenInclude(l => l.City)
                                        .ThenInclude(l => l.Country)
                                        .FirstOrDefaultAsync(m => m.Id == matchId);
            return match;
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