using System;

namespace FootballApp.API.Dtos
{
    public class MatchdayForDisplayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Confirmed { get; set; }
        public bool Checked { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int NumberOfPlayers { get; set; }
        public int NumberOfConfirmedPlayers { get; set; }
        public DateTime DatePlaying { get; set; }
    }
}