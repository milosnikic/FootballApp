using System;

namespace FootballApp.API.Dtos
{
    public class GainedAchievementToReturnDto
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public DateTime DateAchieved { get; set; }
    }
}