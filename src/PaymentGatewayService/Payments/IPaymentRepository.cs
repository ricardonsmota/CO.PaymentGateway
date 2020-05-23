using System.Threading.Tasks;

namespace PaymentGatewayService.Payments
{
    public interface IPaymentRepository
    {
        Task Create(Payment payment);

        Task<Payment> Get();
    }
}