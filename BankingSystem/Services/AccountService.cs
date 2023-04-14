using BankingSystem.Data;
using BankingSystem.Dtos;
using BankingSystem.Enums;
using BankingSystem.Models;

namespace BankingSystem.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account> _accountRepository;
        const int minBalance = 100;
        const int maxDeposit = 10000;
        const decimal maxWitdraw = 0.9m;
        public AccountService(IRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<Account>> GetAccountListByUserId(int id)
        {
            var restul = await _accountRepository.FindAsync(c => c.User.Id == id);
            return restul;
        }

        public async Task<Account> GetAccount(int id)
        {
            var restul = await _accountRepository.GetById(id);
            return restul;
        }

        public Account CreateAccount(User user)
        {
            if (user == null)
                throw new ArgumentNullException();

            var account = new Account { Balance = 0 };
            user.Accounts.Add(account);
            return account;
        }

        public void DeleteAccount(Account account)
        {
            if (account.Balance > 0)
                throw new InvalidOperationException();

            _accountRepository.Remove(account);
        }

        public void MoneyTransfer(Account account, decimal Amount, MoneyTransfer moneyTransfer) =>
    MoneyTransfer(account, new AccountMoneyTransferDto { Amount = Amount, MoneyTransfer = moneyTransfer });
        public void MoneyTransfer(Account account, AccountMoneyTransferDto accountDeposit)
        {
            switch (accountDeposit.MoneyTransfer)
            {
                case Enums.MoneyTransfer.Deposit:
                    Deposit(account, accountDeposit.Amount);
                    break;
                case Enums.MoneyTransfer.Withdraw:
                    Withdraw(account, accountDeposit.Amount);
                    break;
                default:
                    throw new InvalidOperationException();
                    ;
            }
        }

        private void Deposit(Account account, decimal amount)
        {
            if (amount <= 0 || amount > maxDeposit)
                throw new InvalidOperationException("Invalid deposit amount.");

            account.Balance += amount;

            _accountRepository.Update(account);
        }

        private void Withdraw(Account account, decimal amount)
        {
            if (amount <= 0 || account.Balance - amount < minBalance || amount > account.Balance * maxWitdraw)
                throw new InvalidOperationException("Invalid withdrawal amount.");

            account.Balance -= amount;

            _accountRepository.Update(account);
        }

        public async Task SaveChange()
        {
            await _accountRepository.SaveChange();
        }
    }
}
