using System;

namespace PaymentGatewayService.Payments.Commands
{
    public class SetPaymentStatusRejectedCommand
    {
        public string Id { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime Modified { get; set; }
    }
}