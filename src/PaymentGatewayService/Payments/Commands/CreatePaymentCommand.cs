using System;

namespace PaymentGatewayService.Payments.Commands
{
    public class CreatePaymentCommand
    {
        public string CardNumber { get; set; }

        public string Amount { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }

        public string Cvv { get; set; }

        public string Currency { get; set; }

        public DateTime Created { get; set; }
    }
}