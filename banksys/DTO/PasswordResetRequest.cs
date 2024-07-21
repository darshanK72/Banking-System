namespace banksys.DTO
{
    public class PasswordResetRequest
    {
        public string Password { get; set; }
        public string VerifyPassword { get; set; }
        public string Token { get; set; }
    }
}
