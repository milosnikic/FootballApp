using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data;
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
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userFromRepo = await _unitOfWork.Users.GetUserByIdWithAdditionalInformation(id);

            if (userFromRepo == null)
                return BadRequest("Specified user does not exist");

            var userToReturn = _mapper.Map<UserToReturnDto>(userFromRepo);

            return Ok(userToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAll();

            var usersToReturn = _mapper.Map<ICollection<UserToReturnDto>>(users);
            return Ok(usersToReturn);
        }


        [HttpPost]
        [Route("visit")]
        public async Task<IActionResult> VisitUser(VisitUserDto visitUserDto)
        {
            if (visitUserDto.VisitorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            // Map visit user dto to visit object
            var visit = _mapper.Map<Visit>(visitUserDto);
            _unitOfWork.Users.VisitUser(visit);
            if (await _unitOfWork.Complete())
                return Ok(new KeyValuePair<bool,string>(true, "User successfully visited!"));

            return Ok(new KeyValuePair<bool,string>(false, "Problem with visiting user!"));
        }

        [HttpGet]
        [Route("visitors")]
        public async Task<IActionResult> GetLatestFiveVisitorsForUser(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))            
                return Unauthorized();
            
            // TODO: Add mappings for visitors
            var visitors = await _unitOfWork.Users.GetLatestFiveVisitorsForUser(userId);

            return Ok(visitors);
        }

        // [HttpPost("{id}/createGroup")]
        // public async Task<IActionResult> CreateGroup(int id, GroupForCreationDto groupForCreationDto)
        // {
        //     if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        //         return Unauthorized();

        //     var user = await _unitOfWork.GetById(id);

        //     var group = _mapper.Map<Group>(groupForCreationDto);

        //     _repo.Add(group);

        //     var membership = new Membership { UserId = user.Id, GroupId = group.Id, DateSent = DateTime.Now, Role = Role.Owner, Accepted = true, DateAccepted = DateTime.Now };

        //     _repo.Add(membership);

        //     var userToReturn = _mapper.Map<UserToReturnDto>(user);
        //     if (await _repo.SaveAll())
        //         return Ok(userToReturn);

        //     return BadRequest("Could not create group");
        // }

        
    }
}