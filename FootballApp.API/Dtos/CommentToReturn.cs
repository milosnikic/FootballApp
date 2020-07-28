using System;

namespace FootballApp.API.Dtos
{
    public class CommentToReturn
    {
        public int CommenterId { get; set; }
        public UserToReturnDto Commenter { get; set; }
        public int CommentedId { get; set; }
        public UserToReturnDto Commented { get; set; }
        public string Content { get; set; }
        public DateTime DateCommented { get; set; }
    }
}