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

        public DataContext DataContext
        {
            get { return Context as DataContext; } 
        }
    }
}