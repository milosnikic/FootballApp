using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Hubs;
using FootballApp.API.Models;
using FootballApp.API.Services.Chat;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace FootballAppTests.ServiceTests
{
    public class ChatsServiceTests
    {
        private readonly IChatsService _sut;
        private readonly Mock<IHubContext<ChatHub>> _chatHubMock = new Mock<IHubContext<ChatHub>>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        public ChatsServiceTests()
        {
            _sut = new ChatsService(_chatHubMock.Object, _unitOfWorkMock.Object, _mapper.Object);
        }

        [Fact]
        public async Task GetChatWithMessages_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var chat = new Chat()
            {
                Id = 1,
                Name = "Private chat",
                Type = ChatType.Private,
            };

            var chatDto = new ChatToReturnDto()
            {
                Id = 1,
                Name = "Private chat",
                Type = ChatType.Private,
            };

            _unitOfWorkMock.Setup(x => x.Chats.GetChatWithMessages(It.IsAny<int>()))
                .ReturnsAsync(chat);

            _mapper.Setup(x => x.Map<ChatToReturnDto>(It.IsAny<Chat>()))
                .Returns(chatDto);

            // Act
            var result = await _sut.Chat(It.IsAny<int>());

            // Assert
            Assert.Equal(chat.Id, result.Id);
            Assert.Equal(chat.Name, result.Name);
        }

        [Fact]
        public async Task CreateGroupChat_ShouldBeDoneSuccessfully()
        {
            // Arrange
            _unitOfWorkMock.Setup(x => x.Chats.CreateGroupChat(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(new KeyValuePair<bool, string>(true, It.IsAny<string>()));

            _unitOfWorkMock.Setup(x => x.Complete())
                .Verifiable();

            // Act
            var result = await _sut.CreateGroupChat(It.IsAny<string>(), It.IsAny<int>());

            // Assert
            Assert.True(result.Key);
        }

        [Fact]
        public async Task CreatePrivateChat_ShouldBeDoneSuccessfully()
        {
            // Arrange
            _unitOfWorkMock.Setup(x => x.Chats.CreatePrivateChat(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new KeyValuePair<bool, string>(true, It.IsAny<string>()));

            _unitOfWorkMock.Setup(x => x.Complete())
                .Verifiable();

            // Act
            var result = await _sut.CreatePrivateChat(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.True(result.Key);
        }

        [Fact]
        public async Task GetAvailableUsers_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var userDtoList = new List<UserToReturnMiniDto>()
            {
                new UserToReturnMiniDto()
                {
                    Id = 1,
                    Firstname = "Test",
                    Gender = "Male",
                    Lastname = "Test",
                    Username = "test",
                }
            };
            _unitOfWorkMock.Setup(x => x.Chats.GetAvailableUsers(It.IsAny<int>()))
                .ReturnsAsync(It.IsAny<List<User>>());

            _mapper.Setup(x => x.Map<ICollection<UserToReturnMiniDto>>(It.IsAny<List<User>>()))
                .Returns(userDtoList);


            // Act
            var result = await _sut.GetAvailableUsers(It.IsAny<int>());

            // Assert
            var userMini = result.FirstOrDefault();
            Assert.NotNull(userMini);
            Assert.Equal(1, userMini.Id);
            Assert.Equal("Test", userMini.Firstname);
            Assert.Equal("Test", userMini.Lastname);
            Assert.Equal("test", userMini.Username);
            Assert.Equal("Male", userMini.Gender);
        }

        [Fact]
        public async Task GetPrivateChats_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var lastMessageDateTime = DateTime.Now;
            var userDtoList = new List<UserToReturnMiniDto>();
            var messageDtoList = new List<MessageToReturnDto>();
            var chatDtoList = new List<ChatToReturnDto>()
            {
                new ChatToReturnDto()
                {
                    Id = 1,
                    Name = "Test",
                    Type = ChatType.Private,
                    LastMessage = lastMessageDateTime,
                    Messages = messageDtoList,
                    Users = userDtoList
                }
            };
            _unitOfWorkMock.Setup(x => x.Chats.GetPrivateChats(It.IsAny<int>()))
                .ReturnsAsync(It.IsAny<List<Chat>>());

            _mapper.Setup(x => x.Map<ICollection<ChatToReturnDto>>(It.IsAny<List<Chat>>()))
                .Returns(chatDtoList);


            // Act
            var result = await _sut.GetPrivateChats(It.IsAny<int>());

            // Assert
            var chatDto = result.FirstOrDefault();
            Assert.NotNull(chatDto);
            Assert.Equal(1, chatDto.Id);
            Assert.Equal("Test", chatDto.Name);
            Assert.Equal(userDtoList, chatDto.Users);
            Assert.Equal(ChatType.Private, chatDto.Type);
            Assert.Equal(messageDtoList, chatDto.Messages);
            Assert.Equal(lastMessageDateTime, chatDto.LastMessage);
        }

        [Fact]
        public async Task JoinRoom_ShouldBeDoneSuccessfully()
        {
            // Arrange
            _chatHubMock.Setup(x =>
                x.Groups.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), new CancellationToken())).Verifiable();

            // Act
            await _sut.JoinRoom(It.IsAny<string>(), It.IsAny<string>());

            // Assert
        }

        [Fact]
        public async Task LeaveRoom_ShouldBeDoneSuccessfully()
        {
            // Arrange
            _chatHubMock.Setup(x =>
                    x.Groups.RemoveFromGroupAsync(It.IsAny<string>(), It.IsAny<string>(), new CancellationToken()))
                .Verifiable();

            // Act
            await _sut.LeaveRoom(It.IsAny<string>(), It.IsAny<string>());

            // Assert
        }

        [Fact]
        public async Task JoinRoom_NotSignalR_ShouldBeDoneSuccessfully()
        {
            // Arrange
            _unitOfWorkMock.Setup(x => x.Chats.JoinRoom(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new KeyValuePair<bool, string>(true, It.IsAny<string>()));

            _unitOfWorkMock.Setup(x => x.Complete())
                .Verifiable();

            // Act
            var result = await _sut.JoinRoom(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.True(result.Key);
        }

        [Fact]
        public async Task SendMessage_ShouldThrowException()
        {
            // Arrange
            var dateMessageSent = DateTime.Now;
            var message = new Message()
            {
                ChatId = 1,
                Content = "Test",
                MessageSent = dateMessageSent,
                SenderId = 1
            };

            var user = new PowerUser()
            {
                Created = DateTime.Now,
                Id = 1,
                Email = "test",
                City = new City(),
                Chats = new List<ChatUser>(),
                Commented = new List<Comment>(),
                Comments = new List<Comment>(),
                Country = new Country(),
                Firstname = "Test",
                Gender = Gender.Male,
                Lastname = "Test",
                Memberships = new List<Membership>(),
                Messages = new List<Message>(),
                Photos = new List<Photo>(),
                Username = "test",
                Visiteds = new List<Visit>(),
                Visitors = new List<Visit>(),
                CityId = 1,
                CountryId = 1,
                FriendshipsReceived = new List<Friendship>(),
                FriendshipsSent = new List<Friendship>(),
                GainedAchievements = new List<GainedAchievement>(),
                GroupsCreated = new List<Group>(),
                IsActive = true,
                LastActive = DateTime.Now,
                MatchStatuses = new List<MatchStatus>(),
                TeamMembers = new List<TeamMember>(),
                DateOfBirth = DateTime.Now,
                NumberOfGroupsCreated = 5
            };

            _unitOfWorkMock.Setup(x => x.Messages.Add(message))
                .Verifiable();

            _unitOfWorkMock.Setup(x => x.Complete())
                .Verifiable();

            _unitOfWorkMock.Setup(x => x.Users.GetUserByIdWithAdditionalInformation(It.IsAny<int>()))
                .ReturnsAsync(user);

            _chatHubMock.Setup(x => x.Clients.Group(It.IsAny<string>()))
                .Verifiable();

            // Act
            var result = await _sut.SendMessage(It.IsAny<MessageToSendDto>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Key);
        }
    }
}