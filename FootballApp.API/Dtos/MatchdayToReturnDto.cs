using System;
using System.Collections.Generic;

namespace FootballApp.API.Dtos
{
    public class MatchdayToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int NumberOfPlayers { get; set; }
        public int NumberOfConfirmedPlayers { get; set; }
        public DateTime DatePlaying { get; set; }
        public int GroupId { get; set; }
        public ICollection<UserForDisplayDto> AppliedUsers { get; set; }
    }
}