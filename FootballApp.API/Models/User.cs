using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballApp.API.Models
{
    public enum Gender{
        Male,
        Female,
        Other
    }
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? LastActive { get; set; }
        public DateTime Created { get; set; }
        public Gender Gender { get; set; } = Gender.Other;
        public string City { get; set; }
        public string Country { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Membership> Memberships { get; set; }
        public ICollection<Photo> Photos { get; set; }

    }
}