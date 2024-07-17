using ChatAppDAL.Entities;
using ChatAppDAL.Interfaces;
using ChatAppDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppDAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatRepository chatRepository;
        public IChatRepository ChatRepository => chatRepository;

        private MessageRepository messageRepository;
        public IMessageRepository MessageRepository => messageRepository;

        private readonly UserRepository userRepository;
        public IUserRepository UserRepository => userRepository;
        private ChatAppDBContext context;


        public UnitOfWork(ChatAppDBContext context)
        {
            this.context = context;
            chatRepository = new ChatRepository(context);
            messageRepository = new MessageRepository(context);
            userRepository = new UserRepository(context);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
