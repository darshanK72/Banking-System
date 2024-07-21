using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using banksys.DTO;
using banksys.Interfaces;
using banksys.Models;

namespace banksys.Repositories
{
    public class AccountService : IAccountService
    {
        private readonly BankSysDbContext _context;
        private readonly IEmailService _emailService;

        public AccountService(BankSysDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IEnumerable<AccountDTO>> GetAllAccountsAsync()
        {
            var accounts = await _context.Accounts
                .Include(a => a.User)
                .Include(a => a.ResidentialAddress)
                .Include(a => a.PermanentAddress)
                .ToListAsync();

            return accounts.Select(account => new AccountDTO
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                Title = account.Title,
                FirstName = account.FirstName,
                MiddleName = account.MiddleName,
                LastName = account.LastName,
                FathersName = account.FathersName,
                MobileNumber = account.MobileNumber,
                EmailId = account.EmailId,
                AadharCardNumber = account.AadharCardNumber,
                DateOfBirth = account.DateOfBirth,
                ResidentialAddress = account.ResidentialAddress,
                PermanentAddress = account.PermanentAddress,
                OccupationType = account.OccupationType,
                SourceOfIncome = account.SourceOfIncome,
                GrossAnnualIncome = account.GrossAnnualIncome,
                WantDebitCard = account.WantDebitCard,
                OptForNetBanking = account.OptForNetBanking,
                IsApproved = account.IsApproved,
                Balance = account.Balance,
                UserId = account.UserId,
                AccountType = account.AccountType,
                //Payees = account.Payees.Select(p => new PayeeDTO
                //{
                //    PayeeId = p.PayeeId,
                //    PayeeName = p.PayeeName,
                //    AccountNumber = p.AccountNumber
                //}).ToList(),
                //Transactions = account.Transactions.Select(t => new TransactionDTO
                //{
                //    TransactionId = t.TransactionId,
                //    Description = t.Description,
                //    Amount = t.Amount,
                //    TransactionDate = t.TransactionDate,
                //    TransactionType = t.TransactionType,
                //    Status = t.Status
                //}).ToList()
            });
        }
        public async Task<AccountDTO> GetAccountByUserIdAsync(int UserId)
        {
            var account = await _context.Accounts.Where(a => a.UserId == UserId)
                .Include(a => a.User)
                .Include(a => a.ResidentialAddress)
                .Include(a => a.PermanentAddress)
                .FirstOrDefaultAsync();

            if (account == null) return null;

            return new AccountDTO
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                Title = account.Title,
                FirstName = account.FirstName,
                MiddleName = account.MiddleName,
                LastName = account.LastName,
                FathersName = account.FathersName,
                MobileNumber = account.MobileNumber,
                EmailId = account.EmailId,
                AadharCardNumber = account.AadharCardNumber,
                DateOfBirth = account.DateOfBirth,
                ResidentialAddress = account.ResidentialAddress,
                PermanentAddress = account.PermanentAddress,
                OccupationType = account.OccupationType,
                SourceOfIncome = account.SourceOfIncome,
                GrossAnnualIncome = account.GrossAnnualIncome,
                WantDebitCard = account.WantDebitCard,
                OptForNetBanking = account.OptForNetBanking,
                IsApproved = account.IsApproved,
                Balance = account.Balance,
                UserId = account.UserId,
                AccountType = account.AccountType,
                TransactionPassword = account.TransactionPassword
                //Payees = account.Payees.Select(p => new PayeeDTO
                //{
                //    PayeeId = p.PayeeId,
                //    PayeeName = p.PayeeName,
                //    AccountNumber = p.AccountNumber
                //}).ToList(),
                //Transactions = account.Transactions.Select(t => new TransactionDTO
                //{
                //    TransactionId = t.TransactionId,
                //    Description = t.Description,
                //    Amount = t.Amount,
                //    TransactionDate = t.TransactionDate,
                //    TransactionType = t.TransactionType,
                //    Status = t.Status
                //}).ToList()
            };
        }

        public async Task<AccountDTO> GetAccountByIdAsync(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.User)
                .Include(a => a.ResidentialAddress)
                .Include(a => a.PermanentAddress)
                .FirstOrDefaultAsync(a => a.AccountId == id);

            if (account == null) return null;

