using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Users
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {

        public UsersRepository(DataContext context)
            : base(context)
        {
        }

        public async Task<User> GetUserByIdWithAdditionalInformation(int id)
        {
            return await DataContext.Users
                                    .Include(u => u.Memberships)
                                    .Include(u => u.Photos)
                                    .Include(u => u.City)
                                    .Include(u => u.Country)
                                    .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async void VisitUser(Visit visit)
        {
            await DataContext.Visits
                             .AddAsync(visit);
        }

        public async Task<ICollection<Visit>> GetLatestFiveVisitorsForUser(int userId)
        {
            var latestFiveVisitors = await DataContext.Visits
                                                        .Where(v => v.VisitedId == userId)
                                                        .GroupBy(v => new { v.VisitorId, v.VisitedId })
                                                        .Select(v => new Visit {
                                                            VisitorId = v.FirstOrDefault().VisitedId,
                                                            Visitor = v.FirstOrDefault().Visitor,
                                                            VisitedId = userId,
                                                            Visited = v.FirstOrDefault().Visited,
                                                            DateVisited = v.Max(x => x.DateVisited)
                                                        })
                                                        .OrderByDescending(v => v.DateVisited)
                                                        .Include(v => v.Visitor)
                                                        .Take(5)
                                                        .ToListAsync();
  
            return latestFiveVisitors;
        }

        public async Task<ICollection<User>> GetAllExploreUsers(int userId)
        {
            var exploreUsers = await DataContext.Users
                                                .Where(u => u.Id != userId)
                                                .Include(u => u.Photos)
                                                .ToListAsync();
            return exploreUsers;
        }

        public async void GainAchievement(GainedAchievement gainedAchievement)
        {
            await DataContext.GainedAchievements
                             .AddAsync(gainedAchievement);
        }

        public async Task<ICollection<GainedAchievement>> GetAllAchievementsForUser(int userId)
        {
            var achievements = await DataContext.GainedAchievements
                                          .Where(g => g.UserId == userId)
                                          .Include(g => g.Achievement)
                                          .ToListAsync();
            return achievements;
        }

        public DataContext DataContext
        {
            get { return Context as DataContext; } 
        }

    }
}