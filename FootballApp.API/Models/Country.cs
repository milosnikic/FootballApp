using System.Collections.Generic;

namespace FootballApp.API.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<City> Cities { get; set; }
        public ICollection<Location> Locations { get; set; }
    }
}