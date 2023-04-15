using AutoMapper;
using BankingSystem.Controllers;
using BankingSystem.Dtos;
using BankingSystem.Models;
using BankingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BankingSystem.Test.Controllers.Test
{
    internal class AccountsControllerTest
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IAccountService> _accountServiceMock;
        private Mock<IMapper> _mapperMock;
        private AccountsController _controller;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _accountServiceMock = new Mock<IAccountService>();
            _mapperMock = new Mock<IMapper>();
           
            _controller = new AccountsController(
                _accountServiceMock.Object,
                _userServiceMock.Object,
                _mapperMock.Object
                );
        }

        [Test]
        [Category("GetAccount")]
        public async Task GetAccount_InvalidAccountId_ReturnsBadRequest()
        {
            var result = await _controller.GetAccount(-1);
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("GetAccount")]
        public async Task GetAccount_AccountNotFound_ReturnsNotFound()
        {
            _accountServiceMock.Setup(a => a.GetAccountAsync(It.IsAny<int>())).ReturnsAsync((Account)null);

            var result = await _controller.GetAccount(1);
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        [Category("GetAccount")]
        public async Task GetAccount_ValidAccount_ReturnsOk()
        {
            _accountServiceMock.Setup(a => a.GetAccountAsync(It.IsAny<int>())).ReturnsAsync(new Account());
            _mapperMock.Setup(m => m.Map<AccountReadDto>(It.IsAny<Account>())).Returns(new AccountReadDto());

            var result = await _controller.GetAccount(1);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        [Category("GetAccountList")]
        public async Task GetAccountList_InvalidUserId_ReturnsBadRequest()
        {
            var result = await _controller.GetAccountList(-1);
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("GetAccountList")]
        public async Task GetAccountList_ValidUserId_ReturnsOk()
        {
            _accountServiceMock.Setup(a => a.GetAccountsByUserIdAsync(It.IsAny<int>())).ReturnsAsync(new List<Account>());
            _mapperMock.Setup(m => m.Map<IEnumerable<AccountReadDto>>(It.IsAny<IEnumerable<Account>>())).Returns(new List<AccountReadDto>());

            var result = await _controller.GetAccountList(1);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        }

        [Test]
        [Category("CreateAccount")]
        public async Task CreateAccount_InvalidUserId_ReturnsBadRequest()
        {
            var result = await _controller.CreateAccount(-1);
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        [Category("CreateAccount")]
        public async Task CreateAccount_UserNotFound_ReturnsNotFound()
        {
            _userServiceMock.Setup(u => u.GetUser(It.IsAny<int>())).ReturnsAsync((User)null);

            var result = await _controller.CreateAccount(1);
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        [Category("CreateAccount")]
        public async Task CreateAccount_ValidUserId_ReturnsCreatedAtRoute()
        {
            var user = new User();
            _userServiceMock.Setup(u => u.GetUser(It.IsAny<int>())).ReturnsAsync(user);
            _accountServiceMock.Setup(a => a.CreateAccount(It.IsAny<User>())).Returns(new Account());
            _mapperMock.Setup(m => m.Map<AccountReadDto>(It.IsAny<Account>())).Returns(new AccountReadDto());

            var result = await _controller.CreateAccount(1);

            _accountServiceMock.Verify(a => a.CreateAccount(user), Times.Once);
            _accountServiceMock.Verify(a => a.SaveChangesAsync(), Times.Once);

            Assert.That(result.Result, Is.InstanceOf<CreatedAtRouteResult>());
        }
    }
}
