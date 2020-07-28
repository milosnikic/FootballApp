using System;

namespace FootballApp.API.Dtos
{
    public class CommentForCreationDto
    {
        public int CommenterId { get; set; }
        public int CommentedId { get; set; }
        public string Content { get; set; }
        public DateTime DateCommented { get; set; }
        public CommentForCreationDto()
        {
            DateCommented = DateTime.Now;
        }
    }
}