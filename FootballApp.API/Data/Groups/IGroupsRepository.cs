using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Groups
{
    public interface IGroupsRepository : IRepository<Group>
    {
        Task<IEnumerable<Group>> GetGroupsForUser(int userId);
    }
}