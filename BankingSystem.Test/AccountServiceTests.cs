using BankingSystem.Data;
using BankingSystem.Models;
using BankingSystem.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Test
{
    internal class AccountServiceTests
    {
        private AccountService _accountService;
        private Mock<IRepository<Account>> _accountRepository;

        [SetUp]
        public void Setup()
        {
            _accountRepository = new Mock<IRepository<Account>>();
            _accountService = new AccountService(_accountRepository.Object);
        }

        [Test]
        public void CreateAccount_ValidUser_CreatesNewAccount()
        {
            var user = new User { Accounts = new List<Account>() };

            var newAccount = _accountService.CreateAccount(user);

            Assert.That(newAccount, Is.Not.Null);
            Assert.That(newAccount.Balance, Is.EqualTo(0));
            Assert.That(user.Accounts.Contains(newAccount), Is.True);
        }

        [Test]
        public void CreateAccount_NullUser_ThrowsArgumentNullException()
        {
            User user = null;

            Assert.Throws<ArgumentNullException>(() => _accountService.CreateAccount(user));
        }


        [Test]
        public void DeleteAccount_ZeroBalance_RemovesAccount()
        {
            var account = new Account { Balance = 0 };
            _accountService.DeleteAccount(account);
            _accountRepository.Verify(r => r.Remove(account), Times.Once);
        }

        [Test]
        public void DeleteAccount_NonZeroBalance_ThrowsInvalidOperationException()
        {
            var account = new Account { Balance = 100 };

            Assert.Throws<InvalidOperationException>(() => _accountService.DeleteAccount(account));
            _accountRepository.Verify(r => r.Remove(It.IsAny<Account>()), Times.Never);
        }
    }
}
