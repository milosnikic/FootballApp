using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Matchdays
{
    public interface IMatchdaysRepository : IRepository<Matchday>
    {
        Task<Matchday> GetMatchdayWithAdditionalInformation(int matchId);   
        Task<ICollection<Matchday>> GetUpcomingMatchesForGroup(int groupId);
    }
}