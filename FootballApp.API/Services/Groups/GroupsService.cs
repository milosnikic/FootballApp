using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Helpers;
using FootballApp.API.Models;

namespace FootballApp.API.Services.Groups
{
    public class GroupsService : IGroupsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GroupsService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<KeyValuePair<bool, string>> AcceptUser(int groupId, int userId)
        {
            var group = await _unitOfWork.Groups.GetById(groupId);
            if (group == null)
            {
                return new KeyValuePair<bool, string>(false, "Specified group doesn't exist!");
            }

            var user = await _unitOfWork.Users.GetById(userId);
            if (user == null)
            {
                return new KeyValuePair<bool, string>(false, "Specified user doesn't exist!");
            }

            var membership = await _unitOfWork.Memberships.GetMembershipById(userId, groupId);
            if (membership == null)
            {
                return new KeyValuePair<bool, string>(false, "You have to ask to join to group!");
            }

            membership.DateAccepted = DateTime.Now;
            membership.Accepted = true;
            membership.MembershipStatus = MembershipStatus.Accepted;

            if (await _unitOfWork.Complete())
            {
                return new KeyValuePair<bool, string>(true, "User joined group!");
            }

            return new KeyValuePair<bool, string>(false, "User hasn't joined group!");
        }

        public async Task<KeyValuePair<bool, string>> CreateGroup(int userId, GroupForCreationDto group)
        {
            // Groups and User relationship is based on Membership so
            // when we add group we need to add new Membership with desired user
            var user = await _unitOfWork.Users.GetById(userId);
            if (user == null)
            {
                return new KeyValuePair<bool, string>(false, "User doesn't exist!");
            }

            if (!(user is PowerUser))
            {
                return new KeyValuePair<bool, string>(false, "Unauthorized");
            }

            PowerUser powerUser = (PowerUser)user;
            var groupToAdd = _mapper.Map<Group>(group);

            if (group.Image != null)
            {
                if (ImageValidator.ImageExtensionValidation(group.Image)
                && ImageValidator.ImageSizeValidation(group.Image)
                && ImageValidator.ImageSignatureValidation(group.Image))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await group.Image.CopyToAsync(memoryStream);
                        groupToAdd.Image = memoryStream.ToArray();
                    }
                }
                else
                {
                    return new KeyValuePair<bool, string>(false, "Image cannot be uploaded");
                }
            }

            // We check if selected location exists
            // If it doesn't we create a new one with specified country and city
            var location = await _unitOfWork.Locations.GetByName(groupToAdd.Name);
            if (location == null)
            {
                location = new Location { Name = group.Location, CityId = group.CityId, CountryId = group.CountryId };
                _unitOfWork.Locations.Add(location);
            }

            groupToAdd.CreatedBy = powerUser;
            groupToAdd.Location = location;

            powerUser.NumberOfGroupsCreated += 1;
            _unitOfWork.Groups.Add(groupToAdd);

            if (!await _unitOfWork.Complete())
            {
                return new KeyValuePair<bool, string>(false, "Couldn't create group");
            }

            var membership = new Membership { UserId = powerUser.Id, GroupId = groupToAdd.Id, DateSent = DateTime.Now, Role = Role.Owner, Accepted = true, DateAccepted = DateTime.Now, User = powerUser, Group = groupToAdd, MembershipStatus = MembershipStatus.Accepted };
            _unitOfWork.Memberships.Add(membership);

            if (await _unitOfWork.Complete())
            {
                return new KeyValuePair<bool, string>(true, "Group created successfully!");
            }

            return new KeyValuePair<bool, string>(false, "Something went wrong!");
        }

        public async Task<IEnumerable<GroupToReturnDto>> GetAllCreatedGroups(int userId)
        {
            var user = await _unitOfWork.Users.GetUserByIdWithAdditionalInformation(userId);
            if (!(user is PowerUser))
            {
                return null;
            }

            PowerUser powerUser = (PowerUser)user;
            var createdGroups = await _unitOfWork.Groups.GetCreatedGroupsForUser(powerUser);
            return _mapper.Map<ICollection<GroupToReturnDto>>(createdGroups);
        }

        public async Task<IEnumerable<GroupToReturnDto>> GetAllGroups(int userId)
        {
            var groups = await _unitOfWork.Groups.GetAllGroupsWithInclude(userId);
            return _mapper.Map<ICollection<GroupToReturnDto>>(groups);
        }

        public async Task<IEnumerable<GroupToReturnDto>> GetFavoriteGroupsForUser(int userId)
        {
            var groups = await _unitOfWork.Groups.GetFavoriteGroupsForUser(userId);

            return _mapper.Map<ICollection<GroupToReturnDto>>(groups);
        }

        public async Task<DetailGroupToReturnDto> GetGroup(int id, int userId)
        {
            var groupFromRepo = await _unitOfWork.Groups.GetGroupWithInclude(id, userId);

            if (groupFromRepo == null)
                return null;

            return _mapper.Map<DetailGroupToReturnDto>(groupFromRepo);
        }

        public async Task<IEnumerable<GroupToReturnDto>> GetGroupsForUser(int userId)
        {
            var groups = await _unitOfWork.Groups.GetGroupsForUser(userId);

            return _mapper.Map<ICollection<GroupToReturnDto>>(groups);
        }

        public async Task<MembershipInformationDto> GetMembershipInformation(int userId, int groupId)
        {
            var group = await _unitOfWork.Groups.GetById(groupId);
            if (group == null)
            {
                return null;
            }

            var user = await _unitOfWork.Users.GetById(userId);
            if (user == null)
            {
                return null;
            }

            return _mapper.Map<MembershipInformationDto>(await _unitOfWork.Memberships.GetMembershipById(userId, groupId));
        }

        public async Task<KeyValuePair<bool, string>> LeaveGroup(int groupId, int userId)
        {
            var membership = await _unitOfWork.Memberships.GetMembershipById(userId, groupId);
            if (membership == null)
            {
                return new KeyValuePair<bool, string>(false, "Specified membership doesn't exist!");
            }

            _unitOfWork.Memberships.Remove(membership);
            if (await _unitOfWork.Complete())
            {
                return new KeyValuePair<bool, string>(true, "Successfully left group!");
            }

            return new KeyValuePair<bool, string>(false, "Error leaving group!");
        }

        public async Task<KeyValuePair<bool, string>> MakeFavoriteGroup(int groupId, int userId, bool favorite)
        {
            var group = await _unitOfWork.Groups.GetById(groupId);
            if (group == null)
            {
                return new KeyValuePair<bool, string>(false, "Specified group doesn't exist!");
            }

            var user = await _unitOfWork.Users.GetById(userId);
            if (user == null)
            {
                return new KeyValuePair<bool, string>(false, "Specified user doesn't exist!");
            }

            var membership = await _unitOfWork.Memberships.GetMembershipById(userId, groupId);
            if (membership == null)
            {
                return new KeyValuePair<bool, string>(false, "Specified membership doesn't exist!");
            }

            membership.Favorite = favorite;
            if (await _unitOfWork.Complete())
            {
                return new KeyValuePair<bool, string>(true, "Successfully changed status!");
            }

            return new KeyValuePair<bool, string>(false, "Error changing status!");
        }

        public async Task<KeyValuePair<bool, string>> RejectUser(int groupId, int userId)
        {
            var group = await _unitOfWork.Groups.GetById(groupId);
            if (group == null)
            {
                return new KeyValuePair<bool, string>(false, "Specified group doesn't exist!");
            }

            var user = await _unitOfWork.Users.GetById(userId);
            if (user == null)
            {
                return new KeyValuePair<bool, string>(false, "Specified user doesn't exist!");
            }

            var membership = await _unitOfWork.Memberships.GetMembershipById(userId, groupId);
            if (membership == null)
            {
                return new KeyValuePair<bool, string>(false, "You have to ask to join to group!");
            }

            _unitOfWork.Memberships.Remove(membership);
            if (await _unitOfWork.Complete())
            {
                return new KeyValuePair<bool, string>(true, "User successfully rejected!");
            }

            return new KeyValuePair<bool, string>(false, "User couldn't be rejected!");
        }

        public async Task<KeyValuePair<bool, string>> RequestJoinGroup(int groupId, int userId)
        {
            var group = await _unitOfWork.Groups.GetById(groupId);
            if (group == null)
            {
                return new KeyValuePair<bool, string>(false, "Specified group doesn't exist!");
            }

            var user = await _unitOfWork.Users.GetById(userId);
            if (user == null)
            {
                return new KeyValuePair<bool, string>(false, "Specified user doesn't exist!");
            }

            var membership = new Membership
            {
                Favorite = false,
                Group = group,
                GroupId = group.Id,
                MembershipStatus = MembershipStatus.Sent,
                User = user,
                UserId = user.Id,
            };

            _unitOfWork.Memberships.Add(membership);

            if (await _unitOfWork.Complete())
            {
                return new KeyValuePair<bool, string>(true, "Request successfully sent!");
            }

            return new KeyValuePair<bool, string>(false, "Request not sent!");
        }
    }
}