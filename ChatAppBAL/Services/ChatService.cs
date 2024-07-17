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
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }
        public async Task<int> AddChatAsync(UserModel creator, ChatModel model)
        {
            //TODO:Validating chat name
            Chat chat = new Chat
            {
                CreatorId = creator.Id,
                ChatName = model.ChatName.Trim(),
                Users = new List<User>()
            };
            await _chatRepository.AddAsync(chat);
            return chat.Id;
        }

        public async Task<IEnumerable<int>> GetChatMembersIdsAsync(int chatId)
        {
            Chat chat = await _chatRepository.GetByIdAsync(chatId);
            return chat.Users.Select(u => u.Id);
        }

        public async Task RemoveChatAsync(UserModel sender, ChatModel chatToDelete)
        {
            Chat chat = await _chatRepository.GetByIdAsync(chatToDelete.Id);
            if (chat.CreatorId != sender.Id)
                throw new InvalidOperationException("You are not allowed to delete this chat");
            await _chatRepository.DeleteByIdAsync(chatToDelete.Id);

        }
    }
}
