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

        public async Task<ICollection<Group>> GetAllGroupsWithInclude(int userId) 
        {
            var groups = await DataContext.Groups
                                          .Where(g => g.Memberships.FirstOrDefault(m => m.UserId == userId 
                                                                                    && g.Id == m.GroupId 
                                                                                    && (m.MembershipStatus == MembershipStatus.Accepted
                                                                                        || m.MembershipStatus == MembershipStatus.Sent)) == null)
                                          .Include(g => g.Location)
                                          .Include(g => g.Memberships)
                                          .ToListAsync();

            return groups;
        }
        public async Task<IEnumerable<Membership>> GetGroupsForUser(int userId)
        {
            // var memberships = await DataContext.Memberships.Where(m => m.UserId == userId).Select(m => m.GroupId).ToListAsync();

            // var groups = new List<Group>();
            // foreach (var groupId in memberships)
            // {
            //     var group = await DataContext.Groups.Include(g => g.Memberships).ThenInclude(m => m.User).Include(g => g.Location).FirstOrDefaultAsync(g => g.Id == groupId);
            //     groups.Add(group);
            // }
            var groups = await DataContext.Memberships
                                          .Where(m => m.UserId == userId)
                                          .Include(m => m.Group)
                                          .ThenInclude(g => g.Memberships)
                                          .ToListAsync();

            return groups;
        }

        public DataContext DataContext
        {
            get { return Context as DataContext; } 
        }
    }
}