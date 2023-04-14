using BankingSystem.Models;

namespace BankingSystem.Services
{
    public interface IUserService
    {
        Task<User> GetUser(int id);
    }
}
