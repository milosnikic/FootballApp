using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.MatchStatuses
{
    public interface IMatchStatusesRepository : IRepository<MatchStatus>
    {
        Task<MatchStatus> GetMatchStatusById(int userId, int matchId);
    }
}