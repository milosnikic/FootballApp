using System;

namespace FootballApp.API.Models
{
    public enum Role
    {
        Admin,
        Owner,
        Member
    }
    public enum MembershipStatus
    {
        NotMember,
        Sent,
        Accepted
    }
    public class Membership
    {
        public Membership()
        {
            DateAccepted = null;
            DateSent = DateTime.Now;
            Accepted = false;
            Role = Role.Member;
        }
        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public DateTime DateSent { get; set; }
        public bool Favorite { get; set; }
        public DateTime? DateAccepted { get; set; }
        public MembershipStatus MembershipStatus { get; set; }
        public bool Accepted { get; set; }
        public Role Role { get; set; }
    }
}