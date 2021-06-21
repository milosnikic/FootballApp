using System;

namespace FootballApp.API.Models.Views
{
    public class MatchHistoryView
    {
        public int Id { get; set; }
        public DateTime DatePlayed { get; set; }
        public string Result { get; set; }
        public string HomeName { get; set; }
        public string AwayName { get; set; }
        public string Place { get; set; }
    }
}
