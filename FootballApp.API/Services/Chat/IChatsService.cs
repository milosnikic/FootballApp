using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Dtos;

namespace FootballApp.API.Services.Chat
{
    public interface IChatsService
    {
        Task JoinRoom(string connectionId, string roomId);
        Task<KeyValuePair<bool, string>> JoinRoom(int id, int userId);
        Task LeaveRoom(string connectionId, string roomId);
        Task<ChatToReturnDto> Chat(int id);
        Task<ICollection<ChatToReturnDto>> GetPrivateChats(int userId);
        Task<KeyValuePair<bool, string>> SendMessage(MessageToSendDto message, int userId);
        Task<KeyValuePair<bool, string>> CreateGroupChat(string name, int ownerId);
        Task<KeyValuePair<bool, string>> CreatePrivateChat(int userId, int loggedInUserId);
        Task<ICollection<UserToReturnMiniDto>> GetAvailableUsers(int userId);
    }
}