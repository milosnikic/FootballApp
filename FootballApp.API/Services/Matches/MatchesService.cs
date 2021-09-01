using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using FootballApp.API.Models.Views;

namespace FootballApp.API.Services.Matches
{
    public class MatchesService : IMatchesService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public MatchesService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<KeyValuePair<bool, string>> CheckInForMatch(int userId, int matchId)
        {
            var match = await _unitOfWork.Matchdays.GetById(matchId);
            if (match == null)
            {
                return new KeyValuePair<bool, string>(false, "Specified match doesn't exist!");
            }

            var matchStatus = await _unitOfWork.MatchStatuses.GetMatchStatusById(userId, matchId);
            if (matchStatus != null)
            {
                return new KeyValuePair<bool, string>(false, "You are already checked in!");
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
                return new KeyValuePair<bool, string>(true, "Successfully checked in for match!");
            }

            return new KeyValuePair<bool, string>(false, "Couldn't check in for match!");
        }

        public async Task<KeyValuePair<bool, string>> ConfirmForMatch(int userId, int matchId)
        {
            var match = await _unitOfWork.Matchdays.GetById(matchId);
            if (match == null)
            {
                return new KeyValuePair<bool, string> (false, "Specified match doesn't exist!");
            }

            var matchStatus = await _unitOfWork.MatchStatuses.GetMatchStatusById(userId, matchId);
            if (matchStatus == null)
            {
                return new KeyValuePair<bool, string> (false, "You are not checked in for match.");
            }
            matchStatus.Checked = false;
            matchStatus.Confirmed = true;

            if (await _unitOfWork.Complete())
            {
                return new KeyValuePair<bool, string>(true, "Successfully confirmed for match!");
            }

            return new KeyValuePair<bool, string>(false, "Couldn't confirm for match!");
        }

        public async Task<KeyValuePair<bool, string>> CreateMatch(MatchdayForCreationDto matchdayForCreation, int userId)
        {
            var group = await _unitOfWork.Groups.GetById(matchdayForCreation.GroupId);
            if (group == null)
            {
                return new KeyValuePair<bool, string>(false,"Specified group doesn't exist.");
            }

            var membership = await _unitOfWork.Memberships.GetMembershipById(userId, group.Id);
            if (membership == null
            || membership.MembershipStatus == MembershipStatus.NotMember
            || membership.MembershipStatus == MembershipStatus.Sent)
            // || membership.Role == Role.Member
            {
                return new KeyValuePair<bool, string>(false, "You are not allowed to create match!");
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
                return new KeyValuePair<bool, string>(true, "Matchday has been successfully created.");
            }

            return new KeyValuePair<bool, string>(false, "Matchday has not been created.");
        }

        public async Task<IEnumerable<LatestFiveMatchesView>> GetLatestFiveMatchesForUser(int userId)
        {
            return await _unitOfWork.Matchdays.GetLatestFiveMatchesForUser(userId);
        }

        public async Task<IEnumerable<GroupMatchHistoryView>> GetMatchHistoryForGroup(int groupId)
        {
            return await _unitOfWork.Matchdays.GetMatchHistoryForGroup(groupId);
        }

        public async Task<IEnumerable<MatchHistoryView>> GetMatchHistoryForUser(int userId)
        {
            return await _unitOfWork.Matchdays.GetMatchHistoryForUser(userId);
        }

        public async Task<IEnumerable<OrganizedMatchInformationView>> GetOrganizedMatchInformation(int matchdayId)
        {
            return await _unitOfWork.Matchdays.GetOrganizedMatchInformation(matchdayId);
        }

        public async Task<MatchdayToReturnDto> GetUpcomingMatchday(int matchId)
        {
            return _mapper.Map<MatchdayToReturnDto>(await _unitOfWork.Matchdays.GetMatchdayWithAdditionalInformation(matchId));
        }

        public async Task<IEnumerable<MatchdayToReturnDto>> GetUpcomingMatchesApplicableForUser(int userId)
        {
            return _mapper.Map<ICollection<MatchdayToReturnDto>>(await _unitOfWork.Matchdays.GetUpcomingMatchesApplicableForUser(userId));
        }

        public async Task<IEnumerable<MatchdayToReturnDto>> GetUpcomingMatchesForGroup(int groupId)
        {
            return _mapper.Map<ICollection<MatchdayToReturnDto>>(await _unitOfWork.Matchdays.GetUpcomingMatchesForGroup(groupId));
        }

        public async Task<IEnumerable<MatchdayForDisplayDto>> GetUpcomingMatchesForUser(int userId)
        {
            var user = await _unitOfWork.Users.GetUserByIdWithAdditionalInformation(userId);
            if (user != null)
            {
                return _mapper.Map<ICollection<MatchdayForDisplayDto>>(await _unitOfWork.Matchdays.GetUpcomingMatchesForUser(userId));
            }
            
            return new List<MatchdayForDisplayDto>();
        }

        public async Task<MatchStatusToReturnDto> GetUserStatusForMatchday(int matchId, int userId)
        {
            var match = await _unitOfWork.Matchdays.GetById(matchId);
            if (match == null)
            {
                return new MatchStatusToReturnDto();
            }

            var matchStatus = await _unitOfWork.MatchStatuses.GetMatchStatusById(userId, matchId);
            return _mapper.Map<MatchStatusToReturnDto>(matchStatus);
        }

        public async Task<KeyValuePair<bool, string>> GiveUpForMatch(int userId, int matchId)
        {
            var match = await _unitOfWork.Matchdays.GetById(matchId);
            if (match == null)
            {
                return new KeyValuePair<bool, string>(false, "Specified match doesn't exist!");
            }

            var matchStatus = await _unitOfWork.MatchStatuses.GetMatchStatusById(userId, matchId);
            if (matchStatus == null)
            {
                return new KeyValuePair<bool, string>(false, "You are not checked in for match.");
            }

            _unitOfWork.MatchStatuses.Remove(matchStatus);

            if (await _unitOfWork.Complete())
            {
                return new KeyValuePair<bool, string>(true, "Successfully gave up a match!");
            }

            return new KeyValuePair<bool, string>(false, "Couldn't give up for match!");
        }

        public async Task<KeyValuePair<bool,string>> OrganizeMatch(OrganizeMatchDto organizeMatchDto)
        {
            return await _unitOfWork.Matchdays.OrganizeMatchPlayed(organizeMatchDto);
        }
    }
}