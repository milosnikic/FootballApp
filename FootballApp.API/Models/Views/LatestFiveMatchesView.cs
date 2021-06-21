using System;

namespace FootballApp.API.Models.Views
{
    public class LatestFiveMatchesView
    {
        public int Id { get; set; }
        public DateTime DatePlayed { get; set; }
        public string Result { get; set; }
        public string HomeName { get; set; }
        public string AwayName { get; set; }
        public string Place { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public double Rating { get; set; }
    }
}
