using System;
using Microsoft.AspNetCore.Http;

namespace FootballApp.API.Dtos
{
    public class GroupForCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public IFormFile Image { get; set; }
        public string Location { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public DateTime DateCreated { get; set; }
        public GroupForCreationDto()
        {
            IsActive = true;
            DateCreated = DateTime.Now;
            Image = null;
        }
    }
}