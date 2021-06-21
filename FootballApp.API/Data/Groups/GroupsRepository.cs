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
                                          .ThenInclude(l => l.Country)
                                          .Include(g => g.Location.City)
                                          .Include(g => g.Memberships)
                                          .ToListAsync();

            return groups;
        }
        public async Task<IEnumerable<Membership>> GetGroupsForUser(int userId)
        {
            var groups = await DataContext.Memberships
                                          .Where(m => m.UserId == userId)
                                          .Include(m => m.Group)
                                          .ThenInclude(g => g.Memberships)
                                          .Include(m => m.Group.Location)
                                          .ThenInclude(l => l.City)
                                          .ThenInclude(l => l.Country)
                                          .ToListAsync();

            return groups;
        }

        public async Task<ICollection<Membership>> GetFavoriteGroupsForUser(int userId)
        {
            var groups = await DataContext.Memberships
                                          .Where(m => m.UserId == userId && m.Favorite)
                                          .Include(m => m.Group)
                                          .ThenInclude(g => g.Memberships)
                                          .Include(m => m.Group.Location)
                                          .ThenInclude(l => l.City)
                                          .ThenInclude(l => l.Country)
                                          .ToListAsync();

            return groups;
        }

        public async Task<ICollection<Membership>> GetCreatedGroupsForUser(PowerUser powerUser)
        {
            var groups = await DataContext.Memberships
                                          .Where(m => m.UserId == powerUser.Id && m.Role == Role.Owner)
                                          .Include(m => m.Group)
                                          .ThenInclude(g => g.Memberships)
                                          .Include(m => m.Group.Location)
                                          .ThenInclude(l => l.City)
                                          .ThenInclude(l => l.Country)
                                          .ToListAsync();
            return groups;
        }

        public async Task<Membership> GetGroupWithInclude(int groupId, int userId)
        {
            
            var group = await DataContext.Memberships
                                              .Where(m => m.GroupId == groupId)
                                              .Include(m => m.User)
                                              .Include(m => m.Group)
                                              .ThenInclude(g => g.Location)
                                              .ThenInclude(l => l.City)
                                              .ThenInclude(l => l.Country)
                                              .Include(m => m.Group)
                                              .ThenInclude(g => g.Memberships)
                                              .ThenInclude(m => m.User)
                                              .ThenInclude(u => u.Photos)
                                              .FirstOrDefaultAsync();


            return group;
        }

        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }
    }
}