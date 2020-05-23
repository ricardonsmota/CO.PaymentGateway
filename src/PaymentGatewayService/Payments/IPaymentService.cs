using System.Threading.Tasks;
using PaymentGatewayService.Common.ServiceResponse;
using PaymentGatewayService.Payments.Commands;

namespace PaymentGatewayService.Payments
{
    public interface IPaymentService
    {
        Task<ServiceResult<Payment>> Create(CreatePaymentCommand command);

        Task<ServiceResult<Payment>> Get(GetPaymentCommand command);

        Task<ServiceResult> SetStatusAccepted(SetPaymentStatusAcceptedCommand command);

        Task<ServiceResult> SetStatusRejected(SetPaymentStatusRejectedCommand command);
    }
}