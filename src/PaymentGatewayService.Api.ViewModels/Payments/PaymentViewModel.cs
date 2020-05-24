namespace PaymentGatewayService.Api.ViewModels
{
    public class PaymentViewModel
    {
        public string CardNumber { get; set; }

        public string Amount { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }

        public string Currency { get; set; }
    }
}