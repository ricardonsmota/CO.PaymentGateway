using System.Threading.Tasks;

namespace PaymentGatewayService.Payments
{
    public class PaymentRepository : IPaymentRepository
    {
        public async Task Create(Payment payment)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Payment> Get()
        {
            throw new System.NotImplementedException();
        }
    }
}