namespace PaymentGatewayService.AcquiringBank
{
    public class AcquiringBankServiceParameters
    {
        public int BankTotalBalance { get; set; }

        public int TransactionMinTimeMs { get; set; }

        public int TransactionMaxTimeMs { get; set; }

        public int TransactionTimeoutTimeMs { get; set; }
    }
}