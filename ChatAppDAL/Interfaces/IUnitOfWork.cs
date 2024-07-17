using ChatAppDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppDAL.Interfaces
{
    public interface IUnitOfWork
    {
        IChatRepository ChatRepository { get; }
        IMessageRepository MessageRepository { get; }
        IUserRepository UserRepository { get; }

        Task SaveAsync();
    }
}
