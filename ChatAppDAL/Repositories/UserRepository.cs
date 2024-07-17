using ChatAppDAL.Entities;
using ChatAppDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppDAL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ChatAppDBContext context) : base(context)
        {
        }
        public bool IsUserExist(int id)
        {
            return _context.Users.Any(x => x.Id == id);
        }

        public async Task<User?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Users.Include(u => u.JoinedChats).Include(u => u.CreatedChats).FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<IEnumerable<User>> GetAllWithdetailsAsync()
        {
            return await _context.Users.Include(u => u.JoinedChats).Include(u => u.CreatedChats).ToListAsync();
        }
    }
}
