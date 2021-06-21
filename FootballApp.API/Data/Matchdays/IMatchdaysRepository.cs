using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using FootballApp.API.Models.Views;

namespace FootballApp.API.Data.Matchdays
{
    public interface IMatchdaysRepository : IRepository<Matchday>
    {
        Task<Matchday> GetMatchdayWithAdditionalInformation(int matchId);   
        Task<ICollection<Matchday>> GetUpcomingMatchesForGroup(int groupId);
        Task<ICollection<MatchStatus>> GetUpcomingMatchesForUser(int userId);
        Task<ICollection<Matchday>> GetUpcomingMatchesApplicableForUser(int userId);
        Task<KeyValuePair<bool, string>> OrganizeMatchPlayed(OrganizeMatchDto organizeMatchDto);
        Task<ICollection<OrganizedMatchInformationView>> GetOrganizedMatchInformation(int matchdayId);
        Task<ICollection<MatchHistoryView>> GetMatchHistoryForUser(int userId);
        Task<ICollection<GroupMatchHistoryView>> GetMatchHistoryForGroup(int groupId);
        Task<ICollection<LatestFiveMatchesView>> GetLatestFiveMatchesForUser(int userId);
    }
}