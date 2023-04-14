using BankingSystem.Models;

namespace BankingSystem.Services
{
    public interface IAccountService
    {
        Account CreateAccount(User user);
        void DeleteAccount(Account account);
        Task<Account> GetAccount(int id);
        Task SaveChange();
    }
}