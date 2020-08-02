using System.ComponentModel.DataAnnotations;

namespace FootballApp.API.Dtos
{
    public class UserToUpdateDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public int City { get; set; }
        [Required]
        public int Country { get; set; }
    }
}