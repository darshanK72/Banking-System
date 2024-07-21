using System;
using System.ComponentModel.DataAnnotations;

namespace banksys.DTO
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TransactionDate { get; set; }

        public int FromAccountId { get; set; }

        public string? FromAccountNumber { get; set; }
        public int ToAccountId { get; set; }
        public string? ToAccountNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string TransactionType { get; set; }

        public string Status { get; set; }

        public string? Remark { get; set; }

        public string? MaturityInstructions { get; set; }
    }

}
