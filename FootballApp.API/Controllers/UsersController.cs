using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateUser(int userId, UserToUpdateDto userToUpdateDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _unitOfWork.Users.GetById(userId);

            if (user == null)
            {
                return BadRequest("Specified user doesn't exist.");
            }
            var country = await _unitOfWork.Countries.GetCountryWithCities(userToUpdateDto.Country);
            var city = await _unitOfWork.Cities.GetCityById(userToUpdateDto.City, userToUpdateDto.Country);
            if (city == null || country == null || !country.Cities.Contains(city))
            {
                return BadRequest("Specified country or city doesn't exist!");
            }

            user.City = city;
            user.Country = country;
            user.Email = userToUpdateDto.Email;
            if (await _unitOfWork.Complete())
            {
                return Ok(new KeyValuePair<bool, string>(true, "User successfully updated."));
            }

            return Ok(new KeyValuePair<bool, string>(false, "User can't be updated."));
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
                return Ok(new KeyValuePair<bool, string>(true, "User successfully visited!"));

            return Ok(new KeyValuePair<bool, string>(false, "Problem with visiting user!"));
        }

        [HttpGet]
        [Route("visitors")]
        public async Task<IActionResult> GetLatestFiveVisitorsForUser(int userId)
        {
            var visitors = await _unitOfWork.Users.GetLatestFiveVisitorsForUser(userId);

            var visitorsToReturn = _mapper.Map<ICollection<VisitToReturnDto>>(visitors);

            return Ok(visitorsToReturn);
        }

        [HttpGet]
        [Route("achievements/all")]
        public async Task<IActionResult> GetAllAchievements()
        {
            return Ok(await _unitOfWork.Achievements.GetAll());
        }

        [HttpGet]
        [Route("achievements")]
        public async Task<IActionResult> GetAllAchievementsForUser(int userId)
        {
            var achievements = await _unitOfWork.Users
                                          .GetAllAchievementsForUser(userId);

            return Ok(_mapper.Map<ICollection<GainedAchievementToReturnDto>>(achievements));
        }

        [HttpPost]
        [Route("achievements/new")]
        public async Task<IActionResult> GainAchievement(int userId, GainedAchievementForCreationDto gainedAchievementForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var achievement = await _unitOfWork.Achievements.GetAchievementByValue(gainedAchievementForCreationDto.Value);

            if (achievement == null)
            {
                return BadRequest("Not valid achievement!");
            }
            var gainedAchievement = await _unitOfWork.Achievements.GetGainedAchievement(userId, gainedAchievementForCreationDto.Value);
            if (gainedAchievement != null)
            {
                return BadRequest("Already have gained that achievement!");
            }

            gainedAchievement = new GainedAchievement
            {
                UserId = userId,
                User = await _unitOfWork.Users.GetById(userId),
                DateAchieved = DateTime.Now,
                Achievement = achievement,
                AchievementId = achievement.Id
            };

            _unitOfWork.Users.GainAchievement(gainedAchievement);
            if (await _unitOfWork.Complete())
                return Ok(new KeyValuePair<bool, string>(true, "Achievement successfully gained!"));

            return Ok(new KeyValuePair<bool, string>(false, "Problem gaining achievement!"));
        }

    }
}