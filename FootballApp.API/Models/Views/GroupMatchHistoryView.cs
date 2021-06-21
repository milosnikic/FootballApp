using System;

namespace FootballApp.API.Models.Views
{
    public class GroupMatchHistoryView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DatePlaying { get; set; }
        public int NumberOfPlayers { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public bool IsOrganized { get; set; }
        public int? NumberOfConfirmedPlayers { get; set; }
        public bool CanBeOrganized { get; set; }
        public string HomeName { get; set; }
        public int? HomeGoals { get; set; }
        public string AwayName { get; set; }
        public int? AwayGoals { get; set; }
    }
}
