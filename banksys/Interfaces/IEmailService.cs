using banksys.Models;

namespace banksys.Interfaces
{
    public interface IEmailService
    {
        Task NotifyPasswordReset(string userEmail, string resetToken);
        Task NotifyUsernameRecovery(string userEmail, string username);
        Task NotifyAccountLocked(string userEmail);
        Task NotifyAccountUnlocked(string userEmail);
        Task NotifyPasswordChanged(string userEmail);
        Task RequestOtpForUser(User user);
    }
}
