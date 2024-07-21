using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using banksys.Models;
using banksys.Interfaces;

namespace banksys.Repositories
{
    public class AdminService : IAdminService
    {
        private readonly BankSysDbContext _context;

        public AdminService(BankSysDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetNotApprovedAccountsAsync()
        {
            return await _context.Accounts.Where(a => !a.IsApproved).ToListAsync();
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<bool> ApproveAccountAsync(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                return false;
            }
            account.IsApproved = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelAccountAsync(int accountId)
        {
            var account = await _context.Accounts.Include(a => a.Payees).Include(a => a.Transactions).FirstOrDefaultAsync(a => a.AccountId == accountId);

            if (account == null)
            {
                return false;
            }

            _context.Payees.RemoveRange(account.Payees);
            _context.Transactions.RemoveRange(account.Transactions);

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
