using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using banksys.DTO;
using banksys.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace banksys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User, Admin")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTO>> GetAccountById(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult<MessageResponse>> CreateAccount([FromBody] AccountDTO accountDTO)
        {
            if (accountDTO == null)
            {
                return BadRequest(new MessageResponse { Message = "Invalid account data." });
            }

            try
            {
                var createdAccount = await _accountService.CreateAccountAsync(accountDTO);
                return CreatedAtAction(nameof(GetAccountById), new { id = createdAccount.AccountId }, new MessageResponse { Message = "Account created successfully." });
            }
            catch (InvalidOperationException ex)
            {
                ;

                return StatusCode(StatusCodes.Status500InternalServerError, new MessageResponse { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new MessageResponse { Message = "An unexpected error occurred while creating the account. Please try again later." });
            }
        }

        [HttpGet("user/{UserId}")]
        public async Task<ActionResult<AccountDTO>> GetAccountUserById(int UserId)
        {
            var account = await _accountService.GetAccountByUserIdAsync(UserId);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpGet("otp/{UserId}")]
        public async Task<ActionResult<MessageResponse>> RequestOTP(int UserId)
        {
            var account = await _accountService.SendOtpToUserAsync(UserId);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpPost("register")]
        public async Task<ActionResult<MessageResponse>> RegisterNetBanking([FromBody] NetBankingRequest request)
        {
            if (request == null)
            {
                return BadRequest(new MessageResponse { Message = "Invalid request" });
            }

            try
            {
                var response = await _accountService.RegisterNetBankingAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new MessageResponse { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] AccountDTO accountDTO)
        {
            if (accountDTO == null || accountDTO.AccountId != id)
            {
                return BadRequest();
            }

            var updated = await _accountService.UpdateAccountAsync(accountDTO);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("approve/{id}")]
        public async Task<IActionResult> ApproveAccount(int id)
        {
            var approved = await _accountService.ApproveAccountAsync(id);
            if (!approved)
            {
                return NotFound();
            }

            return NoContent();
        }

        //[HttpGet("statement/{accountId}")]
        //public async Task<ActionResult<AccountStatementDTO>> GetAccountStatement(int accountId)
        //{
        //    var statement = await _accountService.GetAccountStatementAsync(accountId);
        //    if (statement == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(statement);
        //}

        //[HttpPost("transaction")]
        //public async Task<IActionResult> PerformTransaction([FromBody] TransactionRequestDTO transactionRequest)
        //{
        //    if (transactionRequest == null)
        //    {
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        var success = await _accountService.PerformTransactionAsync(transactionRequest);
        //        if (!success)
        //        {
        //            return BadRequest("Transaction failed");
        //        }

        //        return Ok("Transaction completed successfully");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
