using System;
using System.Threading.Tasks;
using FootballApp.API.Data.Achievements;
using FootballApp.API.Data.Cities;
using FootballApp.API.Data.Comments;
using FootballApp.API.Data.Countries;
using FootballApp.API.Data.Groups;
using FootballApp.API.Data.Locations;
using FootballApp.API.Data.Matchdays;
using FootballApp.API.Data.MatchStatuses;
using FootballApp.API.Data.Memberships;
using FootballApp.API.Data.Photos;

namespace FootballApp.API.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersRepository Users { get; }
        IPhotosRepository Photos { get; }
        IGroupsRepository Groups { get; }
        IMembershipsRepository Memberships { get; }
        IAuthRepository Auths { get; }
        ICommentsRepository Comments { get; }
        IAchievementsRepository Achievements { get; }
        ICitiesRepository Cities { get; }
        ICountriesRepository Countries { get; }
        ILocationsRepository Locations { get; }
        IMatchdaysRepository Matchdays { get; }
        IMatchStatusesRepository MatchStatuses { get; }
        Task<bool> Complete();
    }
}