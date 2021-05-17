using System.Collections.Generic;

namespace FootballApp.API.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        public Position? Position { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<TeamMemberStatistic> TeamMemberStatistics { get; set; }
    }
}