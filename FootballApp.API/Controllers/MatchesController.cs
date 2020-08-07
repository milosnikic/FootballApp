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
        public async Task<IActionResult> GetMatch(int matchId)
        {
            var match = await _unitOfWork.Matchdays.GetMatchdayWithLocation(matchId);
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
            || membership.MembershipStatus == MembershipStatus.Sent
            || membership.Role == Role.Member)
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
        public async Task<IActionResult> GetUpcomingMatchesForGroup(int matchId)
        {
            return Ok();
        }

    }
}