using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using banksys.Repositories;
using banksys.DTO;
using banksys.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;
using banksys.Models;
using Microsoft.AspNetCore.Authorization;

namespace banksys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User, Admin")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountservice;
        private readonly IConfiguration _configuration;

        public TransactionsController(ITransactionService transactionService, IAccountService accountService, IConfiguration configuration)
        {
            _transactionService = transactionService;
            _accountservice= accountService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetTransactions()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDTO>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpGet("user/{UserId}")]
        public async Task<ActionResult<AccountDTO>> GetTransactionstUserById(int UserId)
        {
            var account = await _transactionService.GetTransactionByUserIdAsync(UserId);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }




        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransactionDTO transactionRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _transactionService.CreateTransactionAsync(transactionRequest);
                if (result != null)
                {

                    return Ok(result);
                }
                return BadRequest("Transaction failed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
