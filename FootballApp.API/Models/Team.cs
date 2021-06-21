using System.Collections.Generic;

namespace FootballApp.API.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Matchday Matchday { get; set; }
        public ICollection<MatchPlayed> HomeMatches { get; set; }
        public ICollection<MatchPlayed> AwayMatches { get; set; }
        public ICollection<TeamMember> TeamMembers { get; set; }
    }
}