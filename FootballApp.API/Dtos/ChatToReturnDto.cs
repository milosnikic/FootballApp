using System;
using System.Collections.Generic;
using FootballApp.API.Models;

namespace FootballApp.API.Dtos
{
    public class ChatToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ChatType Type { get; set; }
        public DateTime LastMessage { get; set; }
        public ICollection<MessageToReturnDto> Messages { get; set; }
        public ICollection<UserToReturnMiniDto> Users { get; set; }
    }
}