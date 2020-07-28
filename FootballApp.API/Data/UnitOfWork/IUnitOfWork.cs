using System;
using System.Threading.Tasks;
using FootballApp.API.Data.Achievements;
using FootballApp.API.Data.Comments;
using FootballApp.API.Data.Groups;
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
        Task<bool> Complete();
    }
}