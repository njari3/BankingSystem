using BankingSystem.Data;
using BankingSystem.Dtos;
using BankingSystem.Enums;
using BankingSystem.Models;

namespace BankingSystem.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account> _accountRepository;
        private const int MinBalance = 100;
        private const int MaxDeposit = 10000;
        private const decimal MaxWithdraw = 0.9m;

        public AccountService(IRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public Task<IEnumerable<Account>> GetAccountsByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId), "User ID must be greater than zero.");

            return _accountRepository.FindAsync(c => c.User.Id == userId);
        }

        public Task<Account> GetAccountAsync(int accountId)
        {
            if (accountId <= 0)
                throw new ArgumentOutOfRangeException(nameof(accountId), "Account ID must be greater than zero.");

            return _accountRepository.GetById(accountId);
        }

        public Account CreateAccount(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var account = new Account { Balance = 0, User = user };
            user.Accounts.Add(account);
            return account;
        }

        public void DeleteAccount(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            if (account.Balance > 0)
                throw new InvalidOperationException("Account balance must be zero before deletion.");

            _accountRepository.Remove(account);
        }

        public void TransferMoney(Account account, decimal amount, MoneyTransfer moneyTransfer) =>
            TransferMoney(account, new AccountMoneyTransferDto { Amount = amount, MoneyTransfer = moneyTransfer });

        public void TransferMoney(Account account, AccountMoneyTransferDto accountMoneyTransfer)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            if (accountMoneyTransfer == null)
                throw new ArgumentNullException(nameof(accountMoneyTransfer));

            switch (accountMoneyTransfer.MoneyTransfer)
            {
                case Enums.MoneyTransfer.Deposit:
                    Deposit(account, accountMoneyTransfer.Amount);
                    break;
                case Enums.MoneyTransfer.Withdraw:
                    Withdraw(account, accountMoneyTransfer.Amount);
                    break;
                default:
                    throw new InvalidOperationException("Invalid money transfer operation.");
            }
        }

        private void Deposit(Account account, decimal amount)
        {
            if (amount <= 0 || amount > MaxDeposit)
                throw new ArgumentOutOfRangeException(nameof(amount), "Invalid deposit amount.");

            account.Balance += amount;

            _accountRepository.Update(account);
        }

        private void Withdraw(Account account, decimal amount)
        {
            if (amount <= 0 || account.Balance - amount < MinBalance || amount > account.Balance * MaxWithdraw)
                throw new ArgumentOutOfRangeException(nameof(amount), "Invalid withdrawal amount.");

            account.Balance -= amount;

            _accountRepository.Update(account);
        }

        public async Task SaveChangesAsync()
        {
            await _accountRepository.SaveChange();
        }
    }
}