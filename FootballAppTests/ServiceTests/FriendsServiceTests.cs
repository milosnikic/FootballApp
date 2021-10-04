using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using FootballApp.API.Services.Friends;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FootballAppTests.ServiceTests
{
    public class FriendsServiceTests
    {
        private readonly FriendsService _sut;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

        public FriendsServiceTests()
        {
            _sut = new FriendsService(_mapperMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task AcceptFriendRequest_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var responseMessage = new KeyValuePair<bool, string>(true, "Test");
            _unitOfWorkMock.Setup(x => x.Friends.AcceptFriendRequest(It.IsAny<FriendRequestDto>()))
                            .ReturnsAsync(responseMessage);

            // Act
            var result = await _sut.AcceptFriendRequest(It.IsAny<FriendRequestDto>());

            // Assert
            Assert.Equal(responseMessage.Value, result.Value);
        }

        [Fact]
        public async Task DeleteFriendRequest_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var responseMessage = new KeyValuePair<bool, string>(true, "Test");
            _unitOfWorkMock.Setup(x => x.Friends.DeleteFriendRequest(It.IsAny<FriendRequestDto>()))
                            .ReturnsAsync(responseMessage);

            // Act
            var result = await _sut.DeleteFriendRequest(It.IsAny<FriendRequestDto>());

            // Assert
            Assert.Equal(responseMessage.Value, result.Value);
        }

        [Fact]
        public async Task SendFriendRequest_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var responseMessage = new KeyValuePair<bool, string>(true, "Test");
            _unitOfWorkMock.Setup(x => x.Friends.SendFriendRequest(It.IsAny<FriendRequestDto>()))
                            .ReturnsAsync(responseMessage);

            // Act
            var result = await _sut.SendFriendRequest(It.IsAny<FriendRequestDto>());

            // Assert
            Assert.Equal(responseMessage.Value, result.Value);
        }

        [Fact]
        public async Task GetAllExploreUsers_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var friends = new List<User>();
            var friend = new PowerUser()
            {
                Id = 1,
                Firstname = "Milos"
            };
            friends.Add(friend);

            var friendsToReturn = new List<ExploreUserDto>()
            { 
                new ExploreUserDto()
                {
                    Id = 1,
                    Firstname = "Milos"
                }
            };

            _unitOfWorkMock.Setup(x => x.Friends.GetAllExploreUsers(It.IsAny<int>()))
                            .ReturnsAsync(friends);

            _mapperMock.Setup(x => x.Map<ICollection<ExploreUserDto>>(It.IsAny<ICollection<User>>()))
                            .Returns(friendsToReturn);

            // Act
            var result = await _sut.GetAllExploreUsers(It.IsAny<int>());

            // Assert
            Assert.Equal(friend.Id, result.FirstOrDefault().Id);
            Assert.Equal(friend.Firstname, result.FirstOrDefault().Firstname);
        }

        [Fact]
        public async Task GetAllFriendsForUser_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var friends = new List<User>();
            var friend = new PowerUser()
            {
                Id = 1,
                Firstname = "Milos"
            };
            friends.Add(friend);

            var friendsToReturn = new List<ExploreUserDto>()
            {
                new ExploreUserDto()
                {
                    Id = 1,
                    Firstname = "Milos"
                }
            };

            _unitOfWorkMock.Setup(x => x.Friends.GetAllFriendsForUser(It.IsAny<int>()))
                            .ReturnsAsync(friends);

            _mapperMock.Setup(x => x.Map<ICollection<ExploreUserDto>>(It.IsAny<ICollection<User>>()))
                            .Returns(friendsToReturn);

            // Act
            var result = await _sut.GetAllFriendsForUser(It.IsAny<int>());

            // Assert
            Assert.Equal(friend.Id, result.FirstOrDefault().Id);
            Assert.Equal(friend.Firstname, result.FirstOrDefault().Firstname);
        }

        [Fact]
        public async Task PendingFriendRequests_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var friends = new List<User>();
            var friend = new PowerUser()
            {
                Id = 1,
                Firstname = "Milos"
            };
            friends.Add(friend);

            var friendsToReturn = new List<ExploreUserDto>()
            {
                new ExploreUserDto()
                {
                    Id = 1,
                    Firstname = "Milos"
                }
            };

            _unitOfWorkMock.Setup(x => x.Friends.PendingFriendRequests(It.IsAny<int>()))
                            .ReturnsAsync(friends);

            _mapperMock.Setup(x => x.Map<ICollection<ExploreUserDto>>(It.IsAny<ICollection<User>>()))
                            .Returns(friendsToReturn);

            // Act
            var result = await _sut.PendingFriendRequests(It.IsAny<int>());

            // Assert
            Assert.Equal(friend.Id, result.FirstOrDefault().Id);
            Assert.Equal(friend.Firstname, result.FirstOrDefault().Firstname);
        }

        [Fact]
        public async Task SentFriendRequests_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var friends = new List<User>();
            var friend = new PowerUser()
            {
                Id = 1,
                Firstname = "Milos"
            };
            friends.Add(friend);

            var friendsToReturn = new List<ExploreUserDto>()
            {
                new ExploreUserDto()
                {
                    Id = 1,
                    Firstname = "Milos"
                }
            };

            _unitOfWorkMock.Setup(x => x.Friends.SentFriendRequests(It.IsAny<int>()))
                            .ReturnsAsync(friends);

            _mapperMock.Setup(x => x.Map<ICollection<ExploreUserDto>>(It.IsAny<ICollection<User>>()))
                            .Returns(friendsToReturn);

            // Act
            var result = await _sut.SentFriendRequests(It.IsAny<int>());

            // Assert
            Assert.Equal(friend.Id, result.FirstOrDefault().Id);
            Assert.Equal(friend.Firstname, result.FirstOrDefault().Firstname);
        }
    }
}
