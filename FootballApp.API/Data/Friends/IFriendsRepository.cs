using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Dtos;
using FootballApp.API.Models;

namespace FootballApp.API.Data.Friends
{
    public interface IFriendsRepository
    {
        Task<IEnumerable<User>> GetAllFriendsForUser(int userId);
        Task<IEnumerable<User>> PendingFriendRequests(int userId);
        Task<IEnumerable<User>> SentFriendRequests(int userId);
        Task<KeyValuePair<bool, string>> SendFriendRequest(FriendRequestDto friendRequestDto);
        Task<KeyValuePair<bool, string>> AcceptFriendRequest(FriendRequestDto friendRequestDto);
        Task<KeyValuePair<bool, string>> DeleteFriendRequest(FriendRequestDto friendRequestDto);
        Task<ICollection<User>> GetAllExploreUsers(int userId);
    }
}