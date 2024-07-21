using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using banksys.Interfaces;
using banksys.Models;
using banksys.DTO;

public class TransactionService : ITransactionService
{
    private readonly BankSysDbContext _context;

    public TransactionService(BankSysDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TransactionDTO>> GetAllTransactionsAsync()
    {
        var transactions = await _context.Transactions
            .Include(t => t.FromAccount)
            .Include(t => t.ToAccount) 
            .ToListAsync();

        return transactions.Select(transaction => new TransactionDTO
        {
            TransactionId = transaction.TransactionId,
            Amount = transaction.Amount,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            FromAccountId = transaction.FromAccountId,
            ToAccountId = transaction.ToAccountId,
            TransactionType = transaction.TransactionType,
            Status = transaction.Status
        });
    }

    public async Task<TransactionDTO> GetTransactionByIdAsync(int id)
    {
        var transaction = await _context.Transactions
            .Include(t => t.FromAccount)
            .Include(t => t.ToAccount)  
            .FirstOrDefaultAsync(t => t.TransactionId == id);

        if (transaction == null) return null;

        return new TransactionDTO
        {
            TransactionId = transaction.TransactionId,
            Amount = transaction.Amount,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            FromAccountId = transaction.FromAccountId,
            ToAccountId = transaction.ToAccountId,
            TransactionType = transaction.TransactionType,
            Status = transaction.Status
        };
    }

    public async Task<IEnumerable<TransactionDTO>> GetTransactionByUserIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        var account = await _context.Accounts.Where(ac => ac.UserId == user.UserId).FirstOrDefaultAsync();

        return await _context.Transactions.Where(t => t.FromAccountId == account.AccountId || t.ToAccountId == account.AccountId).Select(transaction => new TransactionDTO()
        {
            TransactionId = transaction.TransactionId,
            Amount = transaction.Amount,
            Description = transaction.Description,
            TransactionDate = transaction.TransactionDate,
            FromAccountId = transaction.FromAccountId,
            ToAccountId = transaction.ToAccountId,
            TransactionType = transaction.TransactionType,
            Status = transaction.Status,
            Remark = transaction.Remark,
            MaturityInstructions = transaction.MaturityInstructions,
        }).ToListAsync();
    }

    //public async Task<TransactionDTO> CreateTransactionAsync(TransactionDTO transactionDTO)
    //{
    //    var transaction = new Transaction
    //    {
    //        Amount = transactionDTO.Amount,
    //        Description = transactionDTO.Description,
    //        TransactionDate = transactionDTO.TransactionDate,
    //        FromAccountId = transactionDTO.FromAccountId,
    //        ToAccountId = transactionDTO.ToAccountId,
    //        TransactionType = transactionDTO.TransactionType,
    //        Status = transactionDTO.Status
    //    };

    //    _context.Transactions.Add(transaction);
    //    await _context.SaveChangesAsync();

    //    transactionDTO.TransactionId = transaction.TransactionId;

    //    return transactionDTO;
    //}

    public async Task<TrasnactionResponse> CreateTransactionAsync(TransactionDTO transactionDTO)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var fromAccount = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.AccountNumber == transactionDTO.FromAccountNumber);

                var toAccount = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.AccountNumber == transactionDTO.ToAccountNumber);

                if (fromAccount == null || toAccount == null)
                {
                    return new TrasnactionResponse { Success = false, Message = "Invalid account number(s)." };
                }

                if (fromAccount.Balance < transactionDTO.Amount)
                {
                    return new TrasnactionResponse { Success = false, Message = "Insufficient balance." };
                }

                var transactionEntity = new Transaction
                {
                    Description = transactionDTO.Description,
                    Amount = transactionDTO.Amount,
                    TransactionDate = transactionDTO.TransactionDate,
                    FromAccountId = fromAccount.AccountId,
                    ToAccountId = toAccount.AccountId,
                    TransactionType = transactionDTO.TransactionType,
                    Status = "Completed",
                    Remark = transactionDTO.Remark,
                    MaturityInstructions = transactionDTO.MaturityInstructions
                };

                _context.Transactions.Add(transactionEntity);

                fromAccount.Balance -= transactionDTO.Amount;
                toAccount.Balance += transactionDTO.Amount;

                _context.Accounts.Update(fromAccount);
                _context.Accounts.Update(toAccount);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                transactionDTO.TransactionId = transactionEntity.TransactionId;

                return new TrasnactionResponse { Success = true, Message = "Transaction successful.", Transaction = transactionDTO };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new TrasnactionResponse { Success = false, Message = "An error occurred while processing the transaction: " + ex.Message };
            }
        }
    }

    public async Task<bool> UpdateTransactionAsync(TransactionDTO transactionDTO)
    {
        var transaction = await _context.Transactions.FindAsync(transactionDTO.TransactionId);
        if (transaction == null) return false;

        transaction.Amount = transactionDTO.Amount;
        transaction.Description = transactionDTO.Description;
        transaction.TransactionDate = transactionDTO.TransactionDate;
        transaction.FromAccountId = transactionDTO.FromAccountId;
        transaction.ToAccountId = transactionDTO.ToAccountId;
        transaction.TransactionType = transactionDTO.TransactionType;
        transaction.Status = transactionDTO.Status;

        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTransactionAsync(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null) return false;

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return true;
    }
}



