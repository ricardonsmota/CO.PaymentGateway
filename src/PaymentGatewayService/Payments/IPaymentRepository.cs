using System.Threading.Tasks;

namespace PaymentGatewayService.Payments
{
    public interface IPaymentRepository
    {
        Task<Payment> Create(Payment payment);

        Task<Payment> Get(string id);

        Task Save(Payment payment);
    }
}