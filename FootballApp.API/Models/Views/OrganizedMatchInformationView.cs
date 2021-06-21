using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballApp.API.Models.Views
{
    public class OrganizedMatchInformationView
    {
        public int Id { get; set; }
        public DateTime DatePlayed { get; set; }
        public int? HomeId { get; set; }
        public int? AwayId { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Created { get; set; }
        public byte[] Image { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int MinutesPlayed { get; set; }
        public double Rating { get; set; }
        public string Place { get; set; }
    }
}
