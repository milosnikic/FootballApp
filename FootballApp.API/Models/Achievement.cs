using System.Collections.Generic;

namespace FootballApp.API.Models
{
    public class Achievement
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int Points { get; set; }
        public ICollection<GainedAchievement> GainedAchievements { get; set; }
    }
}