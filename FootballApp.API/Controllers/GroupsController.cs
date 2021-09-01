using System.Security.Claims;
using System.Threading.Tasks;
using FootballApp.API.Dtos;
using FootballApp.API.Services.Groups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupsService _groupsService;
        public GroupsController(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        [HttpGet("{id}", Name = "GetGroup")]
        public async Task<IActionResult> GetGroup(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var group = await _groupsService.GetGroup(id, userId);
            if (group != null)
                return Ok(group);

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupsForUser(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _groupsService.GetGroupsForUser(userId));
        }

        [HttpGet]
        [Route("favorite")]
        public async Task<IActionResult> GetFavoriteGroupsForUser(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _groupsService.GetFavoriteGroupsForUser(userId));
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(int userId, [FromForm] GroupForCreationDto group)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _groupsService.CreateGroup(userId, group));
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllGroups(int userId)
        {
            return Ok(await _groupsService.GetAllGroups(userId));
        }

        [HttpGet]
        [Route("created")]
        public async Task<IActionResult> GetAllCreatedGroups(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var groups = await _groupsService.GetAllCreatedGroups(userId);
            if (groups != null)
                return Ok(groups);
            
            return Unauthorized();
        }

        [HttpPost]
        [Route("leave/{groupId}")]
        public async Task<IActionResult> LeaveGroup(int groupId, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _groupsService.LeaveGroup(groupId, userId));
        }

        [HttpPost]
        [Route("request-join/{groupId}")]
        public async Task<IActionResult> RequestJoinGroup(int groupId, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _groupsService.RequestJoinGroup(groupId, userId));
        }

        [HttpPost]
        [Route("accept/{groupId}")]
        public async Task<IActionResult> AcceptUser(int groupId, int userId)
        {
            // We only check that we are logged in
            // TODO: add check if user is admin or owner in order to 
            //       accept or reject user
            return Ok(await _groupsService.AcceptUser(groupId, userId));
        }

        [HttpDelete]
        [Route("reject/{groupId}")]
        public async Task<IActionResult> RejectUser(int groupId, int userId)
        {   
            // We only check that we are logged in
            // TODO: add check if user is admin or owner in order to 
            //       accept or reject user
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _groupsService.RejectUser(groupId, userId));
        }

        /// <summary>
        /// Method is used to make group favorite or unfavorite 🙂
        /// </summary>
        /// <param name="groupId">Id of group to get favorite or unfavorite</param>
        /// <param name="userId">User id</param>
        /// <param name="like">Indicator if group should be favorite or unfavorite</param>
        /// <returns>Result of favoriting operation</returns>
        [HttpPost]
        [Route("favorite/{groupId}")]
        public async Task<IActionResult> MakeFavoriteGroup(int groupId, int userId, bool favorite = true)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _groupsService.MakeFavoriteGroup(groupId, userId, favorite));
        }


        [HttpGet]
        [Route("{groupId}/membership-info")]
        public async Task<IActionResult> GetMembershipInformation(int userId, int groupId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var membershipInformation = await _groupsService.GetMembershipInformation(userId, groupId);
            if (membershipInformation != null)
                return Ok(membershipInformation);

            return BadRequest();
        }
    }
}
