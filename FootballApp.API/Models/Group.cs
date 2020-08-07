using System;
using System.Collections.Generic;

namespace FootballApp.API.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<Membership> Memberships { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public byte[] Image { get; set; }
        public PowerUser CreatedBy { get; set; }
        public ICollection<Matchday> Matchdays { get; set; }
    }
}