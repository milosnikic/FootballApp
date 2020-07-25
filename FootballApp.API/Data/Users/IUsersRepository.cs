using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<User> GetUserByIdWithAdditionalInformation(int id);
        void VisitUser(Visit visit);
        Task<ICollection<Visit>> GetLatestFiveVisitorsForUser(int userId);
    }
}