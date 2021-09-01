using System.Security.Claims;
using System.Threading.Tasks;
using FootballApp.API.Dtos;
using FootballApp.API.Services.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;
        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }


        /// <summary>
        /// Method is used to fetch all comments for user
        /// </summary>
        /// <param name="userId">Id of user which comments are being retrevied</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCommentsForUser(int userId)
        {
            return Ok(await _commentsService.GetAllCommentsForUser(userId));
        }

        [HttpPost]
        public async Task<IActionResult> PostCommentForUser(int userId, CommentForCreationDto commentForCreationDto)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            return Ok(await _commentsService.PostCommentForUser(userId, commentForCreationDto));
        }
    }
}