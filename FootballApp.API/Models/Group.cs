namespace FootballApp.API.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}