using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballApp.API.Services.Friends
{
    public class FriendsService : IFriendsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public FriendsService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<KeyValuePair<bool, string>> AcceptFriendRequest(FriendRequestDto friendRequestDto)
        {
            var response = await _unitOfWork.Friends.AcceptFriendRequest(friendRequestDto);
            if (response.Key)
            {
                await _unitOfWork.Complete();
            }

            return response;
        }

        public async Task<KeyValuePair<bool, string>> DeleteFriendRequest(FriendRequestDto friendRequestDto)
        {
            var response = await _unitOfWork.Friends.DeleteFriendRequest(friendRequestDto);
            if (response.Key)
            {
                await _unitOfWork.Complete();
            }

            return response;
        }
        public async Task<KeyValuePair<bool, string>> SendFriendRequest(FriendRequestDto friendRequestDto)
        {
            var response = await _unitOfWork.Friends.SendFriendRequest(friendRequestDto);
            if (response.Key)
            {
                await _unitOfWork.Complete();
            }

            return response;
        }

        public async Task<ICollection<ExploreUserDto>> GetAllExploreUsers(int userId)
        {
            var users = await _unitOfWork.Friends.GetAllExploreUsers(userId);

            return _mapper.Map<ICollection<ExploreUserDto>>(users);
        }

        public async Task<ICollection<ExploreUserDto>> GetAllFriendsForUser(int userId)
        {
            return _mapper.Map<ICollection<ExploreUserDto>>(await _unitOfWork.Friends.GetAllFriendsForUser(userId));
        }

        public async Task<ICollection<ExploreUserDto>> PendingFriendRequests(int userId)
        {
            return _mapper.Map<ICollection<ExploreUserDto>>(await _unitOfWork.Friends.PendingFriendRequests(userId));
        }


        public async Task<ICollection<ExploreUserDto>> SentFriendRequests(int userId)
        {
            return _mapper.Map<ICollection<ExploreUserDto>>(await _unitOfWork.Friends.SentFriendRequests(userId));
        }
    }
}