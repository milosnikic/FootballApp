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

            var groupFromRepo = await _unitOfWork.Groups.GetById(id);

            if (groupFromRepo == null)
                return BadRequest(new
                {
                    message = "Specified group does not exist"
                });


            var groupToReturn = _mapper.Map<GroupToReturnDto>(groupFromRepo);

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

        [HttpPost]
        public async Task<IActionResult> CreateGroup(int userId,[FromForm] GroupForCreationDto group)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            // Groups and User relationship is based on Membership so
            // when we add group we need to add new Membership with desired user
            var user = await _unitOfWork.Users.GetById(userId);
            var groupToAdd = _mapper.Map<Group>(group);

            if (group.Image != null && ImageValidator.ValidateImageExtension(group.Image)
                && ImageValidator.ValidateImageSize(group.Image)
                && ImageValidator.ValidateImageSignature(group.Image)) 
                {
                    using(var memoryStream = new MemoryStream())
                    {
                        await group.Image.CopyToAsync(memoryStream);
                        groupToAdd.Image = memoryStream.ToArray();
                    }
                }

            // We check if selected location exists
            // If it doesn't we create a new one with specified country and city
            var location = await _unitOfWork.Locations.GetByName(groupToAdd.Name);
            if(location == null)
            {
                location = new Location { Name = group.Location, CityId = group.CityId, CountryId = group.CountryId };
                _unitOfWork.Locations.Add(location);
            }

            groupToAdd.Location = location;
            _unitOfWork.Groups.Add(groupToAdd);

            if(!await _unitOfWork.Complete())
            {
                return BadRequest(new KeyValuePair<bool,string>(false, "Couldn't create group"));
            }
                        
            var membership = new Membership { UserId = user.Id, GroupId = groupToAdd.Id, DateSent = DateTime.Now, Role = Role.Owner, Accepted = true, DateAccepted = DateTime.Now, User = user, Group = groupToAdd };
            _unitOfWork.Memberships.Add(membership);

            if (await _unitOfWork.Complete())
            {
                var groupToReturn = _mapper.Map<GroupToReturnDto>(groupToAdd);
                return Ok(new KeyValuePair<bool, string>(true, "Group created successfully!"));
            }
            
            return BadRequest(new KeyValuePair<bool,string>(false, "Something went wrong!"));
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _unitOfWork.Groups.GetAllGroupsWithInclude();
            return Ok(_mapper.Map<ICollection<GroupToReturnDto>>(groups));
        }   
    }
}
