using System.Threading.Tasks;
using PaymentGatewayService.Common.ServiceResponse;
using PaymentGatewayService.Payments.Commands;

namespace PaymentGatewayService.Payments
{
    public interface IPaymentService
    {
        Task<ServiceResponse<Payment>> Create(CreatePaymentCommand command);

        Task<ServiceResponse<Payment>> Get(GetPaymentCommand command);
    }
}