namespace FootballApp.API.Models
{
    public class Friendship
    {
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
        public bool Accepted { get; set; }
    }
}