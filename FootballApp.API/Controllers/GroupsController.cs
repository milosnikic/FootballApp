using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data;
using FootballApp.API.Data.Groups;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Helpers;
using FootballApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        public GroupsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet("{id}", Name = "GetGroup")]
        public async Task<IActionResult> GetGroup(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var groupFromRepo = await _unitOfWork.Groups.GetGroupWithInclude(id, userId);

            if (groupFromRepo == null)
                return BadRequest(new
                {
                    message = "Specified group does not exist"
                });


            var groupToReturn = _mapper.Map<DetailGroupToReturnDto>(groupFromRepo);

            return Ok(groupToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsForUser(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var groups = await _unitOfWork.Groups.GetGroupsForUser(userId);

            var groupsToReturn = _mapper.Map<ICollection<GroupToReturnDto>>(groups);

            return Ok(groupsToReturn);
        }

        [HttpGet]
        [Route("favorite")]
        public async Task<IActionResult> GetFavoriteGroupsForUser(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var groups = await _unitOfWork.Groups.GetFavoriteGroupsForUser(userId);

            var groupsToReturn = _mapper.Map<ICollection<GroupToReturnDto>>(groups);

            return Ok(groupsToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(int userId, [FromForm] GroupForCreationDto group)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // Groups and User relationship is based on Membership so
            // when we add group we need to add new Membership with desired user
            var user = await _unitOfWork.Users.GetById(userId);
            if (user == null)
            {
                return BadRequest("User doesn't exist!");
            }

            if (!(user is PowerUser))
            {
                return Unauthorized();
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
                    return Ok(new KeyValuePair<bool, string>(false, "Image cannot be uploaded"));
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
                return BadRequest(new KeyValuePair<bool, string>(false, "Couldn't create group"));
            }

            var membership = new Membership { UserId = powerUser.Id, GroupId = groupToAdd.Id, DateSent = DateTime.Now, Role = Role.Owner, Accepted = true, DateAccepted = DateTime.Now, User = powerUser, Group = groupToAdd, MembershipStatus = MembershipStatus.Accepted };
            _unitOfWork.Memberships.Add(membership);

            if (await _unitOfWork.Complete())
            {
                return Ok(new KeyValuePair<bool, string>(true, "Group created successfully!"));
            }

            return BadRequest(new KeyValuePair<bool, string>(false, "Something went wrong!"));
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllGroups(int userId)
        {
            var groups = await _unitOfWork.Groups.GetAllGroupsWithInclude(userId);
            return Ok(_mapper.Map<ICollection<GroupToReturnDto>>(groups));
        }

        [HttpGet]
        [Route("created")]
        public async Task<IActionResult> GetAllCreatedGroups(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _unitOfWork.Users.GetUserByIdWithAdditionalInformation(userId);
            if (!(user is PowerUser))
            {
                return BadRequest("You are not power user!");
            }

            PowerUser powerUser = (PowerUser)user;
            var createdGroups = await _unitOfWork.Groups.GetCreatedGroupsForUser(powerUser);
            return Ok(_mapper.Map<ICollection<GroupToReturnDto>>(createdGroups));
        }

        [HttpPost]
        [Route("leave/{groupId}")]
        public async Task<IActionResult> LeaveGroup(int groupId, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var membership = await _unitOfWork.Memberships.GetMembershipById(userId, groupId);
            if (membership == null)
            {
                return BadRequest("Specified membership doesn't exist!");
            }

            _unitOfWork.Memberships.Remove(membership);
            if (await _unitOfWork.Complete())
            {
                return Ok(new KeyValuePair<bool, string>(true, "Successfully left group!"));
            }

            return Ok(new KeyValuePair<bool, string>(false, "Error leaving group!"));
        }

        [HttpPost]
        [Route("request-join/{groupId}")]
        public async Task<IActionResult> RequestJoinGroup(int groupId, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var group = await _unitOfWork.Groups.GetById(groupId);
            if (group == null)
            {
                return BadRequest("Specified group doesn't exist!");
            }

            var user = await _unitOfWork.Users.GetById(userId);
            if (user == null)
            {
                return BadRequest("Specified user doesn't exist!");
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
                return Ok(new KeyValuePair<bool, string>(true, "Request successfully sent!"));
            }

            return Ok(new KeyValuePair<bool, string>(false, "Request not sent!"));
        }

        [HttpPost]
        [Route("accept/{groupId}")]
        public async Task<IActionResult> AcceptUser(int groupId, int userId)
        {
            // We only check that we are logged in
            // TODO: add check if user is admin or owner in order to 
            //       accept or reject user
            
            var group = await _unitOfWork.Groups.GetById(groupId);
            if (group == null)
            {
                return BadRequest("Specified group doesn't exist!");
            }

            var user = await _unitOfWork.Users.GetById(userId);
            if (user == null)
            {
                return BadRequest("Specified user doesn't exist!");
            }

            var membership = await _unitOfWork.Memberships.GetMembershipById(userId, groupId);
            if (membership == null)
            {
                return BadRequest("You have to ask to join to group!");
            }

            membership.DateAccepted = DateTime.Now;
            membership.Accepted = true;
            membership.MembershipStatus = MembershipStatus.Accepted;

            if (await _unitOfWork.Complete())
            {
                return Ok(new KeyValuePair<bool, string>(true, "User joined group!"));
            }

            return Ok(new KeyValuePair<bool, string>(false, "User hasn't joined group!"));
        }

        [HttpDelete]
        [Route("reject/{groupId}")]
        public async Task<IActionResult> RejectUser(int groupId, int userId)
        {   
            // We only check that we are logged in
            // TODO: add check if user is admin or owner in order to 
            //       accept or reject user

            var group = await _unitOfWork.Groups.GetById(groupId);
            if (group == null)
            {
                return BadRequest("Specified group doesn't exist!");
            }

            var user = await _unitOfWork.Users.GetById(userId);
            if (user == null)
            {
                return BadRequest("Specified user doesn't exist!");
            }

            var membership = await _unitOfWork.Memberships.GetMembershipById(userId, groupId);
            if (membership == null)
            {
                return BadRequest("You have to ask to join to group!");
            }

            _unitOfWork.Memberships.Remove(membership);
            if (await _unitOfWork.Complete())
            {
                return Ok(new KeyValuePair<bool, string>(true, "User successfully rejected!"));
            }

            return Ok(new KeyValuePair<bool, string>(false, "User couldn't be rejected!"));
        }

        /// <summary>
        /// Method is used to make group favorite or unfavorite 🙂
        /// </summary>
        /// <param name="groupId">Id of group to get favorite or unfavorite</param>
        /// <param name="userId">User id</param>
        /// <param name="like">Indicator if group should be favorite or unfavorite</param>
        /// <returns>Result of favoriting operation</returns>
        [HttpPost]
        [Route("favorite/{groupId}")]
        public async Task<IActionResult> MakeFavoriteGroup(int groupId, int userId, bool favorite = true)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var group = await _unitOfWork.Groups.GetById(groupId);
            if (group == null)
            {
                return BadRequest("Specified group doesn't exist!");
            }

            var user = await _unitOfWork.Users.GetById(userId);
            if (user == null)
            {
                return BadRequest("Specified user doesn't exist!");
            }

            var membership = await _unitOfWork.Memberships.GetMembershipById(userId, groupId);
            if (membership == null)
            {
                return BadRequest("Specified membership doesn't exist!");
            }

            membership.Favorite = favorite;
            if (await _unitOfWork.Complete())
            {
                return Ok(new KeyValuePair<bool, string>(true, "Successfully changed status!"));
            }

            return Ok(new KeyValuePair<bool, string>(false, "Error changing status!"));
        }


        [HttpGet]
        [Route("{groupId}/membership-info")]
        public async Task<IActionResult> GetMembershipInformation(int userId, int groupId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var group = await _unitOfWork.Groups.GetById(groupId);
            if (group == null)
            {
                return BadRequest("Specified group doesn't exist!");
            }

            var user = await _unitOfWork.Users.GetById(userId);
            if (user == null)
            {
                return BadRequest("Specified user doesn't exist!");
            }

            var membership = _mapper.Map<MembershipInformationDto>(await _unitOfWork.Memberships.GetMembershipById(userId, groupId));
            
            return Ok(membership);
        }
    }
}
