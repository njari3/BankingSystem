using BankingSystem.Data;
using BankingSystem.Dtos;
using BankingSystem.Enums;
using BankingSystem.Models;
using BankingSystem.Services;
using Moq;

namespace BankingSystem.Test.Services.Test
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
        [Category("CreateAccount")]
        public void CreateAccount_ValidUser_CreatesNewAccount()
        {
            var user = new User();

            var newAccount = _accountService.CreateAccount(user);

            Assert.That(newAccount.Balance, Is.EqualTo(0));
            Assert.That(newAccount, Is.Not.Null);
            Assert.That(user.Accounts.Contains(newAccount), Is.True);
        }

        [Test]
        [Category("CreateAccount")]
        public void CreateAccount_NullUser_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _accountService.CreateAccount(null));
        }

        [Test]
        [Category("CreateAccount")]
        public void CreateAccount_ValidUser_ReturnedAccountHasUserAsOwner()
        {
            var user = new User { Id = 1, Name = "Test 1" };
            var account = _accountService.CreateAccount(user);

            Assert.That(account.User, Is.EqualTo(user));
        }

        [Test]
        [Category("DeleteAccount")]
        public void DeleteAccount_ZeroBalance_RemovesAccount()
        {
            var account = new Account { Balance = 0 };
            _accountService.DeleteAccount(account);
            _accountRepository.Verify(r => r.Remove(account), Times.Once);
        }

        [Test]
        [Category("DeleteAccount")]
        public void DeleteAccount_NonZeroBalance_ThrowsInvalidOperationException()
        {
            var account = new Account { Balance = 100 };

            Assert.Throws<InvalidOperationException>(() => _accountService.DeleteAccount(account));
            _accountRepository.Verify(r => r.Remove(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        [Category("DeleteAccount")]
        public void DeleteAccount_AccountIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _accountService.DeleteAccount(null));
        }

        [Test]
        [Category("TransferMoney")]
        public void TransferMoney_AccountIsNull_ThrowsArgumentNullException()
        {
            var moneyTransferDto = new AccountMoneyTransferDto { Amount = 100, MoneyTransfer = MoneyTransfer.Deposit };
            Assert.Throws<ArgumentNullException>(() => _accountService.TransferMoney(null, moneyTransferDto));
        }

        [Test]
        [Category("TransferMoney")]
        public void TransferMoney_AccountMoneyTransferDtoIsNull_ThrowsArgumentNullException()
        {
            var account = new Account { Id = 1, Balance = 100 };
            Assert.Throws<ArgumentNullException>(() => _accountService.TransferMoney(account, null));
        }

        [TestCase(1000, 500, 500)]
        [Category("Withdraw")]

        public void TransferMoney_WithdrawValidAmount_UpdatesAccountBalance(decimal balance, decimal amount, decimal result)
        {
            var account = new Account { Balance = balance };

            _accountService.TransferMoney(account, amount, MoneyTransfer.Withdraw);

            Assert.That(account.Balance, Is.EqualTo(result));

            _accountRepository.Verify(r => r.Update(account), Times.Once);
        }

        [TestCase(1000, -100)]
        [TestCase(1000, 0)]

        [TestCase(1000, 950)]
        [TestCase(1000, 901)]

        [TestCase(200, 150, TestName = "TransferMoney_AccountBalanceBelowMinimum_ThrowsInvalidOperationException")]
        [Category("Withdraw")]
        public void TransferMoney_WithdrawInvalidAmount_ThrowsInvalidOperationException(decimal balance, decimal amount)
        {
            var account = new Account { Balance = balance };

            Assert.Throws<ArgumentOutOfRangeException>(() => _accountService.TransferMoney(account, amount, MoneyTransfer.Withdraw));
            _accountRepository.Verify(r => r.Update(It.IsAny<Account>()), Times.Never);
        }

        [TestCase(1000, 500, 1500)]
        [Category("Deposit")]
        public void TransferMoney_DepositValidAmount_UpdatesAccountBalance(decimal balance, decimal amount, decimal result)
        {
            var account = new Account { Balance = balance };

            _accountService.TransferMoney(account, amount, MoneyTransfer.Deposit);

            Assert.That(account.Balance, Is.EqualTo(result));
            _accountRepository.Verify(r => r.Update(account), Times.Once);
        }

        [TestCase(1000, -100)]
        [TestCase(1000, 0)]
        [TestCase(1000, 10001)]
        [Category("Deposit")]
        public void TransferMoney_DepositInvalidAmount_ThrowsInvalidOperationException(decimal balance, decimal amount)
        {
            var account = new Account { Balance = balance };

            Assert.Throws<ArgumentOutOfRangeException>(() => _accountService.TransferMoney(account, amount, MoneyTransfer.Deposit));
            _accountRepository.Verify(r => r.Update(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        [Category("GetAccountsByUserIdAsync")]
        public void GetAccountsByUserIdAsync_InvalidUserId_ThrowsArgumentOutOfRangeException()
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _accountService.GetAccountsByUserIdAsync(-1));
        }

        [Test]
        [Category("GetAccountAsync")]
        public void GetAccountAsync_InvalidAccountId_ThrowsArgumentOutOfRangeException()
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _accountService.GetAccountAsync(-1));
        }

    }
}
