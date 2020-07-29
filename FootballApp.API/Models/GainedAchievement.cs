using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballApp.API.Models
{
    public class GainedAchievement
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int AchievementId { get; set; }
        public Achievement Achievement { get; set; }
        public DateTime DateAchieved { get; set; }

        public GainedAchievement()
        {
            DateAchieved = DateTime.Now;
        }
    }
}