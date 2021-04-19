using System;
using System.ComponentModel.DataAnnotations;

namespace FootballApp.API.Dtos
{
    public class MessageToSendDto
    {
        [Required]
        public string Content { get; set; }
        public int ChatId { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.Now;
    }
}