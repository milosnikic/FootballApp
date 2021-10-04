using FootballApp.API.Dtos;
using FootballApp.API.Services.Friends;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FootballApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendsService _friendsService;
        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllFriendsForUser(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _friendsService.GetAllFriendsForUser(userId));
        }

        [HttpGet("pending-requests/{userId}")]
        public async Task<IActionResult> PendingFriendRequests(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _friendsService.PendingFriendRequests(userId));
        }

        [HttpGet("sent-requests/{userId}")]
        public async Task<IActionResult> SentFriendRequests(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _friendsService.SentFriendRequests(userId));
        }

        [HttpPost("send-request")]
        public async Task<IActionResult> SendFriendRequest(FriendRequestDto friendRequestDto)
        {
            if (friendRequestDto.SenderId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _friendsService.SendFriendRequest(friendRequestDto));
        }

        [HttpPost("accept-request")]
        public async Task<IActionResult> AcceptFriendRequest(FriendRequestDto friendRequestDto)
        {
            if (friendRequestDto.ReceiverId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _friendsService.AcceptFriendRequest(friendRequestDto));
        }

        [HttpDelete("delete-request")]
        public async Task<IActionResult> DeleteFriendRequest(FriendRequestDto friendRequestDto)
        {
            if (friendRequestDto.SenderId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
               && friendRequestDto.ReceiverId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _friendsService.DeleteFriendRequest(friendRequestDto));
        }

        [HttpGet("explore/{userId}")]
        public async Task<IActionResult> GetAllExploreUsers(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _friendsService.GetAllExploreUsers(userId));
        }
    }
}