//// Services/TransactionService.cs
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using banksys.DTO;
//using banksys.Interfaces;
//using banksys.Data;
//using banksys.Models;

//public class TransactionService : ITransactionService
//{
//    private readonly MyBankDBContext _context;

//    public TransactionService(MyBankDBContext context)
//    {
//        _context = context;
//    }

//    public async Task<IEnumerable<TransactionDTO>> GetAllTransactionsAsync()
//    {
//        var transactions = await _context.Transactions.ToListAsync();
//        var transactionDTOs = new List<TransactionDTO>();
//        foreach (var transaction in transactions)
//        {
//            transactionDTOs.Add(new TransactionDTO
//            {
//                Id = transaction.Id,
//                AccountId = transaction.AccountId,
//                Description = transaction.Description,
//                Amount = transaction.Amount,
//                TransactionDate = transaction.TransactionDate,
//                Type = transaction.Type
//            });
//        }
//        return transactionDTOs;
//    }

//    public async Task<TransactionDTO> GetTransactionByIdAsync(int id)
//    {
//        var transaction = await _context.Transactions.FindAsync(id);
//        if (transaction == null) return null;

//        return new TransactionDTO
//        {
//            Id = transaction.Id,
//            AccountId = transaction.AccountId,
//            Description = transaction.Description,
//            Amount = transaction.Amount,
//            TransactionDate = transaction.TransactionDate,
//            Type = transaction.Type
//        };
//    }

//    //public async Task<TransactionDTO> CreateTransactionAsync(TransactionDTO transactionDTO)
//    //{
//    //    var transaction = new Transaction
//    //    {
//    //        Id = transactionDTO.Id,
//    //        AccountId = transactionDTO.AccountId,
//    //        Description = transactionDTO.Description,
//    //        Amount = transactionDTO.Amount,
//    //        TransactionDate = transactionDTO.TransactionDate,
//    //        Type = transactionDTO.Type
//    //    };

//    //    _context.Transactions.Add(transaction);
//    //    await _context.SaveChangesAsync();
//    //    return transactionDTO;
//    //}

//    //public async Task<bool> UpdateTransactionAsync(TransactionDTO transactionDTO)
//    //{
//    //    var transaction = await _context.Transactions.FindAsync(transactionDTO.Id);
//    //    if (transaction == null) return false;

//    //    transaction.AccountId = transactionDTO.AccountId;
//    //    transaction.Description = transactionDTO.Description;
//    //    transaction.Amount = transactionDTO.Amount;
//    //    transaction.TransactionDate = transactionDTO.TransactionDate;
//    //    transaction.Type = transactionDTO.Type;

//    //    _context.Transactions.Update(transaction);
//    //    await _context.SaveChangesAsync();
//    //    return true;
//    //}

//    //public async Task<bool> DeleteTransactionAsync(int id)
//    //{
//    //    var transaction = await _context.Transactions.FindAsync(id);
//    //    if (transaction == null) return false;

//    //    _context.Transactions.Remove(transaction);
//    //    await _context.SaveChangesAsync();
//    //    return true;
//    //}
//}
