using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Achievements
{
    public class AchievementsRepository : Repository<Achievement>, IAchievementsRepository
    {
        public AchievementsRepository(DataContext context)
            : base(context)
        {
        }
        public async Task<Achievement> GetAchievementByValue(int value)
        {
            return await DataContext.Achievements
                              .Where(a => a.Value == value)
                              .FirstOrDefaultAsync();
        }

        public async Task<GainedAchievement> GetGainedAchievement(int userId, int value)
        {
            var gainedAchievement = await DataContext.GainedAchievements
                                               .Where(g => g.UserId == userId && g.Achievement.Value == value)
                                               .FirstOrDefaultAsync();
            return gainedAchievement;
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