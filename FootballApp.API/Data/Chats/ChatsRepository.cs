using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Chats
{
    public class ChatsRepository : Repository<Chat>, IChatsRepository
    {
        public DataContext DataContext
        {
            get
            {
                return Context as DataContext;
            }
        }
        public ChatsRepository(DataContext context) : base(context) { }

        public async Task<KeyValuePair<bool, string>> CreateGroupChat(string name, int ownerId)
        {
            var user = await DataContext.Users.FirstOrDefaultAsync(u => u.Id == ownerId);

            if (user == null)
            {
                return new KeyValuePair<bool, string>(false, "Specified user doesn't exist");
            }

            var chat = new Chat
            {
                Name = name == null ? "New chat" : name,
                Type = ChatType.Public
            };

            chat.Users.Add(new ChatUser
            {
                UserId = ownerId,
                Role = Role.Owner
            });

            DataContext.Chats.Add(chat);

            return new KeyValuePair<bool, string>(true, "Group chat created successfully");
        }

        public async Task<KeyValuePair<bool, string>> CreatePrivateChat(int userFrom, int userTo)
        {
            var userSender = await DataContext.Users.FirstOrDefaultAsync(u => u.Id == userFrom);
            var userReceiver = await DataContext.Users.FirstOrDefaultAsync(u => u.Id == userTo);

            if (userSender == null || userReceiver == null)
            {
                return new KeyValuePair<bool, string>(false, "You must provide valid user ids");
            }

            var chat = new Chat
            {
                Type = ChatType.Private
            };

            chat.Users.Add(
                new ChatUser
                {
                    UserId = userFrom
                }
            );

            chat.Users.Add(
                new ChatUser
                {
                    UserId = userTo
                }
            );

            DataContext.Chats.Add(chat);

            return new KeyValuePair<bool, string>(true, "Private chat created successfully");
        }

        public async Task<KeyValuePair<bool, string>> JoinRoom(int id, int userId)
        {
            var user = await DataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new KeyValuePair<bool, string>(false, "You must provide valid user id");
            }
            
            var chat = await DataContext.Chats.FirstOrDefaultAsync(c => c.Id == id);
            if (chat == null)
            {
                return new KeyValuePair<bool, string>(false, "You must provide valid chat id");
            }

            var chatUser = new ChatUser
            {
                ChatId = id,
                UserId = userId,
                Role = Role.Member
            };

            DataContext.ChatUsers.Add(chatUser);

            return new KeyValuePair<bool, string>(true, $"Successfully joined room {id}");
        }

        public async Task<Chat> GetChatWithMessages(int id)
        {
            var chat = await DataContext.Chats
                                        .Include(c => c.Messages)
                                        .Include(c => c.Users)
                                        .ThenInclude(u => u.User)
                                        .ThenInclude(u => u.Photos)
                                        .FirstOrDefaultAsync(c => c.Id == id);

            return chat;
        }

        public async Task<IEnumerable<Chat>> GetPrivateChats(int userId)
        {
            var chats = await DataContext.Chats.Include(c => c.Messages)
                                               .Include(c => c.Users)
                                               .ThenInclude(u => u.User)
                                               .ThenInclude(u => u.Photos)
                                               .Where(x => x.Type == ChatType.Private && x.Users.Any(y => y.UserId == userId))
                                               .ToListAsync();

            return chats;
        }

        public async Task<IEnumerable<User>> GetAvailableUsers(int userId)
        {
            // All users
            var users = await DataContext.Users.Include(u => u.Photos)
                                               .Where(u => u.Id != userId)
                                               .ToListAsync();

            // From all users we need to remove users that already have chat
            var busyUsers = await DataContext.Chats.Include(c => c.Users)
                                                        .ThenInclude(u => u.User)
                                                        .Where(x => x.Type == ChatType.Private && x.Users.Any(y => y.UserId == userId))
                                                        .SelectMany(x => x.Users)
                                                        .ToListAsync();
            
            busyUsers.RemoveAll(x => x.UserId == userId);
            var busyUsersList = busyUsers.Select(x => x.User);
            var finalList = users.Where(u => !busyUsersList.Any(b => b.Id == u.Id)).ToList();

            return finalList;
        }
    }
}
