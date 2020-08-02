using System.Collections.Generic;

namespace FootballApp.API.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
        public int CountryId { get; set; }
        public ICollection<Location> Locations { get; set; }
        public ICollection<User> Users { get; set; }
    }
}