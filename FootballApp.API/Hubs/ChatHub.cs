using Microsoft.AspNetCore.SignalR;

namespace FootballApp.API.Hubs
{
    public class ChatHub : Hub
    {
        public string GetConnectionId() => Context.ConnectionId;
    }
}