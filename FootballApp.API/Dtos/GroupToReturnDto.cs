using System;
using FootballApp.API.Models;

namespace FootballApp.API.Dtos
{
    public class GroupToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Favorite { get; set; }
        public int NumberOfMembers { get; set; }
        public MembershipStatus MembershipStatus { get; set; }
        public LocationToReturnDto Location { get; set; }
        public byte[] Image { get; set; }
        public GroupToReturnDto()
        {
            Favorite = true;
        }
    }
}