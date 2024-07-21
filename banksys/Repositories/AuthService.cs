using banksys.DTO;
using banksys.Interfaces;
using banksys.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;

namespace banksys.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly BankSysDbContext _context;

        public AuthService(BankSysDbContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<UserDTO> RegisterUserAsync(UserDTO userDTO)
        {
            var user = new User
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                Password = HashPassword(userDTO.Password),
                FullName = userDTO.FullName,
                Address = userDTO.Address ?? string.Empty,
                Role = userDTO.Role ?? "User",
                IsLocked = false,
                InvlaidLoginAttempts = 0
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            userDTO.UserId = user.UserId;
            return userDTO;
        }

        public async Task<AdminDTO> RegisterAdminAsync(AdminDTO adminDTO)
        {
            var admin = new Admin
            {
                UserName = adminDTO.UserName,
                Email = adminDTO.Email,
                Password = HashPassword(adminDTO.Password),
                FullName = adminDTO.FullName,
                Address = adminDTO.Address ?? string.Empty,
                Role = adminDTO.Role ?? "Admin",
                IsLocked = false,
                InvlaidLoginAttempts = 0
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            adminDTO.AdminId = admin.AdminId;
            return adminDTO;
        }

        public async Task<string> LoginAsync(string usernameOrEmail, string password)
        {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.UserName == usernameOrEmail || a.Email == usernameOrEmail);

            if (admin != null)
            {
                if (admin.IsLocked)
                {
                    throw new AccountLockedException("Your account is locked due to multiple invalid login attempts. Please contact support or reset password.");
                }

                if (VerifyPassword(password, admin.Password))
                {
                    admin.InvlaidLoginAttempts = 0;
                    _context.Admins.Update(admin);
                    await _context.SaveChangesAsync();
                    return GenerateToken(admin);
                }
                else
                {
                    admin.InvlaidLoginAttempts++;
                    if (admin.InvlaidLoginAttempts >= 3)
                    {
                        admin.IsLocked = true;
                        await _emailService.NotifyAccountLocked(admin.Email);
                    }
                    _context.Admins.Update(admin);
                    await _context.SaveChangesAsync();

                    if (admin.IsLocked)
                    {
                        throw new AccountLockedException("Your account has been locked due to multiple invalid login attempts. Please contact support or reset password.");
                    }
                    throw new InvalidLoginException("Invalid password. Please try again.");
                }
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == usernameOrEmail || u.Email == usernameOrEmail);

            if (user != null)
            {
                if (user.IsLocked)
                {
                    throw new AccountLockedException("Your account is locked due to multiple invalid login attempts. Please contact support or reset password.");
                }

                if (VerifyPassword(password, user.Password))
                {
                    user.InvlaidLoginAttempts = 0;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return GenerateToken(user);
                }
                else
                {
                    user.InvlaidLoginAttempts++;
                    if (user.InvlaidLoginAttempts >= 3)
                    {
                        user.IsLocked = true;
                        await _emailService.NotifyAccountLocked(user.Email);
                    }
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    if (user.IsLocked)
                    {
                        throw new AccountLockedException("Your account has been locked due to multiple invalid login attempts. Please contact support.");
                    }
                    throw new InvalidLoginException("Invalid password. Please try again.");
                }
            }

            throw new InvalidLoginException("User not found.");
        }

        public async Task<bool> ResetPasswordAsync(PasswordResetRequest resetRequest)
        {
            var user = await _context.Users.Where(u => u.PasswordResetToken == resetRequest.Token).FirstOrDefaultAsync();
            var admin = await _context.Admins.Where(u => u.PasswordResetToken == resetRequest.Token).FirstOrDefaultAsync();

            if (user != null)
            {
                user.Password = HashPassword(resetRequest.Password);
                user.IsLocked = false;
                user.InvlaidLoginAttempts = 0;
                user.PasswordResetToken = null;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                await _emailService.NotifyPasswordChanged(user.Email);
                return true;
            }
            else if (admin != null)
            {
                admin.Password = HashPassword(resetRequest.Password);
                admin.IsLocked = false;
                admin.InvlaidLoginAttempts = 0;
                admin.PasswordResetToken = null;
                _context.Admins.Update(admin);
                await _context.SaveChangesAsync();

                await _emailService.NotifyPasswordChanged(admin.Email);
                return true;
            }

            return false;
        }

        public async Task<bool> LockAccountAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var admin = await _context.Admins.FindAsync(userId);

            if (user != null)
            {
                user.IsLocked = true;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                await _emailService.NotifyAccountLocked(user.Email);
                return true;
            }
            else if (admin != null)
            {
                admin.IsLocked = true;
                _context.Admins.Update(admin);
                await _context.SaveChangesAsync();

                await _emailService.NotifyAccountLocked(admin.Email);
                return true;
            }

            return false;
        }

        public async Task<MessageResponse> ForgotUsernameAsync(ForgetUsernameRequest forgotUsernameDTO)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == forgotUsernameDTO.Email);

            if (user != null)
            {
                await _emailService.NotifyUsernameRecovery(user.Email, user.UserName);
                return new MessageResponse()
                {
                    Message = "Your username has been sent to your email."
                };
            }

            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Email == forgotUsernameDTO.Email);

            if (admin != null)
            {
                await _emailService.NotifyUsernameRecovery(admin.Email, admin.UserName);
                return new MessageResponse()
                {
                    Message = "Your username has been sent to your email."
                };
            }

            return new MessageResponse()
            {
                Message = "No account found with the provided email."
            };
            
        }

        public async Task<MessageResponse> ForgotPasswordAsync(ForgetPasswordRequest forgotPasswordDTO)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == forgotPasswordDTO.Email);

            if (user != null)
            {
                var resetToken = GenerateResetToken(user);
                await _emailService.NotifyPasswordReset(user.Email, resetToken);

                user.PasswordResetToken = resetToken;

                await _context.SaveChangesAsync();

                return new MessageResponse()
                {
                    Message = "Your password reset link has been sent to your email."
                };
            }

            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Email == forgotPasswordDTO.Email);

            if (admin != null)
            {
                var resetToken = GenerateResetToken(admin);
                await _emailService.NotifyPasswordReset(admin.Email, resetToken);

                admin.PasswordResetToken = resetToken;

                await _context.SaveChangesAsync();

                return new MessageResponse()
                {
                    Message = "Your password reset link has been sent to your email."
                };
            }

            return new MessageResponse()
            {
                Message = "No account found with the provided email."
            };

        }

        public async Task<bool> IsAccountLockedAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var admin = await _context.Admins.FindAsync(userId);

            if (user != null)
            {
                return user.IsLocked;
            }
            else if (admin != null)
            {
                return admin.IsLocked;
            }

            return false;
        }

        private string GenerateToken(User user)
        {
            var userJson = JsonConvert.SerializeObject(user);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("user",userJson)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateToken(Admin admin)
        {
            var userJson = JsonConvert.SerializeObject(admin);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, admin.UserName),
                new Claim(JwtRegisteredClaimNames.Email, admin.Email),
                new Claim(ClaimTypes.Role, admin.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim("user",userJson)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateResetToken(User user)
        {
            return Guid.NewGuid().ToString();
        }

        private string GenerateResetToken(Admin admin)
        {
            return Guid.NewGuid().ToString();
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string providedPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }

    public class AccountLockedException : Exception
    {
        public AccountLockedException(string message) : base(message) { }
    }

    public class InvalidLoginException : Exception
    {
        public InvalidLoginException(string message) : base(message) { }
    }
}
