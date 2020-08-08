using System.Threading.Tasks;
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

        public async Task<MatchStatus> GetMatchStatusById(int userId, int matchId)
        {
            return await DataContext.MatchStatuses.FirstOrDefaultAsync(m => m.UserId == userId && m.MatchdayId == matchId);
        }
    }
}