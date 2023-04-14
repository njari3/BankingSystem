using BankingSystem.Dtos;
using BankingSystem.Enums;
using BankingSystem.Models;

namespace BankingSystem.Services
{
    public interface IAccountService
    {
        Account CreateAccount(User user);
        void DeleteAccount(Account account);
        Task<Account> GetAccount(int id);
        Task<IEnumerable<Account>> GetAccountListByUserId(int id);
        void MoneyTransfer(Account account, AccountMoneyTransferDto accountDeposit);
        void MoneyTransfer(Account account, decimal Amount, MoneyTransfer moneyTransfer);
        Task SaveChange();
    }
}