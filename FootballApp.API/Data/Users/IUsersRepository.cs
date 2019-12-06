using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        
    }
}