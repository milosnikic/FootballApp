using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.MatchStatuses
{
    public class MatchStatusesRepository : Repository<MatchStatus>, IMatchStatusesRepository
    {
        public MatchStatusesRepository(DbContext context) : base(context)
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