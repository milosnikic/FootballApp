using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Locations
{
    public interface ILocationsRepository : IRepository<Location>
    {
        Task<ICollection<Location>> GetAllLocationsWithInclude();
        Task<Location> GetByName(string name);
    }
}