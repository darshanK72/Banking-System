using System.Collections.Generic;
using System.Threading.Tasks;
using banksys.Models;

namespace banksys.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<Account>> GetNotApprovedAccountsAsync();
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<bool> ApproveAccountAsync(int accountId);
        Task<bool> CancelAccountAsync(int accountId);
    }
}
