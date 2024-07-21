using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using banksys.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace banksys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AdminController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPut("approve/{id}")]
        [Authorize]
        public async Task<IActionResult> ApproveAccount(int id)
        {
            var approved = await _accountService.ApproveAccountAsync(id);
            if (!approved)
            {
                return NotFound("NOT APPROVED!");
            }
            return Content("APPROVED!");
        }
    }
}
