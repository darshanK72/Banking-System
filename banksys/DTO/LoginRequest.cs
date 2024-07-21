using System.ComponentModel.DataAnnotations;

namespace banksys.DTO
{
    public class LoginRequest
    {
        public string UserNameOrEmail { get; set; }
         public string Password { get; set; }
    }
}
