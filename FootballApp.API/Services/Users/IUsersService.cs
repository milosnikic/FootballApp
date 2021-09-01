using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Dtos;
using FootballApp.API.Models;

namespace FootballApp.API.Services.Users
{
    public interface IUsersService
    {
        Task<KeyValuePair<bool, string>> UpdateUser(int userId, UserToUpdateDto userToUpdateDto);
        Task<UserToReturnDto> GetUser(int id);
        Task<IEnumerable<UserToReturnDto>> GetAllUsers();
        Task<KeyValuePair<bool, string>> VisitUser(VisitUserDto visitUserDto);
        Task<IEnumerable<VisitToReturnDto>> GetLatestFiveVisitorsForUser(int userId);
        Task<KeyValuePair<bool, string>> GainAchievement(int userId, GainedAchievementForCreationDto gainedAchievementForCreationDto);
        Task<IEnumerable<GainedAchievementToReturnDto>> GetAllAchievementsForUser(int userId);
        Task<IEnumerable<Achievement>> GetAllAchievements();
    }
}