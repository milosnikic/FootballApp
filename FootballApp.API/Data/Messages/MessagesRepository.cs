using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Messages
{
    public class MessagesRepository : Repository<Message>, IMessagesRepository
    {
        public MessagesRepository(DataContext context) : base(context) { }


        public DataContext DataContext
        {
            get
            {
                return Context as DataContext;
            }
        }

        public async Task<KeyValuePair<bool, string>> DeleteMessage(int messageId, int chatId, int userId)
        {
            var message = await DataContext.Messages.FirstOrDefaultAsync(m => m.Id == messageId && m.ChatId == chatId);
            if (message != null)
            {
                Remove(message);
                return new KeyValuePair<bool, string>(true, $"Message {messageId} (id) has been deleted successfully");
            }
            return new KeyValuePair<bool, string>(false, "Message cannot be deleted");
        }


        public async Task<KeyValuePair<bool, string>> ReadMessage(int messageId, int chatId, int userId)
        {
            var message = await DataContext.Messages.FirstOrDefaultAsync(m => m.Id == messageId && m.ChatId == chatId);
            
            if (message != null && !message.IsRead)
            {
                message.IsRead = true;
                message.DateRead = DateTime.Now;
                return new KeyValuePair<bool, string>(true, $"Message {messageId} (id) has been read successfully");
            }

            return new KeyValuePair<bool, string>(false, "Message cannot be read");
        }

        public async Task<KeyValuePair<bool, string>> SendMessage(Message message)
        {
            var isUserIdValid = await DataContext.Users.FirstOrDefaultAsync(u => u.Id == message.SenderId) != null;
            var isChatIdValid = await DataContext.Chats.FirstOrDefaultAsync(c => c.Id == message.ChatId) != null;
            
            if (isUserIdValid && isChatIdValid)
            {
                Add(message);
                return new KeyValuePair<bool, string>(true, "Message has been successfully sent!");
            }

            return new KeyValuePair<bool, string>(false, "Message cannot be sent!");
        }
    }
}