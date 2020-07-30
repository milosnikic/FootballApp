using System.Collections.Generic;

namespace FootballApp.API.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}