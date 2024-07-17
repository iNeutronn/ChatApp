using ChatAppBAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppBAL.Interfaces
{
    public interface IUserService
    {
        public Task<int> AddUserAsync(UserModel model);
        public Task UpdateUserAsync(UserModel model);
        public Task DeleteUserAsync(int id);
        public bool UserExist(int id);
        public Task<UserModel> GetUserByIdAsync(int id);
    }
}
