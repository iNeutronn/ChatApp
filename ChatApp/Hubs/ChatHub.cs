using Microsoft.AspNetCore.SignalR;
using ChatAppBAL.Models;
using ChatAppBAL.Interfaces;

namespace ChatAppPL.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessagingService messegingService;
        private readonly IChatService chatService;
        private readonly IUserService userService;

        public ChatHub(IMessagingService messegingService, IChatService chatService, IUserService userService)
        {
            this.messegingService = messegingService;
            this.chatService = chatService;
            this.userService = userService;
        }
        public void SendMessage(MessageModel message)
        {
            messegingService.SendMessage(message);
            this.Clients.Group(message.ChatId.ToString()).SendAsync("ReceiveMessage", message);
        }
        public int CreateChat(UserModel creator, ChatModel newChat)
        {
            int id = chatService.AddChatAsync(creator, newChat).Result;
            Groups.AddToGroupAsync(this.Context.ConnectionId, newChat.Id.ToString()!);
            this.Clients.Caller.SendAsync("ChatCreationSucces");
            this.Clients.Others.SendAsync("NewChatCreated");
            return id;
        }
        public void DeleteChat(UserModel sender, ChatModel chatToDelete)
        {
            chatService.RemoveChatAsync(sender, chatToDelete);
            this.Clients.Caller.SendAsync("ChatDeletionSucces");
            this.Clients.Others.SendAsync("NewChatDeletion");
        }
        public void ConnectChat(UserModel user, ChatModel chat)
        {
            messegingService.JoinChatAsync(user, chat);
            this.Clients.OthersInGroup(chat.Id.ToString()).SendAsync("UserJoined", user, chat);
            Groups.AddToGroupAsync(this.Context.ConnectionId, chat.Id.ToString());
            this.Clients.Caller.SendAsync("ChatConnectionSucces");
        }
        public void LeaveChat(UserModel user, ChatModel chat)
        {
            messegingService.LeaveChat(user, chat);
            Groups.RemoveFromGroupAsync(this.Context.ConnectionId, chat.Id.ToString());
            this.Clients.Caller.SendAsync("ChatLeavingSucces");
            this.Clients.OthersInGroup(chat.Id.ToString()).SendAsync("UserLeft", user, chat);
        }
        public void SearchForChat(SearchChatModel searchChatModel)
        {

        }
        public async Task<int> RegisterUser(UserModel user)
        {
            int id;
            if (userService.UserExist(user.Id))
            {
                user = await userService.GetUserByIdAsync(user.Id);
                foreach (var chatId in user.JoinedChatsIds)
                {
                    await Groups.AddToGroupAsync(this.Context.ConnectionId, chatId.ToString());
                }
                id = user.Id;
            }
            else
            {
                id = userService.AddUserAsync(user).Result;
            }

            await this.Clients.Caller.SendAsync("UserRegistrationSucces");
            return id;
        }

        public void DeleteUser(UserModel user)
        {
            userService.DeleteUserAsync(user.Id);
        }

        public bool UserExist(UserModel user)
        {
            return userService.UserExist(user.Id);
        }

        public async override Task OnConnectedAsync()
        {
            
        }
    }
}
