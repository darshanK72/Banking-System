using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace banksys.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        public bool IsLocked { get; set; }
        public int InvlaidLoginAttempts { get; set; }

        [Required]
        public string Role { get; set; } = "User";
        public string? PasswordResetToken { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
