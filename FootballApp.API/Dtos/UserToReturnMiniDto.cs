namespace FootballApp.API.Dtos
{
    public class UserToReturnMiniDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public byte[] MainPhoto { get; set; }
    }
}