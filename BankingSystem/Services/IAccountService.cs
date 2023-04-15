using BankingSystem.Dtos;
using BankingSystem.Enums;
using BankingSystem.Models;

namespace BankingSystem.Services
{
    public interface IAccountService
    {
        Account CreateAccount(User user);
        void DeleteAccount(Account account);
        Task<Account> GetAccountAsync(int accountId);
        Task<IEnumerable<Account>> GetAccountsByUserIdAsync(int userId);
        void TransferMoney(Account account, AccountMoneyTransferDto accountMoneyTransfer);
        void TransferMoney(Account account, decimal amount, MoneyTransfer moneyTransfer);
        Task SaveChangesAsync();
    }
}