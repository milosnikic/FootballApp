using System.ComponentModel.DataAnnotations;

namespace FootballApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Username length must be between 5 and 15 characters")]
        [RegularExpression("^[0-9]*[a-zA-Z]+[a-zA-Z0-9]*$",
            ErrorMessage = "Username can only contain characters, numbers and '.','-','_'")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public UserForRegisterDto()
        {
            IsActive = true;
        }

    }
}