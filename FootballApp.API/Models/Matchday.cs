using System;
using System.Collections.Generic;

namespace FootballApp.API.Models
{
    public class Matchday
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DatePlaying { get; set; }
        public DateTime DateCreated { get; set; }
        public int NumberOfPlayers { get; set; }
        public Location Location { get; set; }
        public Group Group { get; set; }
        public ICollection<MatchStatus> MatchStatuses { get; set; }
    }
}