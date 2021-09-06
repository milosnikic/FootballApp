using System.Collections.Generic;
using System.Threading.Tasks;
using FootballApp.API.Dtos;

namespace FootballApp.API.Services.Friends
{
    public interface IFriendsService
    {
        Task<ICollection<ExploreUserDto>> GetAllFriendsForUser(int userId);
        Task<ICollection<ExploreUserDto>> PendingFriendRequests(int userId);
        Task<ICollection<ExploreUserDto>> SentFriendRequests(int userId);
        Task<KeyValuePair<bool, string>> SendFriendRequest(FriendRequestDto friendRequestDto);
        Task<KeyValuePair<bool, string>> AcceptFriendRequest(FriendRequestDto friendRequestDto);
        Task<KeyValuePair<bool, string>> DeleteFriendRequest(FriendRequestDto friendRequestDto);
        Task<ICollection<ExploreUserDto>> GetAllExploreUsers(int userId);
    }
}