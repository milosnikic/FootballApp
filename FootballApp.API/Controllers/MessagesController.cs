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
    public class MessagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MessagesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // [HttpPost("send-message")]
        // public async Task<IActionResult> SendMessage(MessageToSendDto messageToSendDto)
        // {
        //     if (messageToSendDto.SenderId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        //         return Unauthorized();

        //     var messageToAdd = _mapper.Map<Message>(messageToSendDto);
        //     var response = await _unitOfWork.Messages.SendMessage(messageToAdd);
        //     if (response.Key) 
        //     {
        //         await _unitOfWork.Complete();
        //     }

        //     return Ok(response);
        // }

        [HttpPost("read-message")]
        public async Task<IActionResult> ReadMessage(int messageId, int chatId, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var response = await _unitOfWork.Messages.ReadMessage(messageId, chatId, userId);
            if (response.Key)
            {
                await _unitOfWork.Complete();
            }

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(int messageId, int chatId, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var response = await _unitOfWork.Messages.DeleteMessage(messageId, chatId, userId);
            if (response.Key)
            {
                await _unitOfWork.Complete();
            }

            return Ok(response);
        }

    }
}