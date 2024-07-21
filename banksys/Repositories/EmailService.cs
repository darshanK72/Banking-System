
using banksys.Interfaces;
using banksys.Models;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Org.BouncyCastle.Utilities;
using System.Text;
using System.Threading.Tasks;

namespace banksys.Repository
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly BankSysDbContext _context;

        public EmailService(IConfiguration configuration, BankSysDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["SmtpSettings:SenderName"], _configuration["SmtpSettings:SenderEmail"]));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using (var client = new SmtpClient())
            {
                client.Connect(_configuration["SmtpSettings:Server"], int.Parse(_configuration["SmtpSettings:Port"]), false);
                client.Authenticate(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]);
                await client.SendAsync(message);
                client.Disconnect(true);
            }
        }

        public async Task NotifyPasswordReset(string userEmail, string resetToken)
        {
            string resetUrl = $"https://localhost:4200/reset-password?token={resetToken}";
            Console.WriteLine(resetUrl);
            string subject = "Password Reset Request";
            string body = $"<p>Dear User,</p><p>We received a request to reset your password. Please click the link below to reset your password:</p>" +
                          $"<p><a href='{resetUrl}'>Reset Password</a></p>" +
                          "<p>If you did not request this, please ignore this email.</p>" +
                          "<p>Thank you!</p>";
            await SendEmailAsync(userEmail, subject, body);
        }

        public async Task NotifyUsernameRecovery(string userEmail, string username)
        {
            string subject = "Username Recovery";
            string body = $"<p>Dear User,</p><p>Your username is: <strong>{username}</strong></p>" +
                          "<p>If you did not request this, please ignore this email.</p>" +
                          "<p>Thank you!</p>";
            await SendEmailAsync(userEmail, subject, body);
        }

        public async Task NotifyAccountLocked(string userEmail)
        {
            string subject = "Account Locked";
            string body = "<p>Dear User,</p><p>Your account has been locked due to too many failed login attempts.</p>" +
                          "<p>Please contact support for further assistance.</p>" +
                          "<p>Thank you!</p>";
            await SendEmailAsync(userEmail, subject, body);
        }

        public async Task NotifyAccountUnlocked(string userEmail)
        {
            string subject = "Account Unlocked";
            string body = "<p>Dear User,</p><p>Your account has been unlocked. You can now log in with your credentials.</p>" +
                          "<p>Thank you!</p>";
            await SendEmailAsync(userEmail, subject, body);
        }

        public async Task NotifyPasswordChanged(string userEmail)
        {
            string subject = "Password Changed Successfully";
            string body = "<p>Dear User,</p><p>Your password has been changed successfully.</p>" +
                          "<p>If you did not make this change, please contact support immediately.</p>" +
                          "<p>Thank you!</p>";
            await SendEmailAsync(userEmail, subject, body);
        }

        public async Task RequestOtpForUser(User user)
        {
            string otp = GenerateOtp();

            var account = await _context.Accounts.Where(ac => ac.UserId == user.UserId).FirstOrDefaultAsync();

            account.OTP = otp;

            await _context.SaveChangesAsync();
                
            await NotifyOtp(user.Email, otp);
        }


        public async Task NotifyOtp(string userEmail, string otp)
        {
            string subject = "Your OTP Code";
            string body = $"<p>Dear User,</p><p>Your OTP code is: <strong>{otp}</strong></p>" +
                          "<p>This code is valid for a short time. Please use it to complete your request.</p>" +
                          "<p>If you did not request this, please ignore this email.</p>" +
                          "<p>Thank you!</p>";
            await SendEmailAsync(userEmail, subject, body);
        }


        private string GenerateOtp(int length = 6)
        {
            var random = new Random();
            var otp = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                otp.Append(random.Next(1, 10));
            }
            return otp.ToString();
        }

    }
}
