using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace banksys.Models
{
    public class Payee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PayeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string PayeeName { get; set; }

        [Required]
        [StringLength(20)]
        public string PayeeAccountNumber { get; set; }

        [Required]
        public int PayeeAccountId { get; set; }

        [ForeignKey("PayeeAccountId")]
        public Account PayeeAccount { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }


    }
}
