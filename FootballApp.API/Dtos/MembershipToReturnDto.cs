using System;
using FootballApp.API.Models;

namespace FootballApp.API.Dtos
{
    public class MembershipToReturnDto
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime DateAccepted { get; set; }
        public bool Accepted { get; set; }
        public Role Role { get; set; }
    }
}