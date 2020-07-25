using System;

namespace FootballApp.API.Dtos
{
    public class VisitToReturnDto
    {
        public UserToReturnDto Visitor { get; set; }
        public DateTime DateVisited { get; set; }
    }
}