using System;

namespace PaymentGatewayService.Payments.Commands
{
    public class SetPaymentStatusAcceptedCommand
    {
        public Guid Id { get; set; }

        public DateTime Modified { get; set; }
    }
}