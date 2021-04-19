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
    public abstract class User
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
        public City City { get; set; }
        public int? CityId { get; set; }
        public Country Country { get; set; }
        public int? CountryId { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Friendship> FriendshipsSent { get; set; }
        public ICollection<Friendship> FriendshipsReceived { get; set; }
        public ICollection<Membership> Memberships { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Visit> Visitors { get; set; }
        public ICollection<Visit> Visiteds { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Comment> Commented { get; set; }
        public ICollection<GainedAchievement> GainedAchievements { get; set; }
        public ICollection<MatchStatus> MatchStatuses { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<ChatUser> Chats { get; set; }
    }
}
