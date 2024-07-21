using System.ComponentModel.DataAnnotations;

namespace banksys.DTO
{
    public class AccountStatementDTO
    {
        [Required(ErrorMessage = "Account number is required.")]
        [StringLength(20, ErrorMessage = "Account number cannot be longer than 20 characters.")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a positive value.")]
        public decimal Balance { get; set; }
    }
}
