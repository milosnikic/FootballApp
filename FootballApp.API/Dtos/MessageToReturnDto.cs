using System;

namespace FootballApp.API.Dtos
{
    public class MessageToReturnDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime MessageSent { get; set; }
        public UserToReturnMiniDto Sender { get; set; }
    }
}
