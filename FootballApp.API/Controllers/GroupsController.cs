using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data;
using FootballApp.API.Data.Groups;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
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
        
    }
}
