using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Groups
{
    public interface IGroupsRepository : IRepository<Group>
    {
        Task<IEnumerable<Membership>> GetGroupsForUser(int userId);
        Task<ICollection<Membership>> GetFavoriteGroupsForUser(int userId);
        Task<ICollection<Group>> GetAllGroupsWithInclude(int userId);
        Task<ICollection<Membership>> GetCreatedGroupsForUser(PowerUser powerUser);
        Task<Group> GetGroupWithInclude(int groupId);
    }
}