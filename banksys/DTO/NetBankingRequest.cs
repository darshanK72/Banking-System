namespace banksys.DTO
{
    public class NetBankingRequest
    {
        public string AccountNumber { get; set; }
        public int UserId { get; set; }
        public string UserPassword { get; set; }
        public string TransactionPassword { get; set; }
        public int OTP { get; set; }
    }
}
