using ChatAppDAL.Entities;
using ChatAppDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppDAL.Repositories
{
    public abstract class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ChatAppDBContext _context;
        

        protected GenericRepository(ChatAppDBContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id) ?? throw new EntityNotFoundException($"Entity with id {id} not found");
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
             return Task.FromResult(_context.Set<T>().AsEnumerable());
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return (await _context.Set<T>().FindAsync(id)) ?? throw new EntityNotFoundException($"Entity with id {id} not found");
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.FromResult(_context.Set<T>().Update(entity));
            await _context.SaveChangesAsync();
        }
    }
}
