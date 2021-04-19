using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Messages
{
    public interface IMessagesRepository : IRepository<Message>
    {
        Task<KeyValuePair<bool, string>> SendMessage(Message message);
        Task<KeyValuePair<bool, string>> ReadMessage(int messageId, int chatId, int userId);
        Task<KeyValuePair<bool, string>> DeleteMessage(int messageId, int chatId, int userId);
    }
}