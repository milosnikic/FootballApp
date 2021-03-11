using System.ComponentModel.DataAnnotations;

namespace FootballApp.API.Dtos
{
    public class FriendRequestDto
    {
        [Required]
        public int SenderId { get; set; }
        [Required]
        public int ReceiverId { get; set; }
        public bool Accpeted { get; set; }
    }
}