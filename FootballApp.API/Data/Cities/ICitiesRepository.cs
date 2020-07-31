using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Cities
{
    public interface ICitiesRepository : IRepository<City>
    {
        Task<ICollection<City>> GetAllCitiesForCountry(int id);
    }
}