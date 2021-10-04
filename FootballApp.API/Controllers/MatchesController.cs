using FootballApp.API.Dtos;
using FootballApp.API.Services.Matches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FootballApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchesService _matchesService;

        public MatchesController(IMatchesService matchesService)
        {
            _matchesService = matchesService;
        }

        [HttpGet]
        [Route("{matchId}")]
        public async Task<IActionResult> GetUpcomingMatchday(int matchId)
        {
            return Ok(await _matchesService.GetUpcomingMatchday(matchId));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateMatch(MatchdayForCreationDto matchdayForCreation, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _matchesService.CreateMatch(matchdayForCreation, userId));
        }

        [HttpGet]
        [Route("upcoming-matches/{groupId}")]
        public async Task<IActionResult> GetUpcomingMatchesForGroup(int groupId)
        {
            return Ok(await _matchesService.GetUpcomingMatchesForGroup(groupId));
        }

        [HttpGet]
        [Route("upcoming-matches")]
        public async Task<IActionResult> GetUpcomingMatchesForUser(int userId)
        {
            return Ok(await _matchesService.GetUpcomingMatchesForUser(userId));
        }

        [HttpGet]
        [Route("upcoming-matches-applicable")]
        public async Task<IActionResult> GetUpcomingMatchesApplicableForUser(int userId)
        {
            return Ok(await _matchesService.GetUpcomingMatchesApplicableForUser(userId));
        }

        [HttpPost]
        [Route("{matchId}/check-in")]
        public async Task<IActionResult> CheckInForMatch(int userId, int matchId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _matchesService.CheckInForMatch(userId, matchId));
        }

        [HttpPost]
        [Route("{matchId}/give-up")]
        public async Task<IActionResult> GiveUpForMatch(int userId, int matchId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _matchesService.GiveUpForMatch(userId, matchId));
        }

        [HttpPost]
        [Route("{matchId}/confirm")]
        public async Task<IActionResult> ConfirmForMatch(int userId, int matchId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _matchesService.ConfirmForMatch(userId, matchId));
        }

        [HttpGet]
        [Route("{matchId}/status/{userId}")]
        public async Task<IActionResult> GetUserStatusForMatchday(int matchId, int userId)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _matchesService.GetUserStatusForMatchday(matchId, userId));
        }

        [HttpPost]
        [Route("organize-match")]
        public async Task<IActionResult> OrganizeMatch(OrganizeMatchDto organizeMatchDto)
        {
            return Ok(await _matchesService.OrganizeMatch(organizeMatchDto));
        }

        [HttpGet]
        [Route("organized-match/{matchdayId}")]
        public async Task<IActionResult> GetOrganizedMatchInformation(int matchdayId)
        {
            return Ok(await _matchesService.GetOrganizedMatchInformation(matchdayId));
        }

        [HttpGet]
        [Route("match-history/{userId}")]
        public async Task<IActionResult> GetMatchHistoryForUser(int userId)
        {
            return Ok(await _matchesService.GetMatchHistoryForUser(userId));
        }

        [HttpGet]
        [Route("match-history-group/{groupId}")]
        public async Task<IActionResult> GetMatchHistoryForGroup(int groupId)
        {
            return Ok(await _matchesService.GetMatchHistoryForGroup(groupId));
        }

        [HttpGet]
        [Route("latest-five-matches/{userId}")]
        public async Task<IActionResult> GetLatestFiveMatchesForUser(int userId)
        {
            return Ok(await _matchesService.GetLatestFiveMatchesForUser(userId));
        }
    }
}