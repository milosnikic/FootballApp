using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using FootballApp.API.Services.Groups;
using Moq;
using Xunit;

namespace FootballAppTests.ServiceTests
{
    public class GroupsServiceTests
    {
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
        private readonly IGroupsService _sut;

        public GroupsServiceTests()
        {
            _sut = new GroupsService(_mapperMock.Object, _unitOfWork.Object);
        }

        [Fact]
        public async Task AcceptUser_ShouldReturnFalse_WhenGroupIsMissing()
        {
            // Arrange
            var expectedMessage = "Specified group doesn't exist!";
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.AcceptUser(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }

        [Fact]
        public async Task AcceptUser_ShouldReturnFalse_WhenUserIsMissing()
        {
            // Arrange
            var expectedMessage = "Specified user doesn't exist!";
            var groupId = 1;
            var groupDescription = "Test";
            var groupName = "Test";
            var creator = new PowerUser()
            {
                Id = 1
            };
            var dateCreated = DateTime.Now;
            var isActive = true;
            var locationId = 1;
            var group = new Group()
            {
                Id = groupId,
                Description = groupDescription,
                Location = new Location(),
                Matchdays = new List<Matchday>(),
                Memberships = new List<Membership>(),
                Name = groupName,
                CreatedBy = creator,
                DateCreated = dateCreated,
                IsActive = isActive,
                LocationId = locationId
            };

            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(group);

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.AcceptUser(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }

        [Fact]
        public async Task AcceptUser_ShouldReturnFalse_WhenMembershipIsMissing()
        {
            // Arrange
            var expectedMessage = "You have to ask to join to group!";
            var groupId = 1;
            var groupDescription = "Test";
            var groupName = "Test";
            var creator = new PowerUser()
            {
                Id = 1
            };
            var dateCreated = DateTime.Now;
            var isActive = true;
            var locationId = 1;
            var group = new Group()
            {
                Id = groupId,
                Description = groupDescription,
                Location = new Location(),
                Matchdays = new List<Matchday>(),
                Memberships = new List<Membership>(),
                Name = groupName,
                CreatedBy = creator,
                DateCreated = dateCreated,
                IsActive = isActive,
                LocationId = locationId
            };

            var membership = new Membership()
            {
                Accepted = true,
                Favorite = true,
                Group = group,
                Role = Role.Admin,
                User = creator,
                DateAccepted = DateTime.Now,
                DateSent = DateTime.Now,
                GroupId = groupId,
                MembershipStatus = MembershipStatus.Accepted,
                UserId = creator.Id
            };

            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(group);

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(creator);

            _unitOfWork.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.AcceptUser(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }

        [Fact]
        public async Task AcceptUser_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var groupId = 1;
            var groupDescription = "Test";
            var groupName = "Test";
            var creator = new PowerUser()
            {
                Id = 1
            };
            var dateCreated = DateTime.Now;
            var isActive = true;
            var locationId = 1;
            var group = new Group()
            {
                Id = groupId,
                Description = groupDescription,
                Location = new Location(),
                Matchdays = new List<Matchday>(),
                Memberships = new List<Membership>(),
                Name = groupName,
                CreatedBy = creator,
                DateCreated = dateCreated,
                IsActive = isActive,
                LocationId = locationId
            };

            var membership = new Membership()
            {
                Accepted = true,
                Favorite = true,
                Group = group,
                Role = Role.Admin,
                User = creator,
                DateAccepted = DateTime.Now,
                DateSent = DateTime.Now,
                GroupId = groupId,
                MembershipStatus = MembershipStatus.Accepted,
                UserId = creator.Id
            };

            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(group);

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(creator);

            _unitOfWork.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(membership);

            _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(true);

            // Act
            var result = await _sut.AcceptUser(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.True(result.Key);
        }

        [Fact]
        public async Task AcceptUser_ShouldBeReturnFalse_WhenIsNotCompleted()
        {
            // Arrange
            var expectedMessage = "User hasn't joined group!";
            var groupId = 1;
            var groupDescription = "Test";
            var groupName = "Test";
            var creator = new PowerUser()
            {
                Id = 1
            };
            var dateCreated = DateTime.Now;
            var isActive = true;
            var locationId = 1;
            var group = new Group()
            {
                Id = groupId,
                Description = groupDescription,
                Location = new Location(),
                Matchdays = new List<Matchday>(),
                Memberships = new List<Membership>(),
                Name = groupName,
                CreatedBy = creator,
                DateCreated = dateCreated,
                IsActive = isActive,
                LocationId = locationId
            };

            var membership = new Membership()
            {
                Accepted = true,
                Favorite = true,
                Group = group,
                Role = Role.Admin,
                User = creator,
                DateAccepted = DateTime.Now,
                DateSent = DateTime.Now,
                GroupId = groupId,
                MembershipStatus = MembershipStatus.Accepted,
                UserId = creator.Id
            };

            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(group);

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(creator);

            _unitOfWork.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(membership);

            _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(false);

            // Act
            var result = await _sut.AcceptUser(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }

        [Fact]
        public async Task CreateGroup_ShouldReturnFalse_WhenUserDoesntExist()
        {
            // Arrange
            var expectedMessage = "User doesn't exist!";
            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.CreateGroup(It.IsAny<int>(), It.IsAny<GroupForCreationDto>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }

        [Fact]
        public async Task CreateGroup_ShouldReturnFalse_WhenUserDoesntHaveAccesas()
        {
            // Arrange
            var expectedMessage = "Unauthorized";
            var commonUser = new CommonUser()
            {
                Id = 1
            };
            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(commonUser);

            // Act
            var result = await _sut.CreateGroup(It.IsAny<int>(), It.IsAny<GroupForCreationDto>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }

        [Fact]
        public async Task CreateGroup_ShouldReturnFalse_WhenLocationDoesntExist_AndIsNotCompleted()
        {
            // Arrange
            var expectedMessage = "Couldn't create group";
            var groupDto = new GroupForCreationDto()
            {
                Image = null,
            };
            var powerUser = new PowerUser()
            {
                Id = 1
            };
            var group = new Group()
            {
                Id = 1,
                Image = null
            };
            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(powerUser);

            _mapperMock.Setup(x => x.Map<Group>(It.IsAny<GroupForCreationDto>()))
                .Returns(group);

            _unitOfWork.Setup(x => x.Locations.GetByName(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _unitOfWork.Setup(x => x.Groups.Add(group)).Verifiable();

            _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(false);

            // Act
            var result = await _sut.CreateGroup(It.IsAny<int>(), groupDto);

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }

        [Fact]
        public async Task CreateGroup_ShouldReturnTrue_WhenLocationDoesntExist_AndIsCompleted()
        {
            // Arrange
            var expectedMessage = "Group created successfully!";
            var groupDto = new GroupForCreationDto()
            {
                Image = null,
            };
            var powerUser = new PowerUser()
            {
                Id = 1
            };
            var group = new Group()
            {
                Id = 1,
                Image = null
            };
            var membership = new Membership()
            {
                UserId = powerUser.Id,
                Accepted = true,
                Favorite = true,
                Group = null,
                Role = Role.Member,
                User = powerUser,
                DateAccepted = DateTime.Now,
                DateSent = DateTime.Now,
                GroupId = group.Id,
                MembershipStatus = MembershipStatus.Sent
            };

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(powerUser);

            _mapperMock.Setup(x => x.Map<Group>(It.IsAny<GroupForCreationDto>()))
                .Returns(group);

            _unitOfWork.Setup(x => x.Locations.GetByName(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _unitOfWork.Setup(x => x.Groups.Add(group)).Verifiable();

            _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(true);

            _unitOfWork.Setup(x => x.Memberships.Add(membership)).Verifiable();


            // Act
            var result = await _sut.CreateGroup(It.IsAny<int>(), groupDto);

            // Assert
            Assert.True(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }

        [Fact]
        public async Task CreateGroup_ShouldReturnFalse_WhenLocationDoesntExist_AndIsNotCompletedForAll()
        {
            // Arrange
            var expectedMessage = "Something went wrong!";
            var groupDto = new GroupForCreationDto()
            {
                Image = null,
            };
            var powerUser = new PowerUser()
            {
                Id = 1
            };
            var group = new Group()
            {
                Id = 1,
                Image = null
            };
            var membership = new Membership()
            {
                UserId = powerUser.Id,
                Accepted = true,
                Favorite = true,
                Group = null,
                Role = Role.Member,
                User = powerUser,
                DateAccepted = DateTime.Now,
                DateSent = DateTime.Now,
                GroupId = group.Id,
                MembershipStatus = MembershipStatus.Sent
            };

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(powerUser);

            _mapperMock.Setup(x => x.Map<Group>(It.IsAny<GroupForCreationDto>()))
                .Returns(group);

            _unitOfWork.Setup(x => x.Locations.GetByName(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _unitOfWork.Setup(x => x.Groups.Add(group)).Verifiable();

            _unitOfWork.SetupSequence(x => x.Complete()).ReturnsAsync(true).ReturnsAsync(false);

            _unitOfWork.Setup(x => x.Memberships.Add(membership)).Verifiable();

            // Act
            var result = await _sut.CreateGroup(It.IsAny<int>(), groupDto);

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }

        [Fact]
        public async Task GetAllCreatedGroups_ShouldReturnNull_WhenUserIsNotPowerUser()
        {
            // Arrange
            _unitOfWork.Setup(x => x.Users.GetUserByIdWithAdditionalInformation(It.IsAny<int>()))
                .ReturnsAsync(new CommonUser());

            // Act
            var result = await _sut.GetAllCreatedGroups(It.IsAny<int>());

            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task GetAllCreatedGroups_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var groupsToReturn = new List<GroupToReturnDto>()
            {
                new GroupToReturnDto()
                {
                    Id = 1
                }
            };

            var memberships = new List<Membership>()
            {
                new Membership()
                {
                    UserId = 1,
                }
            };

            var powerUser = new PowerUser()
            {
                Id = 1
            };
            _unitOfWork.Setup(x => x.Users.GetUserByIdWithAdditionalInformation(It.IsAny<int>()))
                .ReturnsAsync(powerUser);

            _unitOfWork.Setup(x => x.Groups.GetCreatedGroupsForUser(powerUser))
                .ReturnsAsync(memberships);

            _mapperMock.Setup(x => x.Map<ICollection<GroupToReturnDto>>(memberships))
                .Returns(groupsToReturn);

            // Act
            var result = await _sut.GetAllCreatedGroups(It.IsAny<int>());

            // Assert
            var group = result.FirstOrDefault();
            Assert.NotNull(group);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(1, group.Id);
        }
        
        [Fact]
        public async Task GetAllGroups_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var groups = new List<Group>()
            {
                new Group()
                {
                    Id = 1,
                }
            };

            var groupsToReturn = new List<GroupToReturnDto>()
            {
                new GroupToReturnDto()
                {
                    Id = 1,
                }
            };

            _unitOfWork.Setup(x => x.Groups.GetAllGroupsWithInclude(It.IsAny<int>()))
                .ReturnsAsync(groups);

            _mapperMock.Setup(x => x.Map<ICollection<GroupToReturnDto>>(groups))
                .Returns(groupsToReturn);
            
            // Act
            var result = await _sut.GetAllGroups(It.IsAny<int>());

            // Assert
            var group = result.FirstOrDefault();
            Assert.NotNull(group);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(1, group.Id);
        }
        
        [Fact]
        public async Task GetFavoriteGroupsForUser_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var groups = new List<Membership>()
            {
                new Membership()
                {
                    UserId = 1,
                }
            };

            var groupsToReturn = new List<GroupToReturnDto>()
            {
                new GroupToReturnDto()
                {
                    Id = 1,
                    Favorite = true
                }
            };

            _unitOfWork.Setup(x => x.Groups.GetFavoriteGroupsForUser(It.IsAny<int>()))
                .ReturnsAsync(groups);

            _mapperMock.Setup(x => x.Map<ICollection<GroupToReturnDto>>(groups))
                .Returns(groupsToReturn);
            
            // Act
            var result = await _sut.GetFavoriteGroupsForUser(It.IsAny<int>());

            // Assert
            var group = result.FirstOrDefault();
            Assert.NotNull(group);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(group.Favorite);
            Assert.Equal(1, group.Id);
        }

        [Fact]
        public async Task GetGroup_ShouldReturnNull_WhenGroupDosntExist()
        {
            // Arrange
            _unitOfWork.Setup(x => x.Groups.GetGroupWithInclude(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);
            
            // Act
            var result = await _sut.GetGroup(It.IsAny<int>(), It.IsAny<int>());
            
            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task GetGroup_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var groupToReturn = new DetailGroupToReturnDto()
            {
                Id = 1,
                Description = "test",
                Image = null,
                Location = new Location(),
                Members = new List<UserForDisplayDto>(),
                Name = "Test",
                DateCreated = DateTime.Now,
                LatestJoined = new List<UserForDisplayDto>(),
                PendingRequests = new List<UserForDisplayDto>(),
                NumberOfMembers = 10
            };

            var membership = new Membership()
            {
                UserId = 1,
            };
            
            _unitOfWork.Setup(x => x.Groups.GetGroupWithInclude(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(membership);

            _mapperMock.Setup(x => x.Map<DetailGroupToReturnDto>(membership))
                .Returns(groupToReturn);
            
            // Act
            var result = await _sut.GetGroup(It.IsAny<int>(), It.IsAny<int>());
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<DetailGroupToReturnDto>(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetGroupsForUser_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var memberships = new List<Membership>()
            {
                new Membership()
                {
                    UserId = 1,
                }
            };

            var groupsToReturn = new List<GroupToReturnDto>()
            {
                new GroupToReturnDto()
                {
                    Id = 1,
                }
            };
            
            _unitOfWork.Setup(x => x.Groups.GetGroupsForUser(It.IsAny<int>()))
                .ReturnsAsync(memberships);

            _mapperMock.Setup(x => x.Map<ICollection<GroupToReturnDto>>(memberships))
                .Returns(groupsToReturn);

            // Act
            var result = await _sut.GetGroupsForUser(It.IsAny<int>());

            // Assert
            var group = result.FirstOrDefault();
            Assert.NotNull(result);
            Assert.NotNull(group);
            Assert.NotEmpty(result);
            Assert.Equal(1, group.Id);
        }

        [Fact]
        public async Task GetMembershipInformation_ShouldReturnNull_WhenGroupDoesntExist()
        {
            // Arrange
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetMembershipInformation(It.IsAny<int>(), It.IsAny<int>());
            
            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task GetMembershipInformation_ShouldReturnNull_WhenUserDoesntExist()
        {
            // Arrange
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Group());

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetMembershipInformation(It.IsAny<int>(), It.IsAny<int>());
            
            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public async Task GetMembershipInformation_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var membership = new Membership()
            {
                UserId = 1,
            };

            var membershipToReturnDto = new MembershipInformationDto()
            {
                Favorite = true,
                Role = Role.Owner,
                MembershipStatus = MembershipStatus.Accepted
            };
            
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Group());

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PowerUser());

            _unitOfWork.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(membership);

            _mapperMock.Setup(x => x.Map<MembershipInformationDto>(membership))
                .Returns(membershipToReturnDto);

            // Act
            var result = await _sut.GetMembershipInformation(It.IsAny<int>(), It.IsAny<int>());
            
            // Assert
            Assert.NotNull(result);
            Assert.True(result.Favorite);
            Assert.Equal(Role.Owner, result.Role);
        }
        
        [Fact]
        public async Task LeaveGroup_ShouldReturnFalse_WhenUserIsNotMemberOfGroup()
        {
            // Arrange
            var expectedMessage = "Specified membership doesn't exist!";
            _unitOfWork.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.LeaveGroup(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task LeaveGroup_ShouldReturnFalse_WhenIsNotCompleted()
        {
            // Arrange
            var expectedMessage = "Error leaving group!";
            _unitOfWork.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Membership());
            
            _unitOfWork.Setup(x => x.Memberships.Remove(new Membership()))
                .Verifiable();

            _unitOfWork.Setup(x => x.Complete())
                .ReturnsAsync(false);

            // Act
            var result = await _sut.LeaveGroup(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task LeaveGroup_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var expectedMessage = "Successfully left group!";
            _unitOfWork.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Membership());
            
            _unitOfWork.Setup(x => x.Memberships.Remove(new Membership()))
                .Verifiable();

            _unitOfWork.Setup(x => x.Complete())
                .ReturnsAsync(true);

            // Act
            var result = await _sut.LeaveGroup(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.True(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task MakeFavoriteGroup_ShouldReturnFalse_WhenGroupDoesntExist()
        {
            // Arrange
            var expectedMessage = "Specified group doesn't exist!";
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.MakeFavoriteGroup(It.IsAny<int>(), It.IsAny<int>(), true);

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task MakeFavoriteGroup_ShouldReturnFalse_WhenUserDoesntExist()
        {
            // Arrange
            var expectedMessage = "Specified user doesn't exist!";
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Group());

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.MakeFavoriteGroup(It.IsAny<int>(), It.IsAny<int>(), true);

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task MakeFavoriteGroup_ShouldReturnFalse_WhenMembershipDoesntExist()
        {
            // Arrange
            var expectedMessage = "Specified membership doesn't exist!";
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Group());

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PowerUser());

            _unitOfWork.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);
            
            // Act
            var result = await _sut.MakeFavoriteGroup(It.IsAny<int>(), It.IsAny<int>(), true);

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task MakeFavoriteGroup_ShouldReturnFalse_WhenIsNotCompleted()
        {
            // Arrange
            var expectedMessage = "Error changing status!";
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Group());

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PowerUser());

            _unitOfWork.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Membership());

            _unitOfWork.Setup(x => x.Complete())
                .ReturnsAsync(false);
            
            // Act
            var result = await _sut.MakeFavoriteGroup(It.IsAny<int>(), It.IsAny<int>(), true);

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task MakeFavoriteGroup_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var expectedMessage = "Successfully changed status!";
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Group());

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PowerUser());

            _unitOfWork.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Membership());

            _unitOfWork.Setup(x => x.Complete())
                .ReturnsAsync(true);
            
            // Act
            var result = await _sut.MakeFavoriteGroup(It.IsAny<int>(), It.IsAny<int>(), true);

            // Assert
            Assert.True(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task RejectUser_ShouldReturnFalse_WhenGroupDoesntExist()
        {
            // Arrange
            var expectedMessage = "Specified group doesn't exist!";
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.RejectUser(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task RejectUser_ShouldReturnFalse_WhenUserDoesntExist()
        {
            // Arrange
            var expectedMessage = "Specified user doesn't exist!";
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Group());

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.RejectUser(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task RejectUser_ShouldReturnFalse_WhenMembershipDoesntExist()
        {
            // Arrange
            var expectedMessage = "You have to ask to join to group!";
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Group());

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PowerUser());

            _unitOfWork.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);
            
            // Act
            var result = await _sut.RejectUser(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task RejectUser_ShouldReturnFalse_WhenIsNotCompleted()
        {
            // Arrange
            var expectedMessage = "User couldn't be rejected!";
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Group());

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PowerUser());

            _unitOfWork.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Membership());

            _unitOfWork.Setup(x => x.Complete())
                .ReturnsAsync(false);
            
            // Act
            var result = await _sut.RejectUser(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task RejectUser_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var expectedMessage = "User successfully rejected!";
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Group());

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PowerUser());

            _unitOfWork.Setup(x => x.Memberships.GetMembershipById(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Membership());

            _unitOfWork.Setup(x => x.Complete())
                .ReturnsAsync(true);
            
            // Act
            var result = await _sut.RejectUser(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.True(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task RequestJoinGroup_ShouldReturnFalse_WhenGroupDoesntExist()
        {
            // Arrange
            var expectedMessage = "Specified group doesn't exist!";
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.RequestJoinGroup(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task RequestJoinGroup_ShouldReturnFalse_WhenUserDoesntExist()
        {
            // Arrange
            var expectedMessage = "Specified user doesn't exist!";
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Group());

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.RequestJoinGroup(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }

        [Fact]
        public async Task RequestJoinGroup_ShouldReturnFalse_WhenIsNotCompleted()
        {
            // Arrange
            var expectedMessage = "Request not sent!";
            var membership = new Membership()
            {
                Favorite = false,
                Group = null,
                GroupId = 1,
                User = new CommonUser(),
                UserId = 1,
                MembershipStatus = MembershipStatus.Sent,
            };
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Group());

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PowerUser());
            
            _unitOfWork.Setup(x => x.Memberships.Add(membership)).Verifiable();

            _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(false);

            // Act
            var result = await _sut.RequestJoinGroup(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.False(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
        
        [Fact]
        public async Task RequestJoinGroup_ShouldBeDoneSuccessfully()
        {
            // Arrange
            var expectedMessage = "Request successfully sent!";
            var membership = new Membership()
            {
                Favorite = false,
                Group = null,
                GroupId = 1,
                User = new CommonUser(),
                UserId = 1,
                MembershipStatus = MembershipStatus.Sent,
            };
            _unitOfWork.Setup(x => x.Groups.GetById(It.IsAny<int>()))
                .ReturnsAsync(new Group());

            _unitOfWork.Setup(x => x.Users.GetById(It.IsAny<int>()))
                .ReturnsAsync(new PowerUser());
            
            _unitOfWork.Setup(x => x.Memberships.Add(membership)).Verifiable();

            _unitOfWork.Setup(x => x.Complete()).ReturnsAsync(true);

            // Act
            var result = await _sut.RequestJoinGroup(It.IsAny<int>(), It.IsAny<int>());

            // Assert
            Assert.True(result.Key);
            Assert.Equal(expectedMessage, result.Value);
        }
    }
}