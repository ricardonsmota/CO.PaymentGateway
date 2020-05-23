using System;

namespace PaymentGatewayService.Payments
{
    public class Payment
    {
        public Guid Id { get; set; }

        public string CardNumber { get; set; }

        public string Amount { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }

        public int Cvv { get; set; }

        public PaymentCurrency Currency { get; set; }

        public DateTime Created { get; set; }

        public PaymentStatus Status { get; set; }
    }
}