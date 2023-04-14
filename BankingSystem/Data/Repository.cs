using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BankingSystem.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
    
        public async Task<T> GetById(int id)
        {
            var result = await _dbSet.FindAsync(id);

            return result;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }
        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }
    }
}
