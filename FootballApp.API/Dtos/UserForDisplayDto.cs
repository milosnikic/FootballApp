using System;

namespace FootballApp.API.Dtos
{
    public class UserForDisplayDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public DateTime? LastActive { get; set; }
        public DateTime Created { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Flag { get; set; }
        public PhotoToReturnDto MainPhoto { get; set; }
    }
}