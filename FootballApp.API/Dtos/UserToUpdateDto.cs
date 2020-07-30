using System.ComponentModel.DataAnnotations;

namespace FootballApp.API.Dtos
{
    public class UserToUpdateDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
    }
}