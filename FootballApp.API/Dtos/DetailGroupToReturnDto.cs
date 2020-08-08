using System;
using System.Collections.Generic;
using FootballApp.API.Models;

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
        public bool IsMember { get; set; }
        public ICollection<UserForDisplayDto> LatestJoined { get; set; }
        public ICollection<UserForDisplayDto> PendingRequests { get; set; }
        public ICollection<UserForDisplayDto> Members { get; set; }
        public Location Location { get; set; }
        public byte[] Image { get; set; }
        
    }
}