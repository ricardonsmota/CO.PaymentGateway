using System;

namespace PaymentGatewayService.Payments.Commands
{
    public class SetPaymentStatusAcceptedCommand
    {
        public string Id { get; set; }

        public DateTime Modified { get; set; }
    }
}