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
    public class MatchesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MatchesController(IUnitOfWork unitOfWork,
                                IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }

        [HttpGet]
        [Route("{matchId}")]
        public async Task<IActionResult> GetUpcomingMatchday(int matchId)
        {
            var match = _mapper.Map<MatchdayToReturnDto>(await _unitOfWork.Matchdays.GetMatchdayWithAdditionalInformation(matchId));
            return Ok(match);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateMatch(MatchdayForCreationDto matchdayForCreation, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var group = await _unitOfWork.Groups.GetById(matchdayForCreation.GroupId);
            if (group == null)
            {
                return BadRequest("Specified group doesn't exist.");
            }

            var membership = await _unitOfWork.Memberships.GetMembershipById(userId, group.Id);
            if (membership == null
            || membership.MembershipStatus == MembershipStatus.NotMember
            || membership.MembershipStatus == MembershipStatus.Sent)
            // || membership.Role == Role.Member
            {
                return Unauthorized();
            }

            var match = _mapper.Map<Matchday>(matchdayForCreation);

            // We check if selected location exists
            // If it doesn't we create a new one with specified country and city
            var location = await _unitOfWork.Locations.GetByName(matchdayForCreation.Name);
            if (location == null)
            {
                location = new Location { Name = matchdayForCreation.Location, CityId = matchdayForCreation.CityId, CountryId = matchdayForCreation.CountryId };
                _unitOfWork.Locations.Add(location);
            }

            match.Location = location;
            match.Group = group;
            _unitOfWork.Matchdays.Add(match);

            if (await _unitOfWork.Complete())
            {
                return Ok(new KeyValuePair<bool, string>(true, "Matchday has been successfully created."));
            }

            return Ok(new KeyValuePair<bool, string>(false, "Matchday has not been created."));
        }

        [HttpGet]
        [Route("upcoming-matches/{groupId}")]
        public async Task<IActionResult> GetUpcomingMatchesForGroup(int groupId)
        {
            var matches = _mapper.Map<ICollection<MatchdayToReturnDto>>(await _unitOfWork.Matchdays.GetUpcomingMatchesForGroup(groupId));

            return Ok(matches);
        }

        [HttpGet]
        [Route("upcoming-matches")]
        public async Task<IActionResult> GetUpcomingMatchesForUser(int userId)
        {
            var user = await _unitOfWork.Users.GetUserByIdWithAdditionalInformation(userId);
            if(user != null)
            {
                return Ok(_mapper.Map<ICollection<MatchdayForDisplayDto>>(await _unitOfWork.Matchdays.GetUpcomingMatchesForUser(userId)));
            }
            return BadRequest("Specified user doesn't exist.");
        }

        [HttpGet]
        [Route("upcoming-matches-applicable")]
        public async Task<IActionResult> GetUpcomingMatchesApplicableForUser(int userId)
        {
            return Ok(_mapper.Map<ICollection<MatchdayToReturnDto>>(await _unitOfWork.Matchdays.GetUpcomingMatchesApplicableForUser(userId)));
        }

        [HttpPost]
        [Route("{matchId}/check-in")]
        public async Task<IActionResult> CheckInForMatch(int userId, int matchId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var match = await _unitOfWork.Matchdays.GetById(matchId);
            if (match == null)
            {
                return BadRequest("Specified match doesn't exist!");
            }

            var matchStatus = await _unitOfWork.MatchStatuses.GetMatchStatusById(userId, matchId);
            if (matchStatus != null)
            {
                return BadRequest("You are already checked in!");
            }

            matchStatus = new MatchStatus
            {
                UserId = userId,
                MatchdayId = match.Id,
                Matchday = match,
                Checked = true,
                Confirmed = null
            };
            _unitOfWork.MatchStatuses.Add(matchStatus);
            if (await _unitOfWork.Complete())
            {
                return Ok(new KeyValuePair<bool, string>(true, "Successfully checked in for match!"));
            }

            return Ok(new KeyValuePair<bool, string>(false, "Couldn't check in for match!"));
        }

        [HttpPost]
        [Route("{matchId}/give-up")]
        public async Task<IActionResult> GiveUpForMatch(int userId, int matchId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var match = await _unitOfWork.Matchdays.GetById(matchId);
            if (match == null)
            {
                return BadRequest("Specified match doesn't exist!");
            }

            var matchStatus = await _unitOfWork.MatchStatuses.GetMatchStatusById(userId, matchId);
            if (matchStatus == null)
            {
                return BadRequest("You are not checked in for match.");
            }

            _unitOfWork.MatchStatuses.Remove(matchStatus);

            if (await _unitOfWork.Complete())
            {
                return Ok(new KeyValuePair<bool, string>(true, "Successfully gave up a match!"));
            }

            return Ok(new KeyValuePair<bool, string>(false, "Couldn't give up for match!"));
        }

        [HttpPost]
        [Route("{matchId}/confirm")]
        public async Task<IActionResult> ConfirmForMatch(int userId, int matchId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var match = await _unitOfWork.Matchdays.GetById(matchId);
            if (match == null)
            {
                return BadRequest("Specified match doesn't exist!");
            }

            var matchStatus = await _unitOfWork.MatchStatuses.GetMatchStatusById(userId, matchId);
            if (matchStatus == null)
            {
                return BadRequest("You are not checked in for match.");
            }
            matchStatus.Checked = false;
            matchStatus.Confirmed = true;

            if (await _unitOfWork.Complete())
            {
                return Ok(new KeyValuePair<bool, string>(true, "Successfully confirmed for match!"));
            }

            return Ok(new KeyValuePair<bool, string>(false, "Couldn't confirm for match!"));
        }

        [HttpGet]
        [Route("{matchId}/status/{userId}")]
        public async Task<IActionResult> GetUserStatusForMatchday(int matchId, int userId)
        {

            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var match = await _unitOfWork.Matchdays.GetById(matchId);
            if (match == null)
            {
                return BadRequest("Specified match doesn't exist!");
            }

            var matchStatus = await _unitOfWork.MatchStatuses.GetMatchStatusById(userId, matchId);
            return Ok(_mapper.Map<MatchStatusToReturnDto>(matchStatus));
        }
    }
}