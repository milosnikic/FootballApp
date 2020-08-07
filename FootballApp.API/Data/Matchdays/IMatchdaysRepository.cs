using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Matchdays
{
    public interface IMatchdaysRepository : IRepository<Matchday>
    {
        Task<Matchday> GetMatchdayWithLocation(int matchId);   
    }
}