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

        public async Task SaveChange()
        {
            await _accountRepository.SaveChange();
        }
    }
}
