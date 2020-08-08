using FootballApp.API.Models;

namespace FootballApp.API.Dtos
{
    public class MembershipInformationDto
    {
        public bool Favorite { get; set; } = false;
        public MembershipStatus MembershipStatus { get; set; }
    }
}