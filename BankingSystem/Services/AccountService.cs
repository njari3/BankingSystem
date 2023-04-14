using BankingSystem.Data;
using BankingSystem.Models;

namespace BankingSystem.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account> _accountRepository;
        public AccountService(IRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Account CreateAccount(User user)
        {
            if (user == null)
                throw new ArgumentNullException();

            var account = new Account { Balance = 0 };
            user.Accounts.Add(account);
            return account;
        }

        public async Task SaveChange()
        {
            await _accountRepository.SaveChange();
        }
    }
}
