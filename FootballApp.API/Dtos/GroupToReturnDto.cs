using System;

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
        public int Status { get; set; }
        public LocationToReturnDto Location { get; set; }

        public byte[] Image { get; set; }
        public GroupToReturnDto()
        {
            Favorite = true;
        }
    }
}