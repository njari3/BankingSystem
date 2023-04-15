using AutoMapper;
using BankingSystem.Dtos;
using BankingSystem.Models;
using BankingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountsController(
            IAccountService accountService,
            IUserService userService,
            IMapper mapper
            )
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("GetAccount/{accountId}", Name = "GetAccount")]
        public async Task<ActionResult<Account>> GetAccount(int accountId)
        {
            if (accountId <= 0)
                return BadRequest("Account ID must be greater than zero.");

            var account = await _accountService.GetAccountAsync(accountId);

            if (account == null)
                return NotFound();

            var result = _mapper.Map<AccountReadDto>(account);
            return Ok(result);
        }

        [HttpGet("GetAccountList/{userId}")]
        public async Task<ActionResult<List<Account>>> GetAccountList(int userId)
        {
            if (userId <= 0)
                return BadRequest("User ID must be greater than zero.");

            var accountList = await _accountService.GetAccountsByUserIdAsync(userId);

            return Ok(_mapper.Map<IEnumerable<AccountReadDto>>(accountList));
        }

        [HttpPost("CreateAccount/{userId}")]
        public async Task<ActionResult<Account>> CreateAccount(int userId)
        {
            if (userId <= 0)
                return BadRequest("User ID must be greater than zero.");

            var user = await _userService.GetUser(userId);

            if (user == null)
                return NotFound();

            var account = _accountService.CreateAccount(user);

            await _accountService.SaveChangesAsync();
            var result = _mapper.Map<AccountReadDto>(account);

            return CreatedAtRoute(nameof(GetAccount), new { UserId = userId, accountId = account.Id }, result);
        }


        [HttpDelete("DeleteAccount/{accountId}")]
        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            if (accountId <= 0)
                return BadRequest("Account ID must be greater than zero.");

            var account = await _accountService.GetAccountAsync(accountId);

            if (account == null)
                return NotFound();

            try
            {
                _accountService.DeleteAccount(account);
                await _accountService.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost("MoneyTransfer")]
        public async Task<ActionResult> MoneyTransfer(AccountMoneyTransferDto accountDeposit)
        {
            var user = await _userService.GetUser(accountDeposit.UserId);

            if (user == null)
                return NotFound("User not found!");

            var account = await _accountService.GetAccountAsync(accountDeposit.AccountId);

            if (account == null)
                return NotFound("Account not found!");

            if (user.Id != account.User.Id)
                return BadRequest("The user id and the account id are not the same");

            try
            {
                _accountService.TransferMoney(account, accountDeposit);
                await _accountService.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(_mapper.Map<AccountReadDto>(account));
        }
    }
}
