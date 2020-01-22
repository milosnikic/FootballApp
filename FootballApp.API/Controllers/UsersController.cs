using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUsersRepository _usersRepository;
        private readonly IRepository _repo;

        public UsersController(IUsersRepository usersRepository, IRepository repo, IMapper mapper)
        {
            _repo = repo;
            _usersRepository = usersRepository;
            _mapper = mapper;

        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userFromRepo = await _usersRepository.GetUser(id);

            if (userFromRepo == null)
                return BadRequest("Specified user does not exist");

            var userToReturn = _mapper.Map<UserToReturnDto>(userFromRepo);

            return Ok(userToReturn);
        }


        [HttpPost("{id}/createGroup")]
        public async Task<IActionResult> CreateGroup(int id, GroupForCreationDto groupForCreationDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _usersRepository.GetUser(id);


            var group = _mapper.Map<Group>(groupForCreationDto);

            _repo.Add(group);

            var membership = new Membership { UserId = user.Id, GroupId = group.Id, DateSent = DateTime.Now, Role = Role.Owner, Accepted = true, DateAccepted = DateTime.Now };

            _repo.Add(membership);

            var userToReturn = _mapper.Map<UserToReturnDto>(user);
            if (await _repo.SaveAll())
                return Ok(userToReturn);

            return BadRequest("Could not create group");
        }

        
    }
}