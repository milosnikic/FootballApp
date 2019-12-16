using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Groups
{
    public class GroupsRepository : IGroupsRepository
    {
        private readonly DataContext _context;
        public GroupsRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Group> GetGroup(int id)
        {
            var group = await _context.Groups.Include(g => g.Memberships).FirstOrDefaultAsync(g => g.Id == id);

            return group;
        }

        public async Task<IEnumerable<Group>> GetGroups()
        {
            var groups = await _context.Groups.Include(g => g.Memberships).ToListAsync();

            return groups;
        }

        public async Task<IEnumerable<Group>> GetGroupsForUser(int userId)
        {
            var memberships = await _context.Memberships.Where(m => m.UserId == userId).Select(m => m.GroupId).ToListAsync();

            var groups = new List<Group>();
            foreach (var groupId in memberships)
            {
                var group = await _context.Groups.Include(g => g.Memberships).ThenInclude(m => m.User).FirstOrDefaultAsync(g => g.Id == groupId);
                groups.Add(group);
            }
            return groups;
        }
    }
}