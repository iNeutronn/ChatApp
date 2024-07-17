using ChatAppDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppDAL.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public bool IsUserExist(int id);
        public Task<IEnumerable<User>> GetAllWithdetailsAsync();
        public Task<User?> GetByIdWithDetailsAsync(int id);
    }
}
