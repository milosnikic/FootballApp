namespace FootballApp.API.Dtos
{
    public class LoginUserToReturnDto
    {
        public UserToReturnDto User { get; set; }
        public string Token { get; set; }
    }
}