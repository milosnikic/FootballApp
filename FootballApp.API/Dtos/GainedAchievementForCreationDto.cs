using System;
using System.ComponentModel.DataAnnotations;

namespace FootballApp.API.Dtos
{
    public class GainedAchievementForCreationDto
    {
        [Required]
        public int UserId { get; set; }
        public int Value { get; set; }
        public DateTime DateAchieved { get; set; }
    }
}