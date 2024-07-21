using banksys.DTO;
using banksys.Interfaces;
using banksys.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace banksys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            if (userDTO == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid user data.");
            }

            var result = await _authService.RegisterUserAsync(userDTO);
            return Ok(result);
        }

        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AdminDTO adminDTO)
        {
            if (adminDTO == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid user data.");
            }

            var result = await _authService.RegisterAdminAsync(adminDTO);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid login data.");
            }

            try
            {
                var result = await _authService.LoginAsync(loginRequest.UserNameOrEmail, loginRequest.Password);
                return Ok(new { Token = result });
            }
            catch (AccountLockedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (InvalidLoginException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetRequest resetRequest)
        {
            if (resetRequest == null || !ModelState.IsValid)
            {
                return BadRequest(new MessageResponse()
                {
                    Message = "Invalid password reset request."
                });
            }

            var result = await _authService.ResetPasswordAsync(resetRequest);

            if (result)
            {
                return Ok(new MessageResponse()
                {
                    Message = "Password has been reset successfully."
                });
            }

            return NotFound(new MessageResponse()
            {
                Message = "User not found."
            });
        }

        [HttpPost("forgot-username")]
        public async Task<IActionResult> ForgotUsername([FromBody] ForgetUsernameRequest forgetRequest)
        {
            if (forgetRequest == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid forget username request.");
            }

            var result = await _authService.ForgotUsernameAsync(forgetRequest);

            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordRequest forgetRequest)
        {
            if (forgetRequest == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid forget password request.");
            }

            var result = await _authService.ForgotPasswordAsync(forgetRequest);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound(result);
        }
    }
}
