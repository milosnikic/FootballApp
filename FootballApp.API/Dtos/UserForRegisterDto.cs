using System;
using System.ComponentModel.DataAnnotations;
using FootballApp.API.Models;

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
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastActive { get; set; }
        [Required]
        public Gender Gender { get; set; } = Gender.Other;
        public bool IsActive { get; set; }

        public UserForRegisterDto()
        {
            IsActive = true;
            Created = DateTime.Now;
        }

    }
}