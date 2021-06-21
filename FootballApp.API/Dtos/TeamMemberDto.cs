using FootballApp.API.Models;

namespace FootballApp.API.Dtos
{
    public class TeamMemberDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public TeamMemberStatistic Statistics { get; set; }
        public Position? Position { get; set; }
    }
}