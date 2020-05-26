namespace PaymentGatewayService.AcquiringBank.Commands
{
    public class StartTransactionCommand
    {
        public string PaymentId { get; set; }

        public int Amount { get; set; }
    }
}