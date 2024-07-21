using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace banksys.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TransactionDate { get; set; }

        [Required]
        public int FromAccountId { get; set; }

        [ForeignKey("FromAccountId")]
        public Account FromAccount { get; set; }

        [Required]
        public int ToAccountId { get; set; }

        [ForeignKey("ToAccountId")]
        public Account ToAccount { get; set; }

        [Required]
        [StringLength(10)]
        public string TransactionType { get; set; }

        public string Status { get; set; }

        public string? Remark { get; set; }

        public string? MaturityInstructions { get; set; }
    }
}
