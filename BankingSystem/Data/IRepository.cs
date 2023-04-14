namespace BankingSystem.Data
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task AddAsync(T entity);
        Task SaveChange();
    }
}
