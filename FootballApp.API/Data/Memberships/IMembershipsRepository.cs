using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Memberships
{
    public interface IMembershipsRepository : IRepository<Membership>
    {
        Task<Membership> GetMembershipById(int userId, int groupId);
    }
}