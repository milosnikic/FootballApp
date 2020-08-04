using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Memberships
{
    public class MembershipsRepository : Repository<Membership>, IMembershipsRepository
    {
        public MembershipsRepository(DataContext context)
            : base(context)
        {
            
        }

        public DataContext DataContext 
        { 
            get { return Context as DataContext; } 
        }

        public async Task<Membership> GetMembershipById(int userId, int groupId)
        {
            var membership = await DataContext.Memberships
                                              .Where(m => m.GroupId == groupId && m.UserId == userId)
                                              .FirstOrDefaultAsync();
            return membership;
        }
    }
}