using System;
using System.Collections.Generic;

namespace FootballApp.API.Models
{
    public class MatchPlayed
    {
        public int Id { get; set; }
        public DateTime DatePlayed { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public int HomeId { get; set; }
        public Team  Home { get; set; }
        public int AwayId { get; set; }
        public Team Away { get; set; }
        public ICollection<TeamMemberStatistic> TeamMemberStatistics { get; set; }
    }
}