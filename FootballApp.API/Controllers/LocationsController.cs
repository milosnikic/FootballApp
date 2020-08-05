using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballApp.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LocationsController(IUnitOfWork unitOfWork,
                                   IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }

        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> AddLocation(LocationToAddDto location)
        {
            var city = await _unitOfWork.Cities.GetById(location.CityId);
            var country = await _unitOfWork.Countries.GetById(location.CountryId);
            if (city == null)
            {
                return BadRequest("City does not exist");
            }

            if(country == null)
            {
                return BadRequest("Country does not exist");
            }

            var locationToAdd = _mapper.Map<Location>(location);
            locationToAdd.City = city;
            locationToAdd.Country = country;

            _unitOfWork.Locations.Add(locationToAdd);

            if (await _unitOfWork.Complete())
            {
                // TODO: Comment all Repository and Controller methods
                //       Move messages to constants!
                return Ok(new KeyValuePair<bool, string>(true, "Location created successfully!"));
            }
            return BadRequest("Problem creating location");
        }

        [HttpPost]
        [Route("country")]
        public async Task<IActionResult> AddCountry(string name)
        {
            if (!await _unitOfWork.Countries.Exists(name)) 
            {
                return BadRequest("Country already exists.");
            }
            var country = new Country { Name = name };
             _unitOfWork.Countries.Add(country);

            if(await _unitOfWork.Complete())
            {
                return Ok(new KeyValuePair<bool, string>(true, "Country added successfully."));
            }
            return BadRequest("Problem creating country");
        }

        [HttpPost]
        [Route("city")]
        public async Task<IActionResult> AddCity(CityForCreationDto city)
        {
            var country = await _unitOfWork.Countries.GetById(city.CountryId);
            if(country == null)
            {
                return BadRequest("Invalid country selected.");
            }

            var cityToAdd = _mapper.Map<City>(city);
            cityToAdd.Country = country;
            _unitOfWork.Cities.Add(cityToAdd);

            if(await _unitOfWork.Complete())
            {
                return Ok(new KeyValuePair<bool, string>(true, "City added successfully."));
            }

            return BadRequest("Problem creating city");
        }


        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllLocations()
        {
            var locations = await _unitOfWork.Locations.GetAllLocationsWithInclude();
            return Ok(_mapper.Map<ICollection<LocationToReturnDto>>(locations));
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("all-countries")]
        public async Task<IActionResult> GetAllCountriesWithCities()
        {
            var countries = await _unitOfWork.Countries.GetAllCountriesWithCities();
            return Ok(_mapper.Map<ICollection<CountryToReturnDto>>(countries));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("country-cities/{id}")]
        public async Task<IActionResult> GetAllCitiesForCountry(int id)
        {
            var cities = await _unitOfWork.Cities.GetAllCitiesForCountry(id);
            return Ok(_mapper.Map<ICollection<CityToReturnDto>>(cities));
        }
    }
}