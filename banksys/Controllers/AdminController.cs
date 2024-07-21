using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using banksys.Models;
using banksys.Repositories;
using Microsoft.AspNetCore.Authorization;
using banksys.Interfaces;
using banksys.DTO;

namespace banksys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("not-approved-accounts")]
        public async Task<ActionResult<IEnumerable<Account>>> GetNotApprovedAccounts()
        {
            var accounts = await _adminService.GetNotApprovedAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet("all-accounts")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAccounts()
        {
            var accounts = await _adminService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        [HttpPost("approve-account/{id}")]
        public async Task<IActionResult> ApproveAccount(int id)
        {
            var result = await _adminService.ApproveAccountAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("cancel-account/{id}")]
        public async Task<ActionResult<MessageResponse>> CancelAccount(int id)
        {
            try
            {
                var result = await _adminService.CancelAccountAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex) {

                return new MessageResponse()
                {
                    Message = ex.Message,
                };
            }

        }
    }
}
