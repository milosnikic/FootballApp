using System;
using System.Collections.Generic;
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

        public async Task<Matchday> GetMatchdayWithAdditionalInformation(int matchId)
        {
            var match = await DataContext.Matchdays
                                        .Include(m => m.Location)
                                        .ThenInclude(l => l.City)
                                        .ThenInclude(l => l.Country)
                                        .Include(m => m.MatchStatuses)
                                        .ThenInclude(m => m.User)
                                        .ThenInclude(u => u.Photos)
                                        .FirstOrDefaultAsync(m => m.Id == matchId);
            return match;
        }

        public async Task<ICollection<Matchday>> GetUpcomingMatchesForGroup(int groupId)
        {
            var upcomingMatches = await DataContext.Matchdays
                                                   .Where(m => m.Group.Id == groupId && m.DatePlaying > DateTime.Now)
                                                   .Include(m => m.Location)
                                                   .ThenInclude(l => l.City)
                                                   .ThenInclude(l => l.Country)
                                                   .Include(m => m.MatchStatuses)
                                                   .OrderBy(m => m.DatePlaying)
                                                   .OrderBy(m => m.Location.CountryId)
                                                   .ToListAsync();
            return upcomingMatches;
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