using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using FootballApp.API.Services.Locations;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FootballAppTests.ServiceTests
{
    public class LocationsServiceTests
    {
        private readonly LocationsService _sut;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

        public LocationsServiceTests()
        {
            _sut = new LocationsService(_mapperMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task AddCity_ShouldNotAdd_WhenCountryIsInvalid()
        {
            // Arrange
            var responseMessage = "Invalid country selected.";
            var cityForCreationDto = new CityForCreationDto()
            {
                CountryId = 1,
                Name = "Belgrade"
            };
            _unitOfWorkMock.Setup(x => x.Countries.GetById(It.IsAny<int>()))
                            .ReturnsAsync(() => null);

            // Act
            var result = await _sut.AddCity(cityForCreationDto);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task AddCity_ShouldNotAdd_WhenIsNotSaved()
        {
            // Arrange
            var responseMessage = "Problem creating city";
            var cityForCreationDto = new CityForCreationDto()
            {
                CountryId = 1,
                Name = "Belgrade"
            };

            var country = new Country()
            {
                Id = 1,
                Name = "Serbia",
                Users = new List<User>(),
                Cities = new List<City>(),
                Flag = "Flag",
                Locations = new List<Location>()
            };

            var cityToAdd = new City()
            {
                Country = country,
                CountryId = country.Id,
                Id = 1,
                Locations = new List<Location>(),
                Name = cityForCreationDto.Name,
                Users = new List<User>(),
            };

            _unitOfWorkMock.Setup(x => x.Countries.GetById(It.IsAny<int>()))
                            .ReturnsAsync(country);

            _mapperMock.Setup(x => x.Map<City>(cityForCreationDto))
                            .Returns(cityToAdd);

            _unitOfWorkMock.Setup(x => x.Cities.Add(cityToAdd))
                            .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                            .ReturnsAsync(false);

            // Act
            var result = await _sut.AddCity(cityForCreationDto);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task AddCity_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var responseMessage = "City added successfully.";
            var cityForCreationDto = new CityForCreationDto()
            {
                CountryId = 1,
                Name = "Belgrade"
            };

            var country = new Country()
            {
                Id = 1,
                Name = "Serbia",
                Users = new List<User>(),
                Cities = new List<City>(),
                Flag = "Flag",
                Locations = new List<Location>()
            };

            var cityToAdd = new City()
            {
                Country = country,
                CountryId = country.Id,
                Id = 1,
                Locations = new List<Location>(),
                Name = cityForCreationDto.Name,
                Users = new List<User>(),
            };

            _unitOfWorkMock.Setup(x => x.Countries.GetById(It.IsAny<int>()))
                            .ReturnsAsync(country);

            _mapperMock.Setup(x => x.Map<City>(cityForCreationDto))
                            .Returns(cityToAdd);

            _unitOfWorkMock.Setup(x => x.Cities.Add(cityToAdd))
                            .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                            .ReturnsAsync(true);

            // Act
            var result = await _sut.AddCity(cityForCreationDto);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task AddCountry_ShouldNotAdd_WhenCountryExists()
        {
            // Arrange
            var responseMessage = "Country already exists.";

            _unitOfWorkMock.Setup(x => x.Countries.Exists(It.IsAny<string>()))
                            .ReturnsAsync(true);

            // Act
            var result = await _sut.AddCountry(It.IsAny<string>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task AddCountry_ShouldNotAdd_WhenIsNotSaved()
        {
            // Arrange
            var responseMessage = "Problem creating country";

            _unitOfWorkMock.Setup(x => x.Countries.Exists(It.IsAny<string>()))
                            .ReturnsAsync(false);

            var country = new Country()
            {
                Name = "Serbia"
            };

            _unitOfWorkMock.Setup(x => x.Countries.Add(country))
                            .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                            .ReturnsAsync(false);

            // Act
            var result = await _sut.AddCountry(It.IsAny<string>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task AddCountry_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var responseMessage = "Country added successfully.";

            _unitOfWorkMock.Setup(x => x.Countries.Exists(It.IsAny<string>()))
                            .ReturnsAsync(false);

            var country = new Country()
            {
                Name = "Serbia"
            };

            _unitOfWorkMock.Setup(x => x.Countries.Add(country))
                            .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                            .ReturnsAsync(true);

            // Act
            var result = await _sut.AddCountry(It.IsAny<string>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }


        [Fact]
        public async Task AddLocation_ShouldNotAdd_WhenCityDoesNotExist()
        {
            // Arrange
            var responseMessage = "City does not exist";
            var locationToAddDto = new LocationToAddDto()
            {
                Name = "Jelovac",
                CityId = 1,
                CountryId = 1
            };
            _unitOfWorkMock.Setup(x => x.Cities.GetById(It.IsAny<int>()))
                            .ReturnsAsync(() => null);

            // Act
            var result = await _sut.AddLocation(locationToAddDto);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task AddLocation_ShouldNotAdd_WhenCountryDoesNotExist()
        {
            // Arrange
            var responseMessage = "Country does not exist";
            var locationToAddDto = new LocationToAddDto()
            {
                Name = "Jelovac",
                CityId = 1,
                CountryId = 1
            };
            _unitOfWorkMock.Setup(x => x.Cities.GetById(It.IsAny<int>()))
                            .ReturnsAsync(new City());

            _unitOfWorkMock.Setup(x => x.Countries.GetById(It.IsAny<int>()))
                            .ReturnsAsync(() => null);

            // Act
            var result = await _sut.AddLocation(locationToAddDto);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task AddLocation_ShouldNotAdd_WhenIsNotSaved()
        {
            // Arrange
            var responseMessage = "Problem creating location";
            var locationToAddDto = new LocationToAddDto()
            {
                Name = "Jelovac",
                CityId = 1,
                CountryId = 1
            };

            var location = new Location()
            {
                Id = 1,
                City = new City(),
                CityId = locationToAddDto.CityId, 
                Country = new Country(),
                CountryId = locationToAddDto.CountryId,
                Groups = new List<Group>(),
                Name = locationToAddDto.Name
            };

            _unitOfWorkMock.Setup(x => x.Cities.GetById(It.IsAny<int>()))
                            .ReturnsAsync(new City());

            _unitOfWorkMock.Setup(x => x.Countries.GetById(It.IsAny<int>()))
                            .ReturnsAsync(new Country());

            _mapperMock.Setup(x => x.Map<Location>(locationToAddDto))
                            .Returns(location);

            _unitOfWorkMock.Setup(x => x.Locations.Add(location))
                            .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                            .ReturnsAsync(false);

            // Act
            var result = await _sut.AddLocation(locationToAddDto);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task AddLocation_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var responseMessage = "Location created successfully!";
            var locationToAddDto = new LocationToAddDto()
            {
                Name = "Jelovac",
                CityId = 1,
                CountryId = 1
            };

            var location = new Location()
            {
                Id = 1,
                City = new City(),
                CityId = locationToAddDto.CityId,
                Country = new Country(),
                CountryId = locationToAddDto.CountryId,
                Groups = new List<Group>(),
                Name = locationToAddDto.Name
            };

            _unitOfWorkMock.Setup(x => x.Cities.GetById(It.IsAny<int>()))
                            .ReturnsAsync(new City());

            _unitOfWorkMock.Setup(x => x.Countries.GetById(It.IsAny<int>()))
                            .ReturnsAsync(new Country());

            _mapperMock.Setup(x => x.Map<Location>(locationToAddDto))
                            .Returns(location);

            _unitOfWorkMock.Setup(x => x.Locations.Add(location))
                            .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                            .ReturnsAsync(true);

            // Act
            var result = await _sut.AddLocation(locationToAddDto);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact] 
        public async Task GetAllCitiesForCountry_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var cities = new List<City>()
            { 
                new City()
                {
                    Id = 1
                }
            };

            var citiesToReturn = new List<CityToReturnDto>()
            { 
                new CityToReturnDto()
                {
                    Id = 1,
                }
            };

            _unitOfWorkMock.Setup(x => x.Cities.GetAllCitiesForCountry(It.IsAny<int>()))
                            .ReturnsAsync(cities);

            _mapperMock.Setup(x => x.Map<ICollection<CityToReturnDto>>(cities))
                            .Returns(citiesToReturn);

            // Act
            var result = await _sut.GetAllCitiesForCountry(It.IsAny<int>());

            // Assert
            Assert.Equal(cities.FirstOrDefault().Id, result.FirstOrDefault().Id);
        }


        [Fact]
        public async Task GetAllCountriesWithCities_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var countries = new List<Country>()
            {
                new Country()
                {
                    Cities = new List<City>()
                    {
                        new City(){ Id = 1 },
                        new City(){ Id = 2 },
                    }
                }
            };

            var countriesToReturn = new List<CountryToReturnDto>()
            {
                new CountryToReturnDto()
                {
                    Cities = new List<CityToReturnDto>()
                    {
                        new CityToReturnDto(){ Id = 1 },
                        new CityToReturnDto(){ Id = 2 },
                    }
                }
            };

            _unitOfWorkMock.Setup(x => x.Countries.GetAllCountriesWithCities())
                            .ReturnsAsync(countries);

            _mapperMock.Setup(x => x.Map<ICollection<CountryToReturnDto>>(countries))
                            .Returns(countriesToReturn);

            // Act
            var result = await _sut.GetAllCountriesWithCities();

            // Assert
            Assert.Equal(countries.FirstOrDefault().Cities.FirstOrDefault().Id, result.FirstOrDefault().Cities.FirstOrDefault().Id);
        }

        [Fact]
        public async Task GetAllLocations_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var locations = new List<Location>()
            {
                new Location()
                {
                    Id = 1,
                    Name = "Test",
                    Country = new Country(),
                    City = new City(),
                    CityId = 1,
                    CountryId = 1,
                    Groups = new List<Group>()
                }
            };

            var locationsToReturn = new List<LocationToReturnDto>()
            {
                new LocationToReturnDto()
                {
                    Id = 1,
                    City = "Test",
                    Country = "Test",
                    Name = "Test"
                }
            };

            _unitOfWorkMock.Setup(x => x.Locations.GetAllLocationsWithInclude())
                            .ReturnsAsync(locations);

            _mapperMock.Setup(x => x.Map<ICollection<LocationToReturnDto>>(locations))
                            .Returns(locationsToReturn);

            // Act
            var result = await _sut.GetAllLocations();

            // Assert
            Assert.Equal(locations.FirstOrDefault().Id, result.FirstOrDefault().Id);
        }
    }
}