using BankingSystem.Data;
using BankingSystem.Enums;
using BankingSystem.Models;
using BankingSystem.Services;
using Moq;

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

        [TestCase(1000, 500, 500)]
        public void Withdraw_ValidAmount_UpdatesAccountBalance(decimal balance, decimal amount, decimal result)
        {
            var account = new Account { Balance = balance };

            _accountService.MoneyTransfer(account, amount, MoneyTransfer.Withdraw);

            Assert.That(account.Balance, Is.EqualTo(result));

            _accountRepository.Verify(r => r.Update(account), Times.Once);
        }

        [TestCase(1000, -100)]
        [TestCase(1000, 0)]

        [TestCase(1000, 950)]
        [TestCase(1000, 901)]

        [TestCase(200, 150, TestName = "Withdraw_AccountBalanceBelowMinimum_ThrowsInvalidOperationException")]

        public void Withdraw_InvalidAmount_ThrowsInvalidOperationException(decimal balance, decimal amount)
        {
            var account = new Account { Balance = balance };

            Assert.Throws<InvalidOperationException>(() => _accountService.MoneyTransfer(account, amount, MoneyTransfer.Withdraw));
            _accountRepository.Verify(r => r.Update(It.IsAny<Account>()), Times.Never);
        }

        [TestCase(1000, 500, 1500)]
        public void Deposit_ValidAmount_UpdatesAccountBalance(decimal balance, decimal amount, decimal result)
        {
            var account = new Account { Balance = balance };

            _accountService.MoneyTransfer(account, amount, MoneyTransfer.Deposit);

            Assert.That(account.Balance, Is.EqualTo(result));
            _accountRepository.Verify(r => r.Update(account), Times.Once);
        }

        [TestCase(1000, -100)]
        [TestCase(1000, 0)]
        [TestCase(1000, 10001)]
        public void Deposit_InvalidAmount_ThrowsInvalidOperationException(decimal balance, decimal amount)
        {
            var account = new Account { Balance = balance };

            Assert.Throws<InvalidOperationException>(() => _accountService.MoneyTransfer(account, amount, MoneyTransfer.Deposit));
            _accountRepository.Verify(r => r.Update(It.IsAny<Account>()), Times.Never);
        }
    }
}
