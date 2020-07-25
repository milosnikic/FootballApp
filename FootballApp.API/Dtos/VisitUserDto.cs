using System;

namespace FootballApp.API.Dtos
{
    public class VisitUserDto
    {
        public int VisitorId { get; set; }
        public int VisitedId { get; set; }
        public DateTime DateVisited { get; set; }
    }
}