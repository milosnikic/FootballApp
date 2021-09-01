using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Hubs;
using FootballApp.API.Models;
using FootballApp.API.Services.Chat;
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
        private readonly IChatsService _chatsService;
        public ChatController(IChatsService chatsService)
        {
            _chatsService = chatsService;
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomId)
        {
            await _chatsService.JoinRoom(connectionId, roomId);
            return Ok();
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> JoinRoom(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(await _chatsService.JoinRoom(id, userId));
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomId)
        {
            await _chatsService.LeaveRoom(connectionId, roomId);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Chat(int id)
        {
            return Ok(await _chatsService.Chat(id));
        }

        [HttpGet("get-private-chats/{userId}")]
        public async Task<IActionResult> GetPrivateChats(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            
            return Ok(await _chatsService.GetPrivateChats(userId));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(MessageToSendDto message)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(await _chatsService.SendMessage(message, userId));
        }

        [HttpPost("create-group-chat/{name}")]
        public async Task<IActionResult> CreateGroupChat(string name)
        {
            var ownerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            return Ok(await _chatsService.CreateGroupChat(name, ownerId));
        }

        [HttpPost("create-private-chat/{userId}")]
        public async Task<IActionResult> CreatePrivateChat(int userId)
        {
            int loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(await _chatsService.CreatePrivateChat(userId, loggedInUserId));
        }

        [HttpGet("available-users")]
        public async Task<IActionResult> GetAvailableUsers()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(await _chatsService.GetAvailableUsers(userId));
        }
    }
}