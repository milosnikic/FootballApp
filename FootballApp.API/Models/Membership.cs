using System;

namespace FootballApp.API.Models
{
    public enum Role
    {
        Admin,
        Owner,
        Member
    }
    public class Membership
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public DateTime DateSent { get; set; }
        public DateTime DateAccepted { get; set; }
        public bool Accepted { get; set; }
        public Role Role { get; set; }
    }
}