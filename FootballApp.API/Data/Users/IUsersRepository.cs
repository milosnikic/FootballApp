using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<User> GetUserByIdWithAdditionalInformation(int id);
        void VisitUser(Visit visit);
        void GainAchievement(GainedAchievement gainedAchievement);
        Task<ICollection<Visit>> GetLatestFiveVisitorsForUser(int userId);
        Task<ICollection<GainedAchievement>> GetAllAchievementsForUser(int userId);
        Task<ICollection<User>> GetUsers();
    }
}