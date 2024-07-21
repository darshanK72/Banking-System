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
        //Task<bool> UpdateTransactionAsync(TransactionDTO transaction);
        //Task<bool> DeleteTransactionAsync(int id);

    }
}
