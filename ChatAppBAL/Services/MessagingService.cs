using AutoMapper;
using ChatAppBAL.Exeptions;
using ChatAppBAL.Interfaces;
using ChatAppBAL.Models;
using ChatAppDAL.Entities;
using ChatAppDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppBAL.Services
{
    public class MessagingService : IMessagingService
    {
        private readonly IChatRepository chatRepository;
        private readonly IUserRepository userRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IMapper mapper;

        public async Task JoinChatAsync(UserModel user, ChatModel chat)
        {
            User userEntity = await userRepository.GetByIdAsync(user.Id) ?? throw new UserNotFoundExeption($"Threre is no user with id {user.Id}");
            Chat chatEntity = await chatRepository.GetByIdAsync(chat.Id) ?? throw new ChatNotFoundExeption($"Threre is no chat with id {chat.Id}");
            chatEntity.Users.Add(userEntity);
            await chatRepository.UpdateAsync(chatEntity);
        }

        public async Task LeaveChat(UserModel user, ChatModel chat)
        {
            User userEntity = await userRepository.GetByIdAsync(user.Id) ?? throw new UserNotFoundExeption($"Threre is no user with id {user.Id}");
            Chat chatEntity = await chatRepository.GetByIdAsync(chat.Id) ?? throw new ChatNotFoundExeption($"Threre is no chat with id {chat.Id}");
            if (!chatEntity.Users.Contains(userEntity))
            {
                throw new InvalidOperationException($"Threre is no user with id {user.Id} in chat with id {chat.Id}");
            }

            chatEntity.Users.Remove(userEntity);
            await chatRepository.UpdateAsync(chatEntity);
        }

        public void SendMessage(MessageModel model)
        {
            var messageEntity = mapper.Map<Message>(model);
            messageRepository.AddAsync(messageEntity);
        }
    }
}
