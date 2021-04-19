using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Hubs;
using FootballApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FootballApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _chat;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ChatController(IHubContext<ChatHub> chat, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _chat = chat;
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomId)
        {
            await _chat.Groups.AddToGroupAsync(connectionId, roomId);
            return Ok();
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> JoinRoom(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var response = await _unitOfWork.Chats.JoinRoom(id, userId);

            if (response.Key)
            {
                await _unitOfWork.Complete();
            }

            return Ok(response);
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomId)
        {
            await _chat.Groups.RemoveFromGroupAsync(connectionId, roomId);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Chat(int id)
        {
            var chat = await _unitOfWork.Chats.GetChatWithMessages(id);

            return Ok(_mapper.Map<ChatToReturnDto>(chat));
        }

        [HttpGet("get-private-chats/{userId}")]
        public async Task<IActionResult> GetPrivateChats(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var chatsToReturn = _mapper.Map<IEnumerable<ChatToReturnDto>>(await _unitOfWork.Chats.GetPrivateChats(userId));

            return Ok(chatsToReturn);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(MessageToSendDto message)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var Message = new Message
                {
                    ChatId = message.ChatId,
                    Content = message.Content,
                    MessageSent = message.MessageSent,
                    SenderId = userId
                };

                _unitOfWork.Messages.Add(Message);

                await _unitOfWork.Complete();

                var userFromRepo = await _unitOfWork.Users.GetUserByIdWithAdditionalInformation(userId);

                await _chat.Clients.Group(message.ChatId.ToString())
                    .SendAsync("ReceiveMessage", new
                    {
                        Content = Message.Content,
                        Sender = _mapper.Map<UserToReturnMiniDto>(userFromRepo),
                        MessageSent = Message.MessageSent
                    });

                return Ok(new KeyValuePair<bool, string>(true, "Message sent successfully!"));
            }
            catch (Exception)
            {
                return Ok(new KeyValuePair<bool, string>(false, "Message could not be sent!"));
            }

        }

        [HttpPost("create-group-chat/{name}")]
        public async Task<IActionResult> CreateGroupChat(string name)
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var response = await _unitOfWork.Chats.CreateGroupChat(name, ownerId);

            if (response.Key)
            {
                await _unitOfWork.Complete();
            }

            return Ok(response);
        }

        [HttpPost("create-private-chat/{userId}")]
        public async Task<IActionResult> CreatePrivateChat(int userId)
        {
            var response = await _unitOfWork.Chats.CreatePrivateChat(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), userId);

            if (response.Key)
            {
                await _unitOfWork.Complete();
            }

            return Ok(response);
        }

        [HttpGet("available-users")]
        public async Task<IActionResult> GetAvailableUsers()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(_mapper.Map<IEnumerable<UserToReturnMiniDto>>(await _unitOfWork.Chats
                                                                                     .GetAvailableUsers(userId)));
        }
    }
}