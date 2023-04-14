using System.Linq.Expressions;

namespace BankingSystem.Data
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task AddAsync(T entity);
        Task SaveChange();
        void Remove(T entity);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}
