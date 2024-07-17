using ChatAppBAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppBAL.Interfaces
{
    public interface IChatService
    {
        public Task<int> AddChatAsync(UserModel creator, ChatModel model);
        public Task RemoveChatAsync(UserModel sender, ChatModel chatToDelete);
        public Task<IEnumerable<int>> GetChatMembersIdsAsync(int chatId);
    }
}
