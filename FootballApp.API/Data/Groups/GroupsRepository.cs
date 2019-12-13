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
            // var group = await _context.Groups.Include(g => g.User).FirstOrDefaultAsync(g => g.Id == id);

            // return group;
            throw new System.Exception();
        }

        public async Task<IEnumerable<Group>> GetGroups()
        {
            var groups = await _context.Groups.ToListAsync();

            return groups;
        }

        public async Task<IEnumerable<Group>> GetGroupsForUser(int userId)
        {
            // var groups = await _context.Groups.Include(g => g.Users).Where(u => u.UserId == userId).ToListAsync();

            throw new System.Exception();
        }
    }
}