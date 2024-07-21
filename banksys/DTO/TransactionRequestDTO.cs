using System.ComponentModel.DataAnnotations;

namespace banksys.DTO
{
    public class TransactionRequestDTO
    {
        [Required(ErrorMessage = "From account number is required.")]
        [StringLength(20, ErrorMessage = "From account number cannot be longer than 20 characters.")]
        public string FromAccountNumber { get; set; }

        [Required(ErrorMessage = "To account number is required.")]
        [StringLength(20, ErrorMessage = "To account number cannot be longer than 20 characters.")]
        public string ToAccountNumber { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment type is required.")]
        [RegularExpression("Credit|Debit", ErrorMessage = "Payment type must be either 'Credit' or 'Debit'.")]
        public string PaymentType { get; set; }
    }
}
