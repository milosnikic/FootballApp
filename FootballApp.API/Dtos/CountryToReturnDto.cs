using System.Collections.Generic;

namespace FootballApp.API.Dtos
{
    public class CountryToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CityToReturnDto> Cities { get; set; }
    }
}