using System.Collections.Generic;
using FootballApp.API.Models;

namespace FootballApp.API.Dtos
{
    public class OrganizeMatchDto
    {
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public int MatchdayId { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public ICollection<TeamMemberDto> HomeTeamMembers { get; set; }
        public ICollection<TeamMemberDto> AwayTeamMembers { get; set; }
    }
}