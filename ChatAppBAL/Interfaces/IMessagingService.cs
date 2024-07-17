using ChatAppBAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppBAL.Interfaces
{
    public interface IMessagingService
    {
        Task JoinChatAsync(UserModel user, ChatModel chat);
        public Task LeaveChat(UserModel user, ChatModel chat);
        public void SendMessage(MessageModel model);
    }
}
