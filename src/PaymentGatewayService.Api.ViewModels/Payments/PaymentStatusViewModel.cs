using System;

namespace PaymentGatewayService.Api.ViewModels
{
    public class PaymentStatusViewModel
    {
        public string StatusCode { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime Modified { get; set; }
    }
}