namespace FootballApp.API.Models
{
    public class TeamMemberStatistic
    {
        public int Id { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int MinutesPlayed { get; set; }
        public double Rating { get; set; }
        public int TeamMemberId { get; set; }
        public TeamMember TeamMember { get; set; }
        public int MatchPlayedId { get; set; }
        public MatchPlayed MatchPlayed { get; set; }
    }
}