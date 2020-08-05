using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballApp.API.Data
{
    public interface IAuthRepository : IRepository<User>
    {
        Task<User> Login(string username, string password);
        Task<User> Register(CommonUser user, string password);
        Task<bool> UserExists(string username);
        
    }
}