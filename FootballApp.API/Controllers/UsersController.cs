using System.Security.Claims;
using System.Threading.Tasks;
using FootballApp.API.Dtos;
using FootballApp.API.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateUser(int userId, UserToUpdateDto userToUpdateDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _usersService.UpdateUser(userId, userToUpdateDto));
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _usersService.GetUser(id);

            if (user != null)
                return Ok(user);

            return BadRequest("Specified user does not exist");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _usersService.GetAllUsers());
        }

        [HttpPost]
        [Route("visit")]
        public async Task<IActionResult> VisitUser(VisitUserDto visitUserDto)
        {
            if (visitUserDto.VisitorId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _usersService.VisitUser(visitUserDto));
        }

        [HttpGet]
        [Route("visitors")]
        public async Task<IActionResult> GetLatestFiveVisitorsForUser(int userId)
        {
            return Ok(await _usersService.GetLatestFiveVisitorsForUser(userId));
        }

        [HttpGet]
        [Route("achievements/all")]
        public async Task<IActionResult> GetAllAchievements()
        {
            return Ok(await _usersService.GetAllAchievements());
        }

        [HttpGet]
        [Route("achievements")]
        public async Task<IActionResult> GetAllAchievementsForUser(int userId)
        {
            return Ok(await _usersService.GetAllAchievementsForUser(userId));
        }

        [HttpPost]
        [Route("achievements/new")]
        public async Task<IActionResult> GainAchievement(int userId, GainedAchievementForCreationDto gainedAchievementForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _usersService.GainAchievement(userId, gainedAchievementForCreationDto));
        }
    }
}