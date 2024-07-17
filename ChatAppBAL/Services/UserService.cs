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
    public class UserService: IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        public UserService(IUserRepository repository, IMapper mapper)
        {
            userRepository = repository;
            this.mapper = mapper;
        }

        public async Task<int> AddUserAsync(UserModel model)
        {
            User entity = mapper.Map<UserModel, User>(model);
            await userRepository.AddAsync(entity);
            return entity.Id;
        }

        public async Task DeleteUserAsync(int id)
        {
            if(!UserExist(id))
                throw new UserNotFoundExeption($"User with id {id} does not exist");
            await userRepository.DeleteByIdAsync(id);
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            User? user = await userRepository.GetByIdWithDetailsAsync(id);
            if(user == null)
                throw new UserNotFoundExeption($"User with id {id} does not exist");
            return mapper.Map<User, UserModel>(user);
        }

        public async Task UpdateUserAsync(UserModel model)
        {
            User user = mapper.Map<UserModel, User>(model);
            await userRepository.UpdateAsync(user);
        }

        public bool UserExist(int id)
        {
            return  userRepository.IsUserExist(id);
        }
    }
}
