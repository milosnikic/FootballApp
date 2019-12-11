using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Groups
{
    public interface IGroupsRepository
    {
        Task<IEnumerable<Group>> GetGroups();
        Task<Group> GetGroup(int id);
        Task<IEnumerable<Group>> GetGroupsForUser(int userId);
    }
}