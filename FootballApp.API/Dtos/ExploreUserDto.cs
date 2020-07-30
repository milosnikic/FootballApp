namespace FootballApp.API.Dtos
{
    public class ExploreUserDto
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public byte[] MainPhoto { get; set; }
    }
}