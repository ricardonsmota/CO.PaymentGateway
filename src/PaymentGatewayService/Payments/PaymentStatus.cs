using System;

namespace PaymentGatewayService.Payments
{
    public class PaymentStatus
    {
        public PaymentStatusCode StatusCode { get; set; }

        public bool IsError => StatusCode == PaymentStatusCode.Rejected;

        public string ErrorMessage { get; set; }

        public DateTime Modified { get; set; }
    }
}