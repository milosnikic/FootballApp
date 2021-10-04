using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FootballApp.API.Data.UnitOfWork;
using FootballApp.API.Dtos;
using FootballApp.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using FootballApp.API.Models;
using System;

namespace FootballApp.API.Services.Chat
{
    public class ChatsService : IChatsService
    {
        private readonly IHubContext<ChatHub> _chat;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ChatsService(IHubContext<ChatHub> chat, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _chat = chat;
        }

        public async Task<ChatToReturnDto> Chat(int id)
        {
            var chat = await _unitOfWork.Chats.GetChatWithMessages(id);

            return _mapper.Map<ChatToReturnDto>(chat);
        }

        public async Task<KeyValuePair<bool, string>> CreateGroupChat(string name, int ownerId)
        {
            var response = await _unitOfWork.Chats.CreateGroupChat(name, ownerId);

            if (response.Key)
            {
                await _unitOfWork.Complete();
            }

            return response;
        }

        public async Task<KeyValuePair<bool, string>> CreatePrivateChat(int userId, int loggedInUserId)
        {
            var response = await _unitOfWork.Chats.CreatePrivateChat(loggedInUserId, userId);

            if (response.Key)
            {
                await _unitOfWork.Complete();
            }

            return response;
        }

        public async Task<ICollection<UserToReturnMiniDto>> GetAvailableUsers(int userId)
        {
            return _mapper.Map<ICollection<UserToReturnMiniDto>>(await _unitOfWork.Chats
                                                                                     .GetAvailableUsers(userId));
        }

        public async Task<ICollection<ChatToReturnDto>> GetPrivateChats(int userId)
        {
            return _mapper.Map<ICollection<ChatToReturnDto>>(await _unitOfWork.Chats.GetPrivateChats(userId));
        }

        public async Task JoinRoom(string connectionId, string roomId)
        {
            await _chat.Groups.AddToGroupAsync(connectionId, roomId);
        }

        public async Task<KeyValuePair<bool, string>> JoinRoom(int id, int userId)
        {
            var response = await _unitOfWork.Chats.JoinRoom(id, userId);

            if (response.Key)
            {
                await _unitOfWork.Complete();
            }

            return response;
        }

        public async Task LeaveRoom(string connectionId, string roomId)
        {
            await _chat.Groups.RemoveFromGroupAsync(connectionId, roomId);
        }

        public async Task<KeyValuePair<bool, string>> SendMessage(MessageToSendDto message, int userId)
        {
            try
            {
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

                return new KeyValuePair<bool, string>(true, "Message sent successfully!");
            }
            catch (Exception)
            {
                return new KeyValuePair<bool, string>(false, "Message could not be sent!");
            }
        }
    }
}