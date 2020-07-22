using FootballApp.API.Models;

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
    }
}