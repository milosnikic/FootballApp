using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Chats
{
    public interface IChatsRepository : IRepository<Chat>
    {
        Task<KeyValuePair<bool, string>> CreateGroupChat(string name, int ownerId);
        Task<KeyValuePair<bool, string>> CreatePrivateChat(int userFrom, int userTo);
        Task<KeyValuePair<bool, string>> JoinRoom(int id, int userId);
        Task<Chat> GetChatWithMessages(int id);
        Task<IEnumerable<Chat>> GetPrivateChats(int userId);
        Task<IEnumerable<User>> GetAvailableUsers(int userId);
    }
}