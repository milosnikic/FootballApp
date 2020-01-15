using System;

namespace FootballApp.API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int CommenterId { get; set; }
        public User Commenter { get; set; }
        public int CommenteeId { get; set; }
        public User Commentee { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
}