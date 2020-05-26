namespace PaymentGatewayService.AcquiringBank
{
    public class StartTransactionResponse
    {
        public string ErrorMessage { get; set; }

        public bool IsSuccess => ErrorMessage == null;
    }
}