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
            var user = await DataContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            
            if(user is PowerUser)
            {
                return await DataContext.PowerUsers
                                    .Include(u => u.Memberships)
                                    .Include(u => u.Photos)
                                    .Include(u => u.City)
                                    .Include(u => u.Country)
                                    .Include(u => u.GroupsCreated)
                                    .Include(u => u.MatchStatuses)
                                    .FirstOrDefaultAsync(u => u.Id == id);
            }

            return await DataContext.CommonUsers
                                    .Include(u => u.Memberships)
                                    .Include(u => u.Photos)
                                    .Include(u => u.City)
                                    .Include(u => u.Country)
                                    .Include(u => u.MatchStatuses)
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

        public async Task<ICollection<User>> GetUsers()
        {
            var users = await DataContext.Users.Include(u => u.Photos).ToListAsync();

            return users;
        }

        public DataContext DataContext
        {
            get { return Context as DataContext; } 
        }

    }
}