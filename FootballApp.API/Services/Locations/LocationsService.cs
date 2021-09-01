using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;

namespace FootballApp.API.Services.Locations
{
    public class LocationsService : ILocationsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public LocationsService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<KeyValuePair<bool, string>> AddCity(CityForCreationDto city)
        {
            var country = await _unitOfWork.Countries.GetById(city.CountryId);
            if(country == null)
            {
                return new KeyValuePair<bool, string>(false, "Invalid country selected.");
            }

            var cityToAdd = _mapper.Map<City>(city);
            cityToAdd.Country = country;
            _unitOfWork.Cities.Add(cityToAdd);

            if(await _unitOfWork.Complete())
            {
                return new KeyValuePair<bool, string>(true, "City added successfully.");
            }

            return new KeyValuePair<bool, string>(false, "Problem creating city");
        }

        public async Task<KeyValuePair<bool, string>> AddCountry(string name)
        {
            if (!await _unitOfWork.Countries.Exists(name)) 
            {
                return new KeyValuePair<bool, string>(false, "Country already exists.");
            }
            
            var country = new Country { Name = name };
             _unitOfWork.Countries.Add(country);

            if(await _unitOfWork.Complete())
            {
                return new KeyValuePair<bool, string>(true, "Country added successfully.");
            }

            return new KeyValuePair<bool, string>(false, "Problem creating country");
        }

        public async Task<KeyValuePair<bool, string>> AddLocation(LocationToAddDto location)
        {
            var city = await _unitOfWork.Cities.GetById(location.CityId);
            var country = await _unitOfWork.Countries.GetById(location.CountryId);
            if (city == null)
            {
                return new KeyValuePair<bool, string>(false, "City does not exist");
            }

            if(country == null)
            {
                return new KeyValuePair<bool, string>(false, "Country does not exist");
            }

            var locationToAdd = _mapper.Map<Location>(location);
            locationToAdd.City = city;
            locationToAdd.Country = country;

            _unitOfWork.Locations.Add(locationToAdd);

            if (await _unitOfWork.Complete())
            {
                // TODO: Comment all Repository and Controller methods
                //       Move messages to constants!
                return new KeyValuePair<bool, string>(true, "Location created successfully!");
            }

            return new KeyValuePair<bool, string>(false, "Problem creating location");
        }

        public async Task<IEnumerable<CityToReturnDto>> GetAllCitiesForCountry(int id)
        {
            var cities = await _unitOfWork.Cities.GetAllCitiesForCountry(id);
            return _mapper.Map<ICollection<CityToReturnDto>>(cities);
        }

        public async Task<IEnumerable<CountryToReturnDto>> GetAllCountriesWithCities()
        {
            var countries = await _unitOfWork.Countries.GetAllCountriesWithCities();
            return _mapper.Map<ICollection<CountryToReturnDto>>(countries);
        }

        public async Task<IEnumerable<LocationToReturnDto>> GetAllLocations()
        {
            var locations = await _unitOfWork.Locations.GetAllLocationsWithInclude();
            return _mapper.Map<ICollection<LocationToReturnDto>>(locations);
        }
    }
}