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
                                        .Include(m => m.Group)
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

        public async Task<ICollection<MatchStatus>> GetUpcomingMatchesForUser(int userId)
        {
            var upcomingMatches = await DataContext.MatchStatuses
                                                    .Where(m => m.UserId == userId && m.Matchday.DatePlaying > DateTime.Now)
                                                    .Include(m => m.Matchday)
                                                    .ThenInclude(m => m.MatchStatuses)
                                                    .Include(m => m.Matchday)
                                                    .ThenInclude(m => m.Location)
                                                    .ThenInclude(l => l.City)
                                                    .ThenInclude(l => l.Country)
                                                    .OrderBy(m => m.Matchday.DatePlaying)
                                                    .ToListAsync();
            return upcomingMatches;
        }

        public async Task<ICollection<Matchday>> GetUpcomingMatchesApplicableForUser(int userId)
        {
            var upcomingMatches = await DataContext.Matchdays
                                                    .Include(m => m.Group)
                                                    .ThenInclude(g => g.Memberships)
                                                    .Where(m =>
                                                     m.Group.Memberships.FirstOrDefault(u => u.UserId == userId) != null && // This line is related to user participation in group
                                                     m.MatchStatuses.FirstOrDefault(ms => ms.UserId == userId) == null &&   // This line is related to user not having match status
                                                     m.DatePlaying > DateTime.Now                                           // This line ensures that match is hasn't been played
                                                    )
                                                    .Include(m => m.Location)
                                                    .ThenInclude(l => l.City)
                                                    .ThenInclude(l => l.Country)
                                                    .Include(m => m.MatchStatuses)
                                                    .OrderBy(m => m.DatePlaying)
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