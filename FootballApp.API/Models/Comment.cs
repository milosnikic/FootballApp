using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballApp.API.Models
{
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public User Commenter { get; set; }
        public int CommenterId { get; set; }
        public User Commented { get; set; }
        public int CommentedId { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
}