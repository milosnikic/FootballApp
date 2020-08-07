namespace FootballApp.API.Models
{
    public class MatchStatus
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public Matchday Matchday { get; set; }
        public int MatchdayId { get; set; }
        public bool? Checked { get; set; }
        public bool? Confirmed { get; set; }
    }
}