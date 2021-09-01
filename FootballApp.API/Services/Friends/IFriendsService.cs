using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Dtos;

namespace FootballApp.API.Services.Friends
{
    public interface IFriendsService
    {
        Task<IEnumerable<ExploreUserDto>> GetAllFriendsForUser(int userId);
        Task<IEnumerable<ExploreUserDto>> PendingFriendRequests(int userId);
        Task<IEnumerable<ExploreUserDto>> SentFriendRequests(int userId);
        Task<KeyValuePair<bool, string>> SendFriendRequest(FriendRequestDto friendRequestDto);
        Task<KeyValuePair<bool, string>> AcceptFriendRequest(FriendRequestDto friendRequestDto);
        Task<KeyValuePair<bool, string>> DeleteFriendRequest(FriendRequestDto friendRequestDto);
        Task<IEnumerable<ExploreUserDto>> GetAllExploreUsers(int userId);
    }
}