using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using FootballApp.API.Models.Views;
using FootballApp.API.Services.Matches;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FootballAppTests.ServiceTests
{
    public class MatchesServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly MatchesService _sut;

        public MatchesServiceTests()
        {
            _sut = new MatchesService(_mapperMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task ChechInForMatch_ShouldNotCheckInWhenMatchDoesNotExist()
        {
            // Arrange
            var responseMessage = "Specified match doesn't exist!";
            _unitOfWorkMock.Setup(x => x.Matchdays.GetById(It.IsAny<int>()))
                        .ReturnsAsync(() => null);

            // Act
            var response = await _sut.CheckInForMatch(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, response.Value);
        }

        [Fact]
        public async Task ChechInForMatch_ShouldNotCheckInWhenUserIsCheckedIn()
        {
            // Arrange
            var responseMessage = "You are already checked in!";
            _unitOfWorkMock.Setup(x => x.Matchdays.GetById(It.IsAny<int>()))
                        .ReturnsAsync(new Matchday());

            _unitOfWorkMock.Setup(x => x.MatchStatuses.GetMatchStatusById(It.IsAny<int>(), It.IsAny<int>()))
                        .ReturnsAsync(new MatchStatus());

            // Act
            var response = await _sut.CheckInForMatch(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, response.Value);
        }

        [Fact]
        public async Task ChechInForMatch_ShouldNotCheckInWhenNotSaved()
        {
            // Arrange
            var responseMessage = "Couldn't check in for match!";
            _unitOfWorkMock.Setup(x => x.Matchdays.GetById(It.IsAny<int>()))
                        .ReturnsAsync(new Matchday());

            _unitOfWorkMock.Setup(x => x.MatchStatuses.GetMatchStatusById(It.IsAny<int>(), It.IsAny<int>()))
                        .ReturnsAsync(() => null);

            _unitOfWorkMock.Setup(x => x.MatchStatuses.Add(new MatchStatus()))
                        .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                        .ReturnsAsync(false);

            // Act
            var response = await _sut.CheckInForMatch(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, response.Value);
        }

        [Fact]
        public async Task ChechInForMatch_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var responseMessage = "Successfully checked in for match!";
            _unitOfWorkMock.Setup(x => x.Matchdays.GetById(It.IsAny<int>()))
                        .ReturnsAsync(new Matchday());

            _unitOfWorkMock.Setup(x => x.MatchStatuses.GetMatchStatusById(It.IsAny<int>(), It.IsAny<int>()))
                        .ReturnsAsync(() => null);

            _unitOfWorkMock.Setup(x => x.MatchStatuses.Add(new MatchStatus()))
                        .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                        .ReturnsAsync(true);

            // Act
            var response = await _sut.CheckInForMatch(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, response.Value);
        }

        [Fact]
        public async Task ConfirmForMatch_ShouldNotConfirm_WhenMatchDoesNotExist()
        {
            // Arrange
            var responseMessage = "Specified match doesn't exist!";
            _unitOfWorkMock.Setup(x => x.Matchdays.GetById(It.IsAny<int>()))
                        .ReturnsAsync(() => null);

            // Act
            var result = await _sut.ConfirmForMatch(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task ConfirmForMatch_ShouldNotConfirm_WhenUserIsNotCheckedInForMatch()
        {
            // Arrange
            var responseMessage = "You are not checked in for match.";
            _unitOfWorkMock.Setup(x => x.Matchdays.GetById(It.IsAny<int>()))
                        .ReturnsAsync(new Matchday());

            _unitOfWorkMock.Setup(x => x.MatchStatuses.GetMatchStatusById(It.IsAny<int>(), It.IsAny<int>()))
                        .ReturnsAsync(() => null);

            // Act
            var result = await _sut.ConfirmForMatch(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task ConfirmForMatch_ShouldNotConfirm_WhenNotSaved()
        {
            // Arrange
            var responseMessage = "Couldn't confirm for match!";
            _unitOfWorkMock.Setup(x => x.Matchdays.GetById(It.IsAny<int>()))
                        .ReturnsAsync(new Matchday());

            _unitOfWorkMock.Setup(x => x.MatchStatuses.GetMatchStatusById(It.IsAny<int>(), It.IsAny<int>()))
                        .ReturnsAsync(new MatchStatus());

            _unitOfWorkMock.Setup(x => x.Complete())
                        .ReturnsAsync(false);

            // Act
            var result = await _sut.ConfirmForMatch(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task ConfirmForMatch_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var responseMessage = "Successfully confirmed for match!";
            _unitOfWorkMock.Setup(x => x.Matchdays.GetById(It.IsAny<int>()))
                        .ReturnsAsync(new Matchday());

            _unitOfWorkMock.Setup(x => x.MatchStatuses.GetMatchStatusById(It.IsAny<int>(), It.IsAny<int>()))
                        .ReturnsAsync(new MatchStatus());

            _unitOfWorkMock.Setup(x => x.Complete())
                        .ReturnsAsync(true);

            // Act
            var result = await _sut.ConfirmForMatch(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task CreateMatch_ShouldNotCreate_WhenGroupDoesNotExist()
        {
            // Arrange
            var responseMessage = "Specified group doesn't exist.";
            _unitOfWorkMock.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                        .ReturnsAsync(() => null);

            // Act
            var result = await _sut.CreateMatch(new MatchdayForCreationDto(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task CreateMatch_ShouldNotCreate_WhenUserDoesNotHaveAccess()
        {
            // Arrange
            var responseMessage = "You are not allowed to create match!";
            _unitOfWorkMock.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                        .ReturnsAsync(new Group());

            _unitOfWorkMock.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                        .ReturnsAsync(() => null);

            // Act
            var result = await _sut.CreateMatch(new MatchdayForCreationDto(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task CreateMatch_ShouldCreate_WhenLocationDoesNotExist()
        {
            // Arrange
            var responseMessage = "Matchday has been successfully created.";
            var membership = new Membership()
            {
                MembershipStatus = MembershipStatus.Accepted,
            };
            _unitOfWorkMock.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                        .ReturnsAsync(new Group());

            _unitOfWorkMock.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                        .ReturnsAsync(membership);

            _mapperMock.Setup(x => x.Map<Matchday>(It.IsAny<MatchdayForCreationDto>()))
                        .Returns(new Matchday());

            _unitOfWorkMock.Setup(x => x.Locations.GetByName(It.IsAny<string>()))
                        .ReturnsAsync(() => null);

            _unitOfWorkMock.Setup(x => x.Locations.Add(new Location()))
                        .Verifiable();

            _unitOfWorkMock.Setup(x => x.Matchdays.Add(new Matchday()))
                        .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                        .ReturnsAsync(true);


            // Act
            var result = await _sut.CreateMatch(new MatchdayForCreationDto(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task CreateMatch_ShouldNotCreate_WhenNotSaved()
        {
            // Arrange
            var responseMessage = "Matchday has not been created.";
            var membership = new Membership()
            {
                MembershipStatus = MembershipStatus.Accepted,
            };
            _unitOfWorkMock.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                        .ReturnsAsync(new Group());

            _unitOfWorkMock.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                        .ReturnsAsync(membership);

            _mapperMock.Setup(x => x.Map<Matchday>(It.IsAny<MatchdayForCreationDto>()))
                        .Returns(new Matchday());

            _unitOfWorkMock.Setup(x => x.Locations.GetByName(It.IsAny<string>()))
                        .ReturnsAsync(() => null);

            _unitOfWorkMock.Setup(x => x.Locations.Add(new Location()))
                        .Verifiable();

            _unitOfWorkMock.Setup(x => x.Matchdays.Add(new Matchday()))
                        .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                        .ReturnsAsync(false);


            // Act
            var result = await _sut.CreateMatch(new MatchdayForCreationDto(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task GetLatestFiveMatchesForUser_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var datePlayed = DateTime.Now;
            var latestMatches = new List<LatestFiveMatchesView>()
            {
                new LatestFiveMatchesView()
                {
                    Id = 1,
                    DatePlayed = datePlayed,
                    Goals = 0,
                    HomeName = "Test",
                    AwayName = "Test",
                    Assists = 1,
                    Place = "Test",
                    Rating = 10,
                    Result = "1 - 0"
                }
            };
            _unitOfWorkMock.Setup(x => x.Matchdays.GetLatestFiveMatchesForUser(It.IsAny<int>()))
                        .ReturnsAsync(latestMatches);

            // Act
            var result = await _sut.GetLatestFiveMatchesForUser(It.IsAny<int>());

            // Assert
            var firstMatch = result.FirstOrDefault();
            Assert.NotNull(firstMatch);
            Assert.Equal(datePlayed, firstMatch.DatePlayed);
            Assert.Equal(1, firstMatch.Id);
            Assert.Equal(0, firstMatch.Goals);
            Assert.Equal("Test", firstMatch.HomeName);
            Assert.Equal("Test", firstMatch.AwayName);
            Assert.Equal(1, firstMatch.Assists);
            Assert.Equal(10, firstMatch.Rating);
            Assert.Equal("Test", firstMatch.Place);
            Assert.Equal("1 - 0", firstMatch.Result);
        }

        [Fact]
        public async Task GetMatchHistoryForGroup_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var datePlayed = DateTime.Now;
            var latestMatches = new List<GroupMatchHistoryView>()
            {
                new GroupMatchHistoryView()
                {
                    Id = 1,
                    DatePlaying = datePlayed,
                }
            };
            _unitOfWorkMock.Setup(x => x.Matchdays.GetMatchHistoryForGroup(It.IsAny<int>()))
                        .ReturnsAsync(latestMatches);

            // Act
            var result = await _sut.GetMatchHistoryForGroup(It.IsAny<int>());

            // Assert
            var firstMatch = result.FirstOrDefault();
            Assert.NotNull(firstMatch);
            Assert.Equal(datePlayed, firstMatch.DatePlaying);
            Assert.Equal(1, firstMatch.Id);
        }

        [Fact]
        public async Task GetMatchHistoryForUser_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var datePlayed = DateTime.Now;
            var latestMatches = new List<MatchHistoryView>()
            {
                new MatchHistoryView()
                {
                    Id = 1,
                    DatePlayed = datePlayed,
                }
            };
            _unitOfWorkMock.Setup(x => x.Matchdays.GetMatchHistoryForUser(It.IsAny<int>()))
                        .ReturnsAsync(latestMatches);

            // Act
            var result = await _sut.GetMatchHistoryForUser(It.IsAny<int>());

            // Assert
            var firstMatch = result.FirstOrDefault();
            Assert.NotNull(firstMatch);
            Assert.Equal(datePlayed, firstMatch.DatePlayed);
            Assert.Equal(1, firstMatch.Id);
        }

        [Fact]
        public async Task GetOrganizedMatchInformation_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var datePlayed = DateTime.Now;
            var latestMatches = new List<OrganizedMatchInformationView>()
            {
                new OrganizedMatchInformationView()
                {
                    Id = 1,
                    DatePlayed = datePlayed,
                }
            };
            _unitOfWorkMock.Setup(x => x.Matchdays.GetOrganizedMatchInformation(It.IsAny<int>()))
                        .ReturnsAsync(latestMatches);

            // Act
            var result = await _sut.GetOrganizedMatchInformation(It.IsAny<int>());

            // Assert
            var firstMatch = result.FirstOrDefault();
            Assert.NotNull(firstMatch);
            Assert.Equal(datePlayed, firstMatch.DatePlayed);
            Assert.Equal(1, firstMatch.Id);
        }

        [Fact]
        public async Task GetUpcomingMatchday_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var matchdayId = 1;
            var datePlaying = DateTime.Now;
            var matchday = new Matchday()
            {
                Id = matchdayId,
                DatePlaying = datePlaying
            };

            var matchdayToReturnDto = new MatchdayToReturnDto()
            {
                Id = matchdayId,
                DatePlaying = datePlaying
            };

            _unitOfWorkMock.Setup(x => x.Matchdays.GetMatchdayWithAdditionalInformation(It.IsAny<int>()))
                        .ReturnsAsync(matchday);

            _mapperMock.Setup(x => x.Map<MatchdayToReturnDto>(matchday))
                        .Returns(matchdayToReturnDto);

            // Act
            var result = await _sut.GetUpcomingMatchday(It.IsAny<int>());

            // Assert
            Assert.Equal(matchdayId, result.Id);
            Assert.Equal(datePlaying, result.DatePlaying);
        }

        [Fact]
        public async Task GetUpcomingMatchesApplicableForUser_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var matchdayId = 1;
            var datePlaying = DateTime.Now;
            var matchdays = new List<Matchday>()
            {
              new Matchday()
                {
                    Id = matchdayId,
                    DatePlaying = datePlaying
                }
            };

            var matchdaysToReturnDto = new List<MatchdayToReturnDto>()
            {
                new MatchdayToReturnDto()
                {
                    Id = matchdayId,
                    DatePlaying = datePlaying
                }
            };

            _unitOfWorkMock.Setup(x => x.Matchdays.GetUpcomingMatchesApplicableForUser(It.IsAny<int>()))
                            .ReturnsAsync(matchdays);

            _mapperMock.Setup(x => x.Map<ICollection<MatchdayToReturnDto>>(matchdays))
                            .Returns(matchdaysToReturnDto);

            // Act
            var result = await _sut.GetUpcomingMatchesApplicableForUser(It.IsAny<int>());

            // Assert
            var firstMatch = result.FirstOrDefault();
            Assert.Equal(matchdayId, firstMatch.Id);
            Assert.Equal(datePlaying, firstMatch.DatePlaying);
        }

        [Fact]
        public async Task GetUpcomingMatchesForGroup_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var matchdayId = 1;
            var datePlaying = DateTime.Now;
            var matchdays = new List<Matchday>()
            {
              new Matchday()
                {
                    Id = matchdayId,
                    DatePlaying = datePlaying
                }
            };

            var matchdaysToReturnDto = new List<MatchdayToReturnDto>()
            {
                new MatchdayToReturnDto()
                {
                    Id = matchdayId,
                    DatePlaying = datePlaying
                }
            };

            _unitOfWorkMock.Setup(x => x.Matchdays.GetUpcomingMatchesForGroup(It.IsAny<int>()))
                            .ReturnsAsync(matchdays);

            _mapperMock.Setup(x => x.Map<ICollection<MatchdayToReturnDto>>(matchdays))
                            .Returns(matchdaysToReturnDto);

            // Act
            var result = await _sut.GetUpcomingMatchesForGroup(It.IsAny<int>());

            // Assert
            var firstMatch = result.FirstOrDefault();
            Assert.Equal(matchdayId, firstMatch.Id);
            Assert.Equal(datePlaying, firstMatch.DatePlaying);
        }

        [Fact]
        public async Task GetUpcomingMatchesForUser_ShouldReturnEmptyList_WhenUserDoesNotExist()
        {
            // Arrange
            _unitOfWorkMock.Setup(x => x.Users.GetUserByIdWithAdditionalInformation(It.IsAny<int>()))
                            .ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetUpcomingMatchesForUser(It.IsAny<int>());

            // Assert
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public async Task GetUpcomingMatchesForUser_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var userId = 1;
            var matchdayId = 1;
            var matchdays = new List<MatchStatus>()
            {
              new MatchStatus()
                {
                    UserId = userId,
                    MatchdayId = matchdayId
                }
            };

            var matchdaysForDisplayDto = new List<MatchdayForDisplayDto>()
            {
                new MatchdayForDisplayDto()
                {
                    Id = matchdayId
                }
            };

            _unitOfWorkMock.Setup(x => x.Users.GetUserByIdWithAdditionalInformation(It.IsAny<int>()))
                            .ReturnsAsync(new PowerUser());

            _unitOfWorkMock.Setup(x => x.Matchdays.GetUpcomingMatchesForUser(It.IsAny<int>()))
                            .ReturnsAsync(matchdays);

            _mapperMock.Setup(x => x.Map<ICollection<MatchdayForDisplayDto>>(matchdays))
                            .Returns(matchdaysForDisplayDto);



            // Act
            var result = await _sut.GetUpcomingMatchesForUser(It.IsAny<int>());

            // Assert
            var firstMatch = result.FirstOrDefault();
            Assert.Equal(matchdayId, firstMatch.Id);
        }

        [Fact]
        public async Task GetUserStatusForMatchday_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var datePlaying = DateTime.Now;
            var matchdayId = 1;
            var userId = 1;
            var matchday = new Matchday()
            {
                Id = matchdayId,
                DatePlaying = datePlaying
            };

            var matchstatus = new MatchStatus()
            { 
                UserId = userId,
                MatchdayId = matchdayId
            };

            var matchStatusToReturnDto = new MatchStatusToReturnDto()
            {
                Checked = true
            };

            _unitOfWorkMock.Setup(x => x.Matchdays.GetById(It.IsAny<int>()))
                            .ReturnsAsync(matchday);

            _unitOfWorkMock.Setup(x => x.MatchStatuses.GetMatchStatusById(It.IsAny<int>(), It.IsAny<int>()))
                            .ReturnsAsync(matchstatus);

            _mapperMock.Setup(x => x.Map<MatchStatusToReturnDto>(matchstatus))
                            .Returns(matchStatusToReturnDto);

            // Act
            var result = await _sut.GetUserStatusForMatchday(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.True(result.Checked);
        }

        [Fact]
        public async Task GetUserStatusForMatchday_ShouldReturnEmptyMatchStatus_WhenMatchdayDoesNotExist()
        {
            // Arrange
            _unitOfWorkMock.Setup(x => x.Matchdays.GetById(It.IsAny<int>()))
                            .ReturnsAsync(() => null);
            
            // Act
            var result = await _sut.GetUserStatusForMatchday(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Checked);
            Assert.False(result.Confirmed);
        }

        [Fact]
        public async Task GiveUpForMatch_ShouldNotSave_WhenMatchDoesNotExist()
        {
            // Arrange
            var responseMessage = "Specified match doesn't exist!";
            _unitOfWorkMock.Setup(x => x.Matchdays.GetById(It.IsAny<int>()))
                            .ReturnsAsync(() => null);

            // Act
            var result = await _sut.GiveUpForMatch(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task GiveUpForMatch_ShouldNotSave_WhenMatchStatusDoesNotExist()
        {
            // Arrange
            var responseMessage = "You are not checked in for match.";
            _unitOfWorkMock.Setup(x => x.Matchdays.GetById(It.IsAny<int>()))
                            .ReturnsAsync(new Matchday());

            _unitOfWorkMock.Setup(x => x.MatchStatuses.GetMatchStatusById(It.IsAny<int>(), It.IsAny<int>()))
                            .ReturnsAsync(() => null);

            // Act
            var result = await _sut.GiveUpForMatch(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task GiveUpForMatch_ShouldNotSave()
        {
            // Arrange
            var responseMessage = "Couldn't give up for match!";
            _unitOfWorkMock.Setup(x => x.Matchdays.GetById(It.IsAny<int>()))
                            .ReturnsAsync(new Matchday());

            _unitOfWorkMock.Setup(x => x.MatchStatuses.GetMatchStatusById(It.IsAny<int>(), It.IsAny<int>()))
                            .ReturnsAsync(new MatchStatus());

            _unitOfWorkMock.Setup(x => x.MatchStatuses.Remove(new MatchStatus()))
                            .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                            .ReturnsAsync(false);

            // Act
            var result = await _sut.GiveUpForMatch(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task GiveUpForMatch_ShouldSave()
        {
            // Arrange
            var responseMessage = "Successfully gave up a match!";
            _unitOfWorkMock.Setup(x => x.Matchdays.GetById(It.IsAny<int>()))
                            .ReturnsAsync(new Matchday());

            _unitOfWorkMock.Setup(x => x.MatchStatuses.GetMatchStatusById(It.IsAny<int>(), It.IsAny<int>()))
                            .ReturnsAsync(new MatchStatus());

            _unitOfWorkMock.Setup(x => x.MatchStatuses.Remove(new MatchStatus()))
                            .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                            .ReturnsAsync(true);

            // Act
            var result = await _sut.GiveUpForMatch(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }

        [Fact]
        public async Task OrganizeMatch_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var responseMessage = "Successfully added";
            var returnValuePair = new KeyValuePair<bool, string>(true, responseMessage);
            _unitOfWorkMock.Setup(x => x.Matchdays.OrganizeMatchPlayed(It.IsAny<OrganizeMatchDto>()))
                            .ReturnsAsync(returnValuePair);

            // Act
            var result = await _sut.OrganizeMatch(It.IsAny<OrganizeMatchDto>());

            // Assert
            Assert.Equal(responseMessage, result.Value);
        }
    }
}
