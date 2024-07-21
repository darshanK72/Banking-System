using banksys.DTO;
using System.Threading.Tasks;

namespace banksys.Interfaces
{
    public interface IAuthService
    {
        Task<UserDTO> RegisterUserAsync(UserDTO userDTO);
        Task<AdminDTO> RegisterAdminAsync(AdminDTO userDTO);
        Task<string> LoginAsync(string usernameOrEmail, string password);
        Task<bool> ResetPasswordAsync(PasswordResetRequest passwordResetRequest);
        Task<bool> LockAccountAsync(int userId);
        Task<MessageResponse> ForgotUsernameAsync(ForgetUsernameRequest forgotUsernameDTO);
        Task<MessageResponse> ForgotPasswordAsync(ForgetPasswordRequest forgotPasswordDTO);
        Task<bool> IsAccountLockedAsync(int userId);
    }
}
