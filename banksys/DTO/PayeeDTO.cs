using System.ComponentModel.DataAnnotations;

namespace banksys.DTO
{
    public class PayeeDTO
    {
        public int PayeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string PayeeName { get; set; }

        [Required]
        [StringLength(20)]
        public string PayeeAccountNumber { get; set; }

        public int PayeeAccountId { get; set; }

        public int? UserId { get; set; }
    }
}
