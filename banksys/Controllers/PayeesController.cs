using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using banksys.Repositories;
using banksys.DTO;
using banksys.Interfaces;

namespace banksys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayeesController : ControllerBase
    {
        private readonly IPayeeService _payeeService;

        public PayeesController(IPayeeService payeeService)
        {
            _payeeService = payeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PayeeDTO>>> GetPayees()
        {
            var payees = await _payeeService.GetAllPayeesAsync();
            return Ok(payees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PayeeDTO>> GetPayee(int id)
        {
            var payee = await _payeeService.GetPayeeByIdAsync(id);
            if (payee == null)
            {
                return NotFound();
            }
            return Ok(payee);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<PayeeDTO>>> GetPayeeByUserId(int id)
        {
            var payee = await _payeeService.GetPayeeByUserIdAsync(id);
            if (payee == null)
            {
                return NotFound();
            }
            return Ok(payee);
        }

        [HttpPost]
        public async Task<ActionResult<MessageResponse>> CreatePayee(PayeeDTO payeeDTO)
        {
            var response = await _payeeService.CreatePayeeAsync(payeeDTO);
            return Ok(new MessageResponse()
            {
                Message = response
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayee(int id, PayeeDTO payeeDTO)
        {
            if (id != payeeDTO.PayeeId)
            {
                return BadRequest();
            }

            var updated = await _payeeService.UpdatePayeeAsync(payeeDTO);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayee(int id)
        {
            var deleted = await _payeeService.DeletePayeeAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
