using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace banksys.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [Required]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string FathersName { get; set; }

        [Required]
        [StringLength(15)]
        public string MobileNumber { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string EmailId { get; set; }

        [Required]
        [StringLength(12)]
        public string AadharCardNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int ResidentialAddressId { get; set; }

        [ForeignKey("ResidentialAddressId")]
        public Address ResidentialAddress { get; set; }

        [Required]
        public int PermanentAddressId { get; set; }

        [ForeignKey("PermanentAddressId")]
        public Address PermanentAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string OccupationType { get; set; }

        public string? TransactionPassword { get; set; }

        [Required]
        [StringLength(50)]
        public string SourceOfIncome { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal GrossAnnualIncome { get; set; }

        [Required]
        public bool WantDebitCard { get; set; }

        public bool OptForNetBanking { get; set; }
        public int? OTP { get; set; }

        public bool IsApproved { get; set; }

        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountType { get; set; }

        public ICollection<Payee> Payees { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

    }
}