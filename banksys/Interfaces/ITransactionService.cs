using System.Collections.Generic;
using System.Threading.Tasks;

using banksys.DTO;

namespace banksys.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDTO>> GetAllTransactionsAsync();
        Task<TransactionDTO> GetTransactionByIdAsync(int id);
        Task<TrasnactionResponse> CreateTransactionAsync(TransactionDTO transactionDTO);
        Task<IEnumerable<TransactionDTO>> GetTransactionByUserIdAsync(int id);
        Task<IEnumerable<TransactionDTO>> SearchTransactionsAsync(
     string? fromAccountNumber, string? toAccountNumber, decimal? minAmount, decimal? maxAmount,
     DateTime? startDate, DateTime? endDate, string? status);
        //Task<bool> UpdateTransactionAsync(TransactionDTO transaction);
        //Task<bool> DeleteTransactionAsync(int id);

    }
}
