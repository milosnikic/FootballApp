using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Groups
{
    public class GroupsRepository : Repository<Group>, IGroupsRepository
    {
        public GroupsRepository(DataContext context)
            : base(context)
        {
        }


        public async Task<IEnumerable<Group>> GetGroupsForUser(int userId)
        {
            var memberships = await DataContext.Memberships.Where(m => m.UserId == userId).Select(m => m.GroupId).ToListAsync();

            var groups = new List<Group>();
            foreach (var groupId in memberships)
            {
                var group = await DataContext.Groups.Include(g => g.Memberships).ThenInclude(m => m.User).FirstOrDefaultAsync(g => g.Id == groupId);
                groups.Add(group);
            }
            return groups;
        }

        public DataContext DataContext
        {
            get { return Context as DataContext; } 
        }
    }
}