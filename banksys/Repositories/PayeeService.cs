using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using banksys.Interfaces;
using banksys.Models;
using banksys.DTO;

namespace banksys.Repositories
{
    public class PayeeService : IPayeeService
    {
        private readonly BankSysDbContext _context;

        public PayeeService(BankSysDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PayeeDTO>> GetAllPayeesAsync()
        {
            var payees = await _context.Payees
                .Include(p => p.PayeeAccount)
                .ToListAsync();

            return payees.Select(payee => new PayeeDTO
            {
                PayeeId = payee.PayeeId,
                PayeeName = payee.PayeeName,
                PayeeAccountNumber = payee.PayeeAccountNumber,
                PayeeAccountId = payee.PayeeAccountId
            });
        }

        public async Task<PayeeDTO> GetPayeeByIdAsync(int id)
        {
            var payee = await _context.Payees
                .Include(p => p.PayeeAccount)
                .FirstOrDefaultAsync(p => p.PayeeId == id);

            if (payee == null) return null;

            return new PayeeDTO
            {
                PayeeId = payee.PayeeId,
                PayeeName = payee.PayeeName,
                PayeeAccountNumber = payee.PayeeAccountNumber,
                PayeeAccountId = payee.PayeeAccountId
            };
        }

        public async Task<IEnumerable<PayeeDTO>> GetPayeeByUserIdAsync(int id)
        {
            return await _context.Payees
                .Include(p => p.PayeeAccount)
                .Where(p => p.UserId == id).Select(payee => new PayeeDTO
                {
                    PayeeId = payee.PayeeId,
                    PayeeName = payee.PayeeName,
                    PayeeAccountNumber = payee.PayeeAccountNumber,
                    PayeeAccountId = payee.PayeeAccountId
                }).ToListAsync();
        }

        public async Task<string> CreatePayeeAsync(PayeeDTO payeeDTO)
        {
            var account = _context.Accounts.Where(ac => ac.AccountNumber == payeeDTO.PayeeAccountNumber).FirstOrDefault();
            if(account == null)
            {
                throw new Exception("Account Number not found");
            }

            var payee = new Payee
            {
                PayeeName = payeeDTO.PayeeName,
                PayeeAccountNumber = payeeDTO.PayeeAccountNumber,
                PayeeAccountId = account.AccountId,
                PayeeAccount = account
            };

            _context.Payees.Add(payee);
            await _context.SaveChangesAsync();

            payeeDTO.PayeeId = payee.PayeeId;

            return "Payee created successfully";
        }

        public async Task<bool> UpdatePayeeAsync(PayeeDTO payeeDTO)
        {
            var payee = await _context.Payees.FindAsync(payeeDTO.PayeeId);
            if (payee == null) return false;

            var account = _context.Accounts.Where(ac => ac.AccountNumber == payeeDTO.PayeeAccountNumber).FirstOrDefault();
            if (account == null)
            {
                throw new Exception("Account Number not found");
            }

            payee.PayeeName = payeeDTO.PayeeName;
            payee.PayeeAccountNumber = account.AccountNumber;
            payee.PayeeAccountId = account.AccountId;

            _context.Payees.Update(payee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePayeeAsync(int id)
        {
            var payee = await _context.Payees.FindAsync(id);
            if (payee == null) return false;

            _context.Payees.Remove(payee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}



//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using banksys.Data;
//using banksys.DTO;
//using banksys.Models;
//using banksys.Interfaces;

//namespace banksys.Repositories
//{
//    public class PayeeService : IPayeeService
//    {
//        private readonly MyBankDBContext _context;

//        public PayeeService(MyBankDBContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<PayeeDTO>> GetAllPayeesAsync()
//        {
//            var payees = await _context.Payees.ToListAsync();
//            var payeeDTOs = new List<PayeeDTO>();
//            foreach (var payee in payees)
//            {
//                payeeDTOs.Add(new PayeeDTO
//                {
//                    Id = payee.Id,
//                    Name = payee.Name,
//                    AccountNumber = payee.AccountNumber,
//                    Address = payee.Address,
//                    Description = payee.Description
//                });
//            }
//            return payeeDTOs;
//        }

//        public async Task<PayeeDTO> GetPayeeByIdAsync(int id)
//        {
//            var payee = await _context.Payees.FindAsync(id);
//            if (payee == null) return null;

//            return new PayeeDTO
//            {
//                Id = payee.Id,
//                Name = payee.Name,
//                AccountNumber = payee.AccountNumber,
//                Address = payee.Address,
//                Description = payee.Description
//            };
//        }

//        public async Task<PayeeDTO> CreatePayeeAsync(PayeeDTO payeeDTO)
//        {
//            var payee = new Payee
//            {
//                Id = payeeDTO.Id,
//                Name = payeeDTO.Name,
//                AccountNumber = payeeDTO.AccountNumber,
//                Address = payeeDTO.Address,
//                Description = payeeDTO.Description
//            };

//            _context.Payees.Add(payee);
//            await _context.SaveChangesAsync();
//            return payeeDTO;
//        }

//        public async Task<bool> UpdatePayeeAsync(PayeeDTO payeeDTO)
//        {
//            var payee = await _context.Payees.FindAsync(payeeDTO.Id);
//            if (payee == null) return false;

//            payee.Name = payeeDTO.Name;
//            payee.AccountNumber = payeeDTO.AccountNumber;
//            payee.Address = payeeDTO.Address;
//            payee.Description = payeeDTO.Description;

//            _context.Payees.Update(payee);
//            await _context.SaveChangesAsync();
//            return true;
//        }

//        public async Task<bool> DeletePayeeAsync(int id)
//        {
//            var payee = await _context.Payees.FindAsync(id);
//            if (payee == null) return false;

//            _context.Payees.Remove(payee);
//            await _context.SaveChangesAsync();
//            return true;
//        }
//    }
//}
