using DLARS.Data;
using Microsoft.EntityFrameworkCore;

namespace DLARS.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<int> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }

    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;


        public BaseRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }


        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }


        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }


        public async Task<int> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await _dbContext.SaveChangesAsync();
        }


        public async Task<bool> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
