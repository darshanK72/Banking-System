using System.Collections.Generic;
using System.Threading.Tasks;
using banksys.DTO;
using banksys.Models;

namespace banksys.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDTO>> GetAllAccountsAsync();
        Task<AccountDTO> GetAccountByIdAsync(int id);
        Task<AccountDTO> GetAccountByUserIdAsync(int id);
        Task<AccountDTO> CreateAccountAsync(AccountDTO accountDTO);
        Task<bool> UpdateAccountAsync(AccountDTO accountDTO);
        Task<bool> ApproveAccountAsync(int id);
        //Task<AccountStatementDTO> GetAccountStatementAsync(int accountId);
        //Task<bool> PerformTransactionAsync(TransactionRequestDTO transactionRequest);
        Task<MessageResponse> SendOtpToUserAsync(int userId);
        Task<string> RegisterNetBankingAsync(NetBankingRequest request);
    }
}
