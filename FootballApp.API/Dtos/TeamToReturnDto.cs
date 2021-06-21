namespace FootballApp.API.Dtos
{
    public class TeamToReturnDto
    {
        public string Name { get; set; }
        public TeamMemberDto Players { get; set; }
    }
}