using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Countries
{
    public interface ICountriesRepository : IRepository<Country>
    {
        Task<bool> Exists(string name);
        Task<ICollection<Country>> GetAllCountriesWithCities();
    }
}