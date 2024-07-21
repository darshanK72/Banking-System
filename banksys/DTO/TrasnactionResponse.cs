namespace banksys.DTO
{
    public class TrasnactionResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public TransactionDTO Transaction { get; set; }
    }
}
