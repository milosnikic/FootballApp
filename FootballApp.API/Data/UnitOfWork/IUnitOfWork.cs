using System;
using System.Threading.Tasks;
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
        Task<bool> Complete();
    }
}