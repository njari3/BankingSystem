using BankingSystem.Models;

namespace BankingSystem.Services
{
    public interface IAccountService
    {
        Account CreateAccount(User user);
        Task<Account> GetAccount(int id);
        Task SaveChange();
    }
}