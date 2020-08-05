namespace FootballApp.API.Models
{
    public class CommonUser : User
    {
        public bool IsSubscribed { get; set; } = false;

    }
}