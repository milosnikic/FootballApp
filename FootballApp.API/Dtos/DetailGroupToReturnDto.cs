using System;
using System.Collections.Generic;

namespace FootballApp.API.Dtos
{
    public class DetailGroupToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Favorite { get; set; }
        public int NumberOfMembers { get; set; }
        public ICollection<UserToReturnDto> LatestJoined { get; set; }
        public ICollection<UserToReturnDto> PendingRequests { get; set; }
        public ICollection<UserToReturnDto> Members { get; set; }
        public LocationToReturnDto Location { get; set; }
        public byte[] Image { get; set; }
        
    }
}