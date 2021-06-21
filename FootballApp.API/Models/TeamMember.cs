using System.Collections.Generic;

namespace FootballApp.API.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public Position? Position { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public TeamMemberStatistic TeamMemberStatistics { get; set; }
    }
}