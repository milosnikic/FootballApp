using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Dtos;

namespace FootballApp.API.Services.Locations
{
    public interface ILocationsService
    {
        Task<KeyValuePair<bool, string>> AddLocation(LocationToAddDto location);
        Task<KeyValuePair<bool, string>> AddCountry(string name);
        Task<KeyValuePair<bool, string>> AddCity(CityForCreationDto city);
        Task<IEnumerable<LocationToReturnDto>> GetAllLocations();
        Task<IEnumerable<CountryToReturnDto>> GetAllCountriesWithCities();
        Task<IEnumerable<CityToReturnDto>> GetAllCitiesForCountry(int id);
    }
}