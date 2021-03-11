using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FriendsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllFriendsForUser(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _unitOfWork.Friends.GetAllFriendsForUser(userId));
        }

        [HttpGet("pending-requests/{userId}")]
        public async Task<IActionResult> PendingFriendRequests(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _unitOfWork.Friends.PendingFriendRequests(userId));
        }

        [HttpPost("send-request")]
        public async Task<IActionResult> SendFriendRequest(FriendRequestDto friendRequestDto)
        {
            if (friendRequestDto.SenderId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var response = await _unitOfWork.Friends.SendFriendRequest(friendRequestDto);
            if (response.Key)
            {
                await _unitOfWork.Complete();
            }

            return Ok(response);
        }

        [HttpPost("accept-request")]
        public async Task<IActionResult> AcceptFriendRequest(FriendRequestDto friendRequestDto)
        {
            if (friendRequestDto.ReceiverId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var response = await _unitOfWork.Friends.AcceptFriendRequest(friendRequestDto);
            if (response.Key)
            {
                await _unitOfWork.Complete();
            }

            return Ok(response);
        }

        [HttpDelete("delete-request")]
        public async Task<IActionResult> DeleteFriendRequest(FriendRequestDto friendRequestDto)
        {
            if (friendRequestDto.SenderId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
               && friendRequestDto.ReceiverId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var response = await _unitOfWork.Friends.DeleteFriendRequest(friendRequestDto);
            if (response.Key)
            {
                await _unitOfWork.Complete();
            }

            return Ok(response);
        }

        [HttpGet("explore/{userId}")]
        public async Task<IActionResult> GetAllExploreUsers(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var users = await _unitOfWork.Friends.GetAllExploreUsers(userId);

            var usersToReturn = _mapper.Map<ICollection<ExploreUserDto>>(users);
            return Ok(usersToReturn);
        }
    }
}