using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Achievements
{
    public interface IAchievementsRepository : IRepository<Achievement>
    {
        Task<Achievement> GetAchievementByValue(int value);
        Task<GainedAchievement> GetGainedAchievement(int userId, int value);
    }
}