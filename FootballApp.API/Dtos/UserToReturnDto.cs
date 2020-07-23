using System;
using System.Collections.Generic;
using FootballApp.API.Models;

namespace FootballApp.API.Dtos
{
    public class UserToReturnDto
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
        public ICollection<MembershipToReturnDto> Memberships { get; set; }
        public ICollection<PhotoToReturnDto> Photos { get; set; }

    }
}