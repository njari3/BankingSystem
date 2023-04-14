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
            _accountService = accountService;
            _mapper = mapper;
            _userService = userService;

        }

        [HttpGet("GetAccount/{userId}/{accountId}", Name = "GetAccount")]
        public async Task<ActionResult<Account>> GetAccount(int userId, int accountId)
        {
            var user = await _userService.GetUser(userId);

            if (user == null)
                return NotFound();

            var account = await _accountService.GetAccount(accountId);

            if (account == null)
                return NotFound();

            var result = _mapper.Map<AccountReadDto>(account);
            return Ok(result);
        }

        [HttpGet("GetAccountList/{userId}")]
        public async Task<ActionResult<List<Account>>> GetAccountList(int userId)
        {
            var accountList = await _accountService.GetAccountListByUserId(userId);

            return Ok(_mapper.Map<IEnumerable<AccountReadDto>>(accountList));
        }

        [HttpPost("CreateAccount/{id}")]
        public async Task<ActionResult<Account>> CreateAccount(int id)
        {
            var user = await _userService.GetUser(id);

            if (user == null)
                return NotFound();

            var account = _accountService.CreateAccount(user);

            await _accountService.SaveChange();
            var result = _mapper.Map<AccountReadDto>(account);

            return CreatedAtRoute(nameof(GetAccount), new { UserId = id, accountId = account.Id }, result);
        }


        [HttpDelete("DeleteAccount/{accountId}")]
        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            var account = await _accountService.GetAccount(accountId);

            if (account == null)
                return NotFound("Account not found!");

            try
            {
                _accountService.DeleteAccount(account);
                await _accountService.SaveChange();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

    }
}
