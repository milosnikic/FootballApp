using FootballApp.API.Dtos;
using FootballApp.API.Services.Locations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FootballApp.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationsService _locationsService;
        public LocationsController(ILocationsService locationsService)
        {
            _locationsService = locationsService;
        }

        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> AddLocation(LocationToAddDto location)
        {
            return Ok(await _locationsService.AddLocation(location));
        }

        [HttpPost]
        [Route("country")]
        public async Task<IActionResult> AddCountry(string name)
        {
            return Ok(await _locationsService.AddCountry(name));
        }

        [HttpPost]
        [Route("city")]
        public async Task<IActionResult> AddCity(CityForCreationDto city)
        {
            return Ok(await _locationsService.AddCity(city));
        }


        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllLocations()
        {
            return Ok(await _locationsService.GetAllLocations());
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("all-countries")]
        public async Task<IActionResult> GetAllCountriesWithCities()
        {
            return Ok(await _locationsService.GetAllCountriesWithCities());
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("country-cities/{id}")]
        public async Task<IActionResult> GetAllCitiesForCountry(int id)
        {
            return Ok(await _locationsService.GetAllCitiesForCountry(id));
        }
    }
}