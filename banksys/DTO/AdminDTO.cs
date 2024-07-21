using System.ComponentModel.DataAnnotations;

namespace banksys.DTO
{
    public class AdminDTO
    {
        public int AdminId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
