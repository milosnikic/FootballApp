using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Users
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {

        public UsersRepository(DataContext context)
            : base(context)
        {
        }

        public async Task<User> GetUserByIdWithAdditionalInformation(int id)
        {
            return await DataContext.Users
                                    .Include(u => u.Memberships)
                                    .Include(u => u.Photos)
                                    .FirstOrDefaultAsync(u => u.Id == id);
        }
        public DataContext DataContext
        {
            get { return Context as DataContext; } 
        }

    }
}