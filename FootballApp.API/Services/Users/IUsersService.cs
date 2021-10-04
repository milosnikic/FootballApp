using FootballApp.API.Dtos;
using FootballApp.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballApp.API.Services.Users
{
    public interface IUsersService
    {
        Task<KeyValuePair<bool, string>> UpdateUser(int userId, UserToUpdateDto userToUpdateDto);
        Task<UserToReturnDto> GetUser(int id);
        Task<ICollection<UserToReturnDto>> GetAllUsers();
        Task<KeyValuePair<bool, string>> VisitUser(VisitUserDto visitUserDto);
        Task<ICollection<VisitToReturnDto>> GetLatestFiveVisitorsForUser(int userId);
        Task<KeyValuePair<bool, string>> GainAchievement(int userId, GainedAchievementForCreationDto gainedAchievementForCreationDto);
        Task<ICollection<GainedAchievementToReturnDto>> GetAllAchievementsForUser(int userId);
        Task<ICollection<Achievement>> GetAllAchievements();
    }
}