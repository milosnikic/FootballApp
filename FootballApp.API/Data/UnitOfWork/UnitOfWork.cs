using System.Threading.Tasks;
using FootballApp.API.Data.Achievements;
using FootballApp.API.Data.Cities;
using FootballApp.API.Data.Comments;
using FootballApp.API.Data.Countries;
using FootballApp.API.Data.Friends;
using FootballApp.API.Data.Groups;
using FootballApp.API.Data.Locations;
using FootballApp.API.Data.Matchdays;
using FootballApp.API.Data.MatchStatuses;
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
            Auths = new AuthRepository(_context);
            Comments = new CommentsRepository(_context);
            Achievements = new AchievementsRepository(_context);
            Cities = new CitiesRepository(_context);
            Countries = new CountriesRepository(_context);
            Locations = new LocationsRepository(_context);
            Matchdays = new MatchdaysRepository(_context);
            MatchStatuses = new MatchStatusesRepository(_context);
            Friends = new FriendsRepository(_context);
        }

        public IGroupsRepository Groups { get; private set; }
        public IUsersRepository Users { get; private set; }
        public IPhotosRepository Photos { get; private set; }
        public IAuthRepository Auths { get; private set; }
        public IMembershipsRepository Memberships { get; private set; }
        public ICommentsRepository Comments { get; set; }
        public IAchievementsRepository Achievements { get; private set; }
        public ICitiesRepository Cities { get; private set; }
        public ICountriesRepository Countries { get; private set; }
        public ILocationsRepository Locations { get; private set; }
        public IMatchdaysRepository Matchdays { get; private set; }
        public IMatchStatusesRepository MatchStatuses { get; private set; }
        public IFriendsRepository Friends { get; private set; }

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