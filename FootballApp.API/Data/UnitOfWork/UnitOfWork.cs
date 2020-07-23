using System.Threading.Tasks;
using FootballApp.API.Data.Groups;
using FootballApp.API.Data.Memberships;
using FootballApp.API.Data.Photos;
using FootballApp.API.Data.Users;

namespace FootballApp.API.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
            Users = new UsersRepository(_context);
            Photos = new PhotosRepository(_context);
            Groups = new GroupsRepository(_context);
            Memberships = new MembershipsRepository(_context);
        }

        public IGroupsRepository Groups { get; private set; }
        public IUsersRepository Users { get; private set; }
        public IPhotosRepository Photos { get; private set; }
        public IMembershipsRepository Memberships { get; private set; }

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}