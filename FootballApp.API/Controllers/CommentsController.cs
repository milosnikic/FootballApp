using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FootballApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CommentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Method is used to fetch all comments for user
        /// </summary>
        /// <param name="userId">Id of user which comments are being retrevied</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCommentsForUser(int userId)
        {
            var comments = await _unitOfWork.Comments.GetAllCommentsForUser(userId);
            return Ok(_mapper.Map<ICollection<CommentToReturn>>(comments));
        }

        /// <summary>
        /// Method is used to fetch all comments for specified user that other
        /// users commented on
        /// </summary>
        /// <param name="userId">Id of user to get comments</param>
        /// <returns></returns>
        // [HttpGet]
        // public async Task<IActionResult> GetAllCommentedForUser(int userId)
        // {
        //     // var comments = await _unitOfWork.Comments.GetAllCommentedForUser
        //     return Ok();
        // }

        [HttpPost]
        public async Task<IActionResult> PostCommentForUser(int userId, CommentForCreationDto commentForCreationDto)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var comment = _mapper.Map<Comment>(commentForCreationDto);
            _unitOfWork.Comments.Add(comment);
            
            if(await _unitOfWork.Complete())
                return Ok(new KeyValuePair<bool, string>(true, "Comment added successfully!"));

            return Ok(new KeyValuePair<bool, string>(false, "Problem adding comment!"));
        }
    }
}