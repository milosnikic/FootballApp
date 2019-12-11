using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data;
using FootballApp.API.Data.Groups;
using FootballApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users/{userId}/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IMapper _mapper;
        private readonly IGroupsRepository _groupsRepository;
        public GroupsController(IRepository repo, IMapper mapper, IGroupsRepository groupsRepository)
        {
            _groupsRepository = groupsRepository;
            _mapper = mapper;
            _repo = repo;

        }

        [HttpGet("{id}", Name = "GetGroup")]
        public async Task<IActionResult> GetGroup(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var groupFromRepo = await _groupsRepository.GetGroup(id);

            if (groupFromRepo == null)
                return BadRequest(new
                {
                    message = "Specified group does not exist"
                });


            var groupToReturn = _mapper.Map<GroupToReturnDto>(groupFromRepo);

            return Ok(groupToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsCreatedByUser(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var groups = await _groupsRepository.GetGroupsForUser(userId);

            var groupsToReturn = _mapper.Map<ICollection<GroupToReturnDto>>(groups);

            return Ok(groupsToReturn);
        }
    }
}