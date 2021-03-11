using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data.Friends
{
    public class FriendsRepository : Repository<Friendship>, IFriendsRepository
    {
        public FriendsRepository(DataContext context) : base(context)
        {
        }

        public async Task<KeyValuePair<bool, string>> DeleteFriendRequest(FriendRequestDto friendRequestDto)
        {
            var friendRequest = await DataContext.Friendships
                                                 .Where(f => f.ReceiverId == friendRequestDto.ReceiverId && f.SenderId == friendRequestDto.SenderId)
                                                 .FirstOrDefaultAsync();

            if (friendRequest != null)
            {
                Remove(friendRequest);
                return new KeyValuePair<bool, string>(true, "Friend request successfully deleted");
            }

            return new KeyValuePair<bool, string>(false, "Specified friend request doesn't exist");
        }

        public async Task<KeyValuePair<bool, string>> AcceptFriendRequest(FriendRequestDto friendRequestDto)
        {
            var friendRequest = await DataContext.Friendships
                                                 .Where(f => f.ReceiverId == friendRequestDto.ReceiverId && f.SenderId == friendRequestDto.SenderId)
                                                 .FirstOrDefaultAsync();

            if (friendRequest != null)
            {
                friendRequest.Accepted = true;
                Update(friendRequest);
                return new KeyValuePair<bool, string>(true, "Friend request successfully accepted");
            }

            return new KeyValuePair<bool, string>(false, "Specified friend request doesn't exist");
        }

        public async Task<IEnumerable<Friendship>> GetAllFriendsForUser(int userId)
        {
            var friends = await DataContext.Friendships
                                            .Where(f => (f.SenderId == userId || f.ReceiverId == userId) && f.Accepted)
                                            .Include(f => f.Sender)
                                            .ThenInclude(u => u.Photos)
                                            .Include(f => f.Receiver)
                                            .ThenInclude(u => u.Photos)
                                            .ToListAsync();

            return friends;
        }

        public async Task<IEnumerable<Friendship>> PendingFriendRequests(int userId)
        {
            var pendingRequests = await DataContext.Friendships
                                                    .Where(f => f.ReceiverId == userId && !f.Accepted)
                                                    .Include(f => f.Sender)
                                                    .ThenInclude(u => u.Photos)
                                                    .ToListAsync();

            return pendingRequests;
        }

        public async Task<KeyValuePair<bool, string>> SendFriendRequest(FriendRequestDto friendRequestDto)
        {
            var friendshipAlreadyExists = await DataContext.Friendships
                                                            .FirstOrDefaultAsync(f => (f.SenderId == friendRequestDto.SenderId
                                                                                    && f.ReceiverId == friendRequestDto.ReceiverId)
                                                                                   || (f.ReceiverId == friendRequestDto.SenderId
                                                                                    && f.SenderId == friendRequestDto.ReceiverId)) != null;

            if (!friendshipAlreadyExists)
            {
                var friendship = new Friendship()
                {
                    SenderId = friendRequestDto.SenderId,
                    ReceiverId = friendRequestDto.ReceiverId,
                    Accepted = false
                };
                Add(friendship);

                return new KeyValuePair<bool, string>(true, "Friend request sent successfully");
            }

            return new KeyValuePair<bool, string>(false, "Friend request cannot be sent");
        }

        public async Task<ICollection<User>> GetAllExploreUsers(int userId)
        {
            var user = await DataContext.Users
                                        .Include(u => u.FriendshipsReceived)
                                        .Include(u => u.FriendshipsSent)
                                        .FirstOrDefaultAsync(u => u.Id == userId);

            var usersFriendsReceivers = user.FriendshipsReceived.Select(f => f.SenderId).ToList();
            
            var usersFriendsSenders = user.FriendshipsSent.Select(f => f.ReceiverId).ToList();
            
            var exploreUsers = await DataContext.Users
                                                .Where(u => !usersFriendsReceivers.Contains(u.Id) && !usersFriendsSenders.Contains(u.Id) && u.Id != userId)
                                                .Include(u => u.Photos)
                                                .ToListAsync();

            return exploreUsers;
        }

        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }
    }
}