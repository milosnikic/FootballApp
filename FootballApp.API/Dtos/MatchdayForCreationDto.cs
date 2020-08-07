using System;
using System.ComponentModel.DataAnnotations;

namespace FootballApp.API.Dtos
{
    public class MatchdayForCreationDto
    {
        [Required]
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DatePlaying { get; set; }
        public DateTime DateCreated { get; set; }
        public int NumberOfPlayers { get; set; }
        public string Location { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public MatchdayForCreationDto()
        {
            DateCreated = DateTime.Now;
        }
    }
}