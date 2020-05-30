using System.Threading.Tasks;
using PaymentGatewayService.Api.ViewModels;
using PaymentGatewayService.Api.ViewModels.Authentication;

namespace PaymentGatewayService.Api.Client
{
    public interface IPaymentServiceClient
    {
        Task<string> Login(UserLoginRequest request);

        Task<PaymentViewModel> CreatePayment(CreatePaymentRequest request);

        Task<PaymentViewModel> GetPayment(string paymentId);
    }
}