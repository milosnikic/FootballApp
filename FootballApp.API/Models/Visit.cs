using System;

namespace FootballApp.API.Models
{   
    public class Visit
    {
        public int VisitorId { get; set; }
        public User Visitor { get; set; }
        public int VisitedId { get; set; }
        public User Visited { get; set; }
        public DateTime DateVisited { get; set; }
    }
}