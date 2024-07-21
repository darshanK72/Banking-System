using System;
using System.ComponentModel.DataAnnotations;

namespace banksys.DTO
{
    public class OpenAccountDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "Full name cannot be longer than 100 characters.")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string Address { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Mobile number cannot be longer than 15 characters.")]
        public string MobileNo { get; set; }

        [Required]
        [StringLength(12, ErrorMessage = "Aadhar Card number cannot be longer than 12 characters.")]
        public string AadharCard { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Occupation cannot be longer than 50 characters.")]
        public string Occupation { get; set; }
    }
}
