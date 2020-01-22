using System;
using FootballApp.API.Models;

namespace FootballApp.API.Dtos
{
    public class PhotoToReturnDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateADded { get; set; }
        public bool IsMain { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}