            return new AccountDTO
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                Title = account.Title,
                FirstName = account.FirstName,
                MiddleName = account.MiddleName,
                LastName = account.LastName,
                FathersName = account.FathersName,
                MobileNumber = account.MobileNumber,
                EmailId = account.EmailId,
                AadharCardNumber = account.AadharCardNumber,
                DateOfBirth = account.DateOfBirth,
                ResidentialAddress = account.ResidentialAddress,
                PermanentAddress = account.PermanentAddress,
                OccupationType = account.OccupationType,
                SourceOfIncome = account.SourceOfIncome,
                GrossAnnualIncome = account.GrossAnnualIncome,
                WantDebitCard = account.WantDebitCard,
                OptForNetBanking = account.OptForNetBanking,
                IsApproved = account.IsApproved,
                Balance = account.Balance,
                UserId = account.UserId,
                AccountType = account.AccountType,
                TransactionPassword = account.TransactionPassword
                //Payees = account.Payees.Select(p => new PayeeDTO
                //{
                //    PayeeId = p.PayeeId,
                //    PayeeName = p.PayeeName,
                //    AccountNumber = p.AccountNumber
                //}).ToList(),
                //Transactions = account.Transactions.Select(t => new TransactionDTO
                //{
                //    TransactionId = t.TransactionId,
                //    Description = t.Description,
                //    Amount = t.Amount,
                //    TransactionDate = t.TransactionDate,
                //    TransactionType = t.TransactionType,
                //    Status = t.Status
                //}).ToList()
            };
        }
        public async Task<AccountDTO> CreateAccountAsync(AccountDTO accountDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Address.Add(accountDTO.ResidentialAddress);
                _context.Address.Add(accountDTO.PermanentAddress);
                await _context.SaveChangesAsync();

                var account = new Account
                {
                    AccountNumber = GenerateAccountNumber(),
                    Title = accountDTO.Title,
                    FirstName = accountDTO.FirstName,
                    MiddleName = accountDTO.MiddleName,
                    LastName = accountDTO.LastName,
                    FathersName = accountDTO.FathersName,
                    MobileNumber = accountDTO.MobileNumber,
                    EmailId = accountDTO.EmailId,
                    AadharCardNumber = accountDTO.AadharCardNumber,
                    DateOfBirth = accountDTO.DateOfBirth,
                    ResidentialAddressId = accountDTO.ResidentialAddress.AddressId,
                    PermanentAddressId = accountDTO.PermanentAddress.AddressId,
                    OccupationType = accountDTO.OccupationType,
                    SourceOfIncome = accountDTO.SourceOfIncome,
                    GrossAnnualIncome = accountDTO.GrossAnnualIncome,
                    WantDebitCard = accountDTO.WantDebitCard,
                    OptForNetBanking = accountDTO.OptForNetBanking,
                    IsApproved = accountDTO.IsApproved || false,
                    Balance = accountDTO.Balance,
                    UserId = accountDTO.UserId,
                    AccountType = accountDTO.AccountType,
                };

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                accountDTO.AccountId = account.AccountId;
                return accountDTO;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new InvalidOperationException("An error occurred while creating the account. Please try again later.", ex);
            }
        }
        private string GenerateAccountNumber()
        {
            Random random = new Random();
            string accountNumber = "";
            for (int i = 0; i < 12; i++)
            {
                accountNumber += random.Next(0, 10).ToString();
            }
            return accountNumber;
        }

        public async Task<MessageResponse> SendOtpToUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return new MessageResponse()
                {
                    Message = "Failed to send OTP to user"
                };
            }

            await _emailService.RequestOtpForUser(user);

            return new MessageResponse()
            {
                Message = "OTP Shared to user's email"
            };

        }

        public async Task<string> RegisterNetBankingAsync(NetBankingRequest request)
        {
            var user = await _context.Users
            .Include(u => u.Accounts)
            .FirstOrDefaultAsync(u => u.UserId == request.UserId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.UserPassword, user.Password);
            if (!isPasswordValid)
            {
                throw new Exception("Invalid user password");
            }

            var account = user.Accounts.FirstOrDefault(a => a.AccountNumber == request.AccountNumber);
            if (account == null)
            {
                throw new Exception("Account number is incorrect.");
                
            }

            if (account.OTP != request.OTP)
            {
                throw new Exception("Invlaid OTP");

            }

            account.TransactionPassword = BCrypt.Net.BCrypt.HashPassword(request.TransactionPassword);
            account.OptForNetBanking = true;
            account.OTP = null;

            try
            {
                await _context.SaveChangesAsync();
                return "Net banking registration successful.";
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while registering for net banking.");
            }
        }
        public async Task<bool> UpdateAccountAsync(AccountDTO accountDTO)
        {
            var account = await _context.Accounts.FindAsync(accountDTO.AccountId);
            if (account == null) return false;

            account.AccountNumber = accountDTO.AccountNumber;
            account.Title = accountDTO.Title;
            account.FirstName = accountDTO.FirstName;
            account.MiddleName = accountDTO.MiddleName;
            account.LastName = accountDTO.LastName;
            account.FathersName = accountDTO.FathersName;
            account.MobileNumber = accountDTO.MobileNumber;
            account.EmailId = accountDTO.EmailId;
            account.AadharCardNumber = accountDTO.AadharCardNumber;
            account.DateOfBirth = accountDTO.DateOfBirth;
            account.ResidentialAddressId = accountDTO.ResidentialAddress.AddressId;
            account.PermanentAddressId = accountDTO.PermanentAddress.AddressId;
            account.OccupationType = accountDTO.OccupationType;
            account.SourceOfIncome = accountDTO.SourceOfIncome;
            account.GrossAnnualIncome = accountDTO.GrossAnnualIncome;
            account.WantDebitCard = accountDTO.WantDebitCard;
            account.OptForNetBanking = accountDTO.OptForNetBanking;
            account.IsApproved = accountDTO.IsApproved;
            account.Balance = accountDTO.Balance;
            account.UserId = accountDTO.UserId;
            account.AccountType = accountDTO.AccountType;

            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveAccountAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return false;

            account.IsApproved = true;
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<AccountSummaryDto> GetAccountSummaryAsync(string accountNumber, string userEmailOrUsername, string userPassword)
        //{
        //    var account = await _context.Accounts
        //        .Include(a => a.Transactions)
        //        .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber && a.User.Email == userEmailOrUsername && a.User.Password == userPassword);

        //    if (account == null)
        //    {
        //        return null; // Handle this case as needed (e.g., throw exception or return not found)
        //    }

        //    var transactions = account.Transactions
        //        .OrderByDescending(t => t.TransactionDate)
        //        .Take(10) // Get the latest 10 transactions
        //        .Select(t => new TransactionDto
        //        {
        //            TransactionId = t.TransactionId,
        //            Description = t.Description,
        //            Amount = t.Amount,
        //            TransactionDate = t.TransactionDate,
        //            TransactionType = t.TransactionType,
        //            Status = t.Status
        //        })
        //        .ToList();

        //    var accountSummary = new AccountSummaryDto
        //    {
        //        AccountNumber = account.AccountNumber,
        //        Balance = account.Balance,
        //        Transactions = transactions
        //    };

        //    return accountSummary;
        //}

        //public async Task<AccountStatementDTO> GetAccountStatementAsync(int accountId)
        //    {
        //        var account = await _context.Accounts
        //            .Include(a => a.User)
        //            .Include(a => a.Transactions)
        //            .Where(a => a.AccountId == accountId)
        //            .Select(a => new AccountStatementDTO
        //            {
        //                AccountNumber = a.AccountNumber,
        //                Name = $"{a.FirstName} {a.LastName}",
        //                Balance = a.Balance,
        //                //Transactions = a.Transactions.Select(t => new TransactionDTO
        //                //{
        //                //    TransactionId = t.TransactionId,
        //                //    Description = t.Description,
        //                //    Amount = t.Amount,
        //                //    TransactionDate = t.TransactionDate,
        //                //    TransactionType = t.TransactionType,
        //                //    Status = t.Status
        //                //}).ToList()
        //            })
        //            .FirstOrDefaultAsync();

        //        if (account == null)
        //        {
        //            throw new Exception("Account not found");
        //        }

        //        return account;
        //    }

        //public async Task<bool> PerformTransactionAsync(TransactionRequestDTO transactionRequest)
        //{
        //    using (var transaction = await _context.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            var fromAccount = await _context.Accounts
        //                .FirstOrDefaultAsync(a => a.AccountNumber == transactionRequest.FromAccountNumber);
        //            if (fromAccount == null)
        //            {
        //                throw new Exception("From account not found");
        //            }

        //            var toAccount = await _context.Accounts
        //                .FirstOrDefaultAsync(a => a.AccountNumber == transactionRequest.ToAccountNumber);
        //            if (toAccount == null)
        //            {
        //                throw new Exception("To account not found");
        //            }

        //            if (fromAccount.Balance < transactionRequest.Amount)
        //            {
        //                throw new Exception("Insufficient balance");
        //            }

        //            fromAccount.Balance -= transactionRequest.Amount;
        //            toAccount.Balance += transactionRequest.Amount;

        //            var fromTransaction = new Transaction
        //            {
        //                AccountId = fromAccount.AccountId,
        //                Description = transactionRequest.Description,
        //                Amount = -transactionRequest.Amount,
        //                TransactionDate = DateTime.UtcNow,
        //                TransactionType = "Debit",
        //                Status = "Completed"
        //            };

        //            var toTransaction = new Transaction
        //            {
        //                AccountId = toAccount.AccountId,
        //                Description = transactionRequest.Description,
        //                Amount = transactionRequest.Amount,
        //                TransactionDate = DateTime.UtcNow,
        //                TransactionType = "Credit",
        //                Status = "Completed"
        //            };

        //            _context.Transactions.Add(fromTransaction);
        //            _context.Transactions.Add(toTransaction);

        //            await _context.SaveChangesAsync();
        //            await transaction.CommitAsync();

        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //            await transaction.RollbackAsync();
        //            throw;
        //        }
        //    }
        //}
    }
}
