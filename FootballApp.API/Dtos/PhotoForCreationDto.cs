using System;
using Microsoft.AspNetCore.Http;

namespace FootballApp.API.Dtos
{
    public class PhotoForCreationDto
    {
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public PhotoForCreationDto()
        {
            DateAdded = DateTime.Now;
            IsMain = false;
        }
    }
}