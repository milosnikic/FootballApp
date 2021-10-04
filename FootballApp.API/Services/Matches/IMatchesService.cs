using FootballApp.API.Dtos;
using FootballApp.API.Models.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballApp.API.Services.Matches
{
    public interface IMatchesService
    {
        Task<MatchdayToReturnDto> GetUpcomingMatchday(int matchId);
        Task<KeyValuePair<bool, string>> CreateMatch(MatchdayForCreationDto matchdayForCreation, int userId);
        Task<ICollection<MatchdayToReturnDto>> GetUpcomingMatchesForGroup(int groupId);
        Task<ICollection<MatchdayForDisplayDto>> GetUpcomingMatchesForUser(int userId);
        Task<ICollection<MatchdayToReturnDto>> GetUpcomingMatchesApplicableForUser(int userId);
        Task<KeyValuePair<bool, string>> CheckInForMatch(int userId, int matchId);
        Task<KeyValuePair<bool, string>> GiveUpForMatch(int userId, int matchId);
        Task<KeyValuePair<bool, string>> ConfirmForMatch(int userId, int matchId);
        Task<MatchStatusToReturnDto> GetUserStatusForMatchday(int matchId, int userId);
        Task<KeyValuePair<bool, string>> OrganizeMatch(OrganizeMatchDto organizeMatchDto);
        Task<ICollection<OrganizedMatchInformationView>> GetOrganizedMatchInformation(int matchdayId);
        Task<ICollection<MatchHistoryView>> GetMatchHistoryForUser(int userId);
        Task<ICollection<GroupMatchHistoryView>> GetMatchHistoryForGroup(int groupId);
        Task<ICollection<LatestFiveMatchesView>> GetLatestFiveMatchesForUser(int userId);
    }
}