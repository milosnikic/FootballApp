using System;

namespace FootballApp.API.Models
{
    public class GainedAchievement
    {
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