namespace PaymentGatewayService.AcquiringBank.Commands
{
    public class StartTransactionCommand
    {
        public string PaymentId { get; set; }

        public double Amount { get; set; }
    }
}