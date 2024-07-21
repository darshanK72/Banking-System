using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using banksys.Interfaces;
using banksys.Models;
using banksys.DTO;
using System.Security.Cryptography;
using System.Text;

namespace banksys.Repositories
{
    public class UserService : IUserService
    {
        private readonly BankSysDbContext _context;

        public UserService(BankSysDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            var userDTOs = new List<UserDTO>();
            foreach (var user in users)
            {
                userDTOs.Add(new UserDTO
                {
                    UserId = user.UserId, 
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    Address = user.Address,
                    Role = user.Role
                });
            }
            return userDTOs;
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return new UserDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                Address = user.Address,
                Role = user.Role
            };
        }

        public async Task<bool> UpdateUserAsync(UserDTO userDTO)
        {
            var user = await _context.Users.FindAsync(userDTO.UserId);
            if (user == null) return false;

            user.UserName = userDTO.UserName;
            user.Email = userDTO.Email;
            user.FullName = userDTO.FullName;
            user.Address = userDTO.Address;
            user.Role = userDTO.Role;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}