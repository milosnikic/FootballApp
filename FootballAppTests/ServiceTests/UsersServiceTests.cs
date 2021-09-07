using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using FootballApp.API.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FootballAppTests.ServiceTests
{
    public class UsersServiceTests
    {
        private readonly UsersService _sut;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly ITestOutputHelper _output;

        public UsersServiceTests(ITestOutputHelper output)
        {
            _output = output;
            _sut = new UsersService(_mapperMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GainAchievement_ShouldGainAchievementSuccessfully_WhenAchievementExists()
        {
            // Arrange
            var userId = 1;
            var user = new PowerUser
            {
                Id = userId
            };

            var achievementName = "Top goal scorer";
            var achievementId = 1;
            var achievementPoints = 5;
            var achievementIcon = "test";
            var achievementValue = 10;
            var achievement = new Achievement
            {
                Id = achievementId,
                Name = achievementName,
                Points = achievementPoints,
                Icon = achievementIcon,
                Value = achievementValue
            };

            var dateAchieved = DateTime.Now;
            var gainedAchievementDto = new GainedAchievementForCreationDto
            {
                UserId = userId,
                DateAchieved = dateAchieved,
                Value = achievementValue
            };

            _unitOfWorkMock.Setup(x => x.Achievements.GetAchievementByValue(It.IsAny<int>()))
                           .ReturnsAsync(achievement);
            _unitOfWorkMock.Setup(x => x.Achievements.GetGainedAchievement(It.IsAny<int>(), achievementValue))
                           .ReturnsAsync(() => null);
            _unitOfWorkMock.Setup(x => x.Users.GetById(It.IsAny<int>()))
                           .ReturnsAsync(user);
            _unitOfWorkMock.Setup(x => x.Complete())
                            .ReturnsAsync(true);
            // Act
            KeyValuePair<bool, string> response = await _sut.GainAchievement(userId, gainedAchievementDto);

            // Assert
            Assert.True(response.Key);
            Assert.Equal("Achievement successfully gained!", response.Value);
        }

        [Fact]
        public async Task GainAchievement_ShouldNotGainAchievement_WhenGainedAchievementIsNotSaved()
        {
            // Arrange
            var userId = 1;
            var user = new PowerUser
            {
                Id = userId
            };

            var achievementName = "Top goal scorer";
            var achievementId = 1;
            var achievementPoints = 5;
            var achievementIcon = "test";
            var achievementValue = 10;
            var achievement = new Achievement
            {
                Id = achievementId,
                Name = achievementName,
                Points = achievementPoints,
                Icon = achievementIcon,
                Value = achievementValue
            };

            var dateAchieved = DateTime.Now;
            var gainedAchievementDto = new GainedAchievementForCreationDto
            {
                UserId = userId,
                DateAchieved = dateAchieved,
                Value = achievementValue
            };

            _unitOfWorkMock.Setup(x => x.Achievements.GetAchievementByValue(It.IsAny<int>()))
                .ReturnsAsync(achievement);
            _unitOfWorkMock.Setup(x => x.Achievements.GetGainedAchievement(It.IsAny<int>(), achievementValue))
                .ReturnsAsync(() => null);
            _unitOfWorkMock.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(user);
            _unitOfWorkMock.Setup(x => x.Complete())
                .ReturnsAsync(false);
            // Act
            KeyValuePair<bool, string> response = await _sut.GainAchievement(userId, gainedAchievementDto);

            // Assert
            Assert.False(response.Key);
            Assert.Equal("Problem gaining achievement!", response.Value);
        }

        [Fact]
        public async Task GainAchievement_ShouldNotGainAchievement_WhenAchievementDoesNotExists()
        {
            // Arrange
            _unitOfWorkMock.Setup(x => x.Achievements.GetAchievementByValue(It.IsAny<int>()))
                           .ReturnsAsync(() => null);
            // Act
            KeyValuePair<bool, string> response = await _sut.GainAchievement(It.IsAny<int>(), new GainedAchievementForCreationDto());

            // Assert
            Assert.False(response.Key);
            Assert.Equal("Not valid achievement!", response.Value);
        }

        [Fact]
        public async Task GainAchievement_ShouldNotGainAchievement_WhenGainedAchievementExists()
        {
            // Arrange
            _unitOfWorkMock.Setup(x => x.Achievements.GetAchievementByValue(It.IsAny<int>()))
                            .ReturnsAsync(new Achievement());
            _unitOfWorkMock.Setup(x => x.Achievements.GetGainedAchievement(It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(new GainedAchievement());
            // Act
            KeyValuePair<bool, string> response = await _sut.GainAchievement(It.IsAny<int>(), new GainedAchievementForCreationDto());

            // Assert
            Assert.False(response.Key);
            Assert.Equal("Already have gained that achievement!", response.Value);
        }

        [Fact]
        public async Task GetAllAchievements_ShouldReturnTwoInList()
        {
            // Arrange
            var achievementList = new List<Achievement>();
            achievementList.Add(new Achievement());
            achievementList.Add(new Achievement());
            _unitOfWorkMock.Setup(x => x.Achievements.GetAll())
                            .ReturnsAsync(achievementList);

            // Act
            var list = await _sut.GetAllAchievements();

            // Assert
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task GetAllAchievements_ShouldReturnAllAchievementsForUser()
        {
            // Arrange
            var achievementId = 1;
            var userId = 1;
            var dateAchieved = DateTime.Now;
            var gainedAchievement = new GainedAchievement()
            {
                Id = 1,
                AchievementId = achievementId,
                DateAchieved = dateAchieved,
                UserId = userId
            };

            var gainedAchievementList = new List<GainedAchievement>();
            gainedAchievementList.Add(gainedAchievement);

            var gainedAchievementDto = new GainedAchievementToReturnDto()
            {
                DateAchieved = dateAchieved,
                Icon = "test",
                Name = "Test",
                Value = 5
            };

            ICollection<GainedAchievementToReturnDto> gainedAchievementToReturnDtos =
                new List<GainedAchievementToReturnDto>();

            gainedAchievementToReturnDtos.Add(gainedAchievementDto);

            _unitOfWorkMock.Setup(x => x.Users.GetAllAchievementsForUser(It.IsAny<int>()))
                .ReturnsAsync(gainedAchievementList);

            _mapperMock.Setup(x =>
                    x.Map<ICollection<GainedAchievementToReturnDto>>
                    (gainedAchievementList))
                .Returns(gainedAchievementToReturnDtos);

            // Act
            var response = await _sut.GetAllAchievementsForUser(userId);

            // Assert
            Assert.Equal(1, response.Count);
            Assert.Equal(dateAchieved, response.FirstOrDefault().DateAchieved);
            Assert.Equal(5, response.FirstOrDefault().Value);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnTwo()
        {
            // Arrange
            var firstUserId = 1;
            var firstUserName = "Milos";
            var firstUserLastName = "Nikic";
            var secondUserId = 1;
            var secondUserName = "Milos";
            var secondUserLastName = "Nikic";
            ICollection<User> users = new List<User>();

            var firstUser = new PowerUser()
            {
                Id = firstUserId,
                Firstname = firstUserName,
                Lastname = firstUserLastName
            };

            var secondUser = new PowerUser()
            {
                Id = secondUserId,
                Firstname = secondUserName,
                Lastname = secondUserLastName
            };

            users.Add(firstUser);
            users.Add(secondUser);

            ICollection<UserToReturnDto> userToReturnDtos = new List<UserToReturnDto>();
            var firstUserToReturnDto = new UserToReturnDto()
            {
                Id = firstUserId,
                Firstname = firstUserName,
                Lastname = firstUserLastName
            };

            var secondUserToReturnDto = new UserToReturnDto()
            {
                Id = secondUserId,
                Firstname = secondUserName,
                Lastname = secondUserLastName
            };

            userToReturnDtos.Add(firstUserToReturnDto);
            userToReturnDtos.Add(secondUserToReturnDto);

            _unitOfWorkMock.Setup(x => x.Users.GetUsers())
                .ReturnsAsync(users);

            _mapperMock.Setup(x =>
                    x.Map<ICollection<UserToReturnDto>>
                        (users))
                .Returns(userToReturnDtos);

            // Act
            var response = await _sut.GetAllUsers();

            // Assert
            var firstUserFromList = response.FirstOrDefault(x => x.Id == firstUserToReturnDto.Id);
            var secondUserFromList = response.FirstOrDefault(x => x.Id == secondUserToReturnDto.Id);
            Assert.Equal(firstUserToReturnDto.Id, firstUserFromList.Id);
            Assert.Equal(firstUserToReturnDto.Firstname, firstUserFromList.Firstname);
            Assert.Equal(firstUserToReturnDto.Lastname, firstUserFromList.Lastname);
            Assert.Equal(secondUserToReturnDto.Id, secondUserFromList.Id);
            Assert.Equal(secondUserToReturnDto.Firstname, secondUserFromList.Firstname);
            Assert.Equal(secondUserToReturnDto.Lastname, secondUserFromList.Lastname);
        }

        [Fact]
        public async Task GetLatestFiveVisitorsForUser_ShouldReturnFiveVisitors()
        {
            // Arrange
            ICollection<VisitToReturnDto> visitToReturnDtos = new List<VisitToReturnDto>();
            visitToReturnDtos.Add(new VisitToReturnDto 
            {
                Visitor = new UserToReturnDto
                {
                    Id = 1,
                    Firstname = "Milos",
                    Lastname = "Nikic"
                }
            });
            visitToReturnDtos.Add(new VisitToReturnDto
            {
                Visitor = new UserToReturnDto
                {
                    Id = 2,
                    Firstname = "Jovan",
                    Lastname = "Jovanovic"
                }
            });
            visitToReturnDtos.Add(new VisitToReturnDto
            {
                Visitor = new UserToReturnDto
                {
                    Id = 3,
                    Firstname = "Ivan",
                    Lastname = "Ivanovic"
                }
            });
            visitToReturnDtos.Add(new VisitToReturnDto
            {
                Visitor = new UserToReturnDto
                {
                    Id = 4,
                    Firstname = "Milan",
                    Lastname = "Milanovic"
                }
            });
            visitToReturnDtos.Add(new VisitToReturnDto
            {
                Visitor = new UserToReturnDto
                {
                    Id = 5,
                    Firstname = "Pera",
                    Lastname = "Peric"
                }
            });


            _unitOfWorkMock.Setup(x => x.Users.GetLatestFiveVisitorsForUser(It.IsAny<int>()))
                            .ReturnsAsync(It.IsAny<ICollection<Visit>>());
            _mapperMock.Setup(x => x.Map<ICollection<VisitToReturnDto>>(It.IsAny<ICollection<Visit>>()))
                            .Returns(visitToReturnDtos);

            // Act
            var result = await _sut.GetLatestFiveVisitorsForUser(It.IsAny<int>());

            // Assert
            Assert.Equal(1, result.FirstOrDefault(x => x.Visitor.Id == 1).Visitor.Id);
            Assert.Equal("Milos", result.FirstOrDefault(x => x.Visitor.Id == 1).Visitor.Firstname);
            Assert.Equal("Nikic", result.FirstOrDefault(x => x.Visitor.Id == 1).Visitor.Lastname);
        }

        [Fact]
        public async Task GetUser_ShouldNotReturnUser()
        {
            // Arrange
            _unitOfWorkMock.Setup(x => x.Users.GetUserByIdWithAdditionalInformation(It.IsAny<int>()))
                    .ReturnsAsync(() => null);
            // Act
            var result = await _sut.GetUser(It.IsAny<int>());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUser_ShouldReturnUser()
        {
            // Arrange
            var userId = 1;
            var firstname = "Jelena";
            var lastname = "Ralevic";
            var user = new PowerUser()
            {
                Id = userId,
                Firstname = firstname,
                Lastname = lastname
            };

            var userToReturnDto = new UserToReturnDto()
            {
                Id = userId,
                Firstname = firstname,
                Lastname = lastname
            };

            _unitOfWorkMock.Setup(x => x.Users.GetUserByIdWithAdditionalInformation(userId))
                    .ReturnsAsync(user);

            _mapperMock.Setup(x => x.Map<UserToReturnDto>(It.IsAny<PowerUser>()))
                    .Returns(userToReturnDto);
            // Act
            var result = await _sut.GetUser(userId);

            // Assert
            Assert.Equal(userId, result.Id);
            Assert.Equal(firstname, result.Firstname);
            Assert.Equal(lastname, result.Lastname);
        }

        [Fact]
        public async Task UpdateUser_ShouldNotUpdate_WhenUserDoesNotExist()
        {
            // Arrange
            var responseMessage = "Specified user doesn't exist.";
            _unitOfWorkMock.Setup(x => x.Users.GetById(It.IsAny<int>()))
                    .ReturnsAsync(() => null);

            // Act
            var result = await _sut.UpdateUser(It.IsAny<int>(), It.IsAny<UserToUpdateDto>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task UpdateUser_ShouldNotUpdate_WhenCountryIsInvalid()
        {
            // Arrange
            var responseMessage = "Specified country or city doesn't exist!";
            var userId = 1;
            var firstname = "Jelena";
            var lastname = "Ralevic";
            var countryId = 2;
            var cityId = 3;

            var user = new PowerUser()
            { 
                Id = userId,
                Firstname = firstname,
                Lastname = lastname
            };

            var userToUpdateDto = new UserToUpdateDto()
            {
                Country = countryId,
                City = cityId
            };

            _unitOfWorkMock.Setup(x => x.Users.GetById(It.IsAny<int>()))
                    .ReturnsAsync(user);

            _unitOfWorkMock.Setup(x => x.Countries.GetCountryWithCities(countryId))
                    .ReturnsAsync(() => null);

            _unitOfWorkMock.Setup(x => x.Cities.GetCityById(cityId, countryId))
                    .ReturnsAsync(It.IsAny<City>());
            // Act
            var result = await _sut.UpdateUser(userId, userToUpdateDto);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task UpdateUser_ShouldNotUpdate_WhenCityIsInvalid()
        {
            // Arrange
            var responseMessage = "Specified country or city doesn't exist!";
            var userId = 1;
            var firstname = "Jelena";
            var lastname = "Ralevic";
            var countryId = 2;
            var cityId = 3;

            var user = new PowerUser()
            {
                Id = userId,
                Firstname = firstname,
                Lastname = lastname
            };

            var userToUpdateDto = new UserToUpdateDto()
            {
                Country = countryId,
                City = cityId
            };

            _unitOfWorkMock.Setup(x => x.Users.GetById(It.IsAny<int>()))
                    .ReturnsAsync(user);

            _unitOfWorkMock.Setup(x => x.Countries.GetCountryWithCities(countryId))
                    .ReturnsAsync(It.IsAny<Country>());

            _unitOfWorkMock.Setup(x => x.Cities.GetCityById(cityId, countryId))
                    .ReturnsAsync(() => null);
            // Act
            var result = await _sut.UpdateUser(userId, userToUpdateDto);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task UpdateUser_ShouldNotUpdate_WhenItIsNotSaved()
        {
            // Arrange
            var responseMessage = "User can't be updated.";
            var userId = 1;
            var firstname = "Jelena";
            var lastname = "Ralevic";
            var countryId = 2;
            var cityId = 3;

            var user = new PowerUser()
            {
                Id = userId,
                Firstname = firstname,
                Lastname = lastname
            };

            var userToUpdateDto = new UserToUpdateDto()
            {
                Country = countryId,
                City = cityId
            };

            var city = new City()
            {
                Id = cityId,
                Name = "Beograd",
                CountryId = countryId,
            };

            var country = new Country()
            { 
                Id = countryId,
                Name = "Srbija",
                Cities = new List<City>()
                {
                    city
                }
            };

            _unitOfWorkMock.Setup(x => x.Users.GetById(It.IsAny<int>()))
                    .ReturnsAsync(user);

            _unitOfWorkMock.Setup(x => x.Countries.GetCountryWithCities(countryId))
                    .ReturnsAsync(country);

            _unitOfWorkMock.Setup(x => x.Cities.GetCityById(cityId, countryId))
                    .ReturnsAsync(city);

            _unitOfWorkMock.Setup(x => x.Complete())
                    .ReturnsAsync(false);
            // Act
            var result = await _sut.UpdateUser(userId, userToUpdateDto);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task UpdateUser_ShouldUpdate()
        {
            // Arrange
            var responseMessage = "User successfully updated.";
            var userId = 1;
            var firstname = "Jelena";
            var lastname = "Ralevic";
            var countryId = 2;
            var cityId = 3;

            var user = new PowerUser()
            {
                Id = userId,
                Firstname = firstname,
                Lastname = lastname
            };

            var userToUpdateDto = new UserToUpdateDto()
            {
                Country = countryId,
                City = cityId
            };

            var city = new City()
            {
                Id = cityId,
                Name = "Beograd",
                CountryId = countryId,
            };

            var country = new Country()
            {
                Id = countryId,
                Name = "Srbija",
                Cities = new List<City>()
                {
                    city
                }
            };

            _unitOfWorkMock.Setup(x => x.Users.GetById(It.IsAny<int>()))
                    .ReturnsAsync(user);

            _unitOfWorkMock.Setup(x => x.Countries.GetCountryWithCities(countryId))
                    .ReturnsAsync(country);

            _unitOfWorkMock.Setup(x => x.Cities.GetCityById(cityId, countryId))
                    .ReturnsAsync(city);

            _unitOfWorkMock.Setup(x => x.Complete())
                    .ReturnsAsync(true);
            // Act
            var result = await _sut.UpdateUser(userId, userToUpdateDto);

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task VisitUser_ShouldNotVisit_WhenItIsNotSaved()
        {
            // Arrange
             var responseMessage = "Problem with visiting user!";

            _mapperMock.Setup(x => x.Map<Visit>(It.IsAny<VisitUserDto>()))
                    .Returns(new Visit());

            _unitOfWorkMock.Setup(x => x.Users.VisitUser(It.IsAny<Visit>()))
                    .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                    .ReturnsAsync(false);
            // Act
            var result = await _sut.VisitUser(It.IsAny<VisitUserDto>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task VisitUser_ShouldVisit()
        {
            // Arrange
            var responseMessage = "User successfully visited!";

            _mapperMock.Setup(x => x.Map<Visit>(It.IsAny<VisitUserDto>()))
                    .Returns(new Visit());

            _unitOfWorkMock.Setup(x => x.Users.VisitUser(It.IsAny<Visit>()))
                    .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                    .ReturnsAsync(true);
            // Act
            var result = await _sut.VisitUser(It.IsAny<VisitUserDto>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }
    }
}
