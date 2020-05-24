using System.Threading.Tasks;
using MongoDB.Driver;
using PaymentGatewayService.Common.Security;

namespace PaymentGatewayService.Payments
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IEncryptor _encryptor;
        private readonly IMongoCollection<Payment> _payments;

        public PaymentRepository(
            IEncryptor encryptor,
            IMongoDatabase database)
        {
            _encryptor = encryptor;
            _payments = database.GetCollection<Payment>("Payments");
        }

        public async Task Create(Payment payment)
        {
            await _payments.InsertOneAsync(payment.EncryptedPayment(_encryptor));
        }

        public async Task<Payment> Get(string id)
        {
            var payment = await _payments.Find(p => p.Id == id).FirstOrDefaultAsync();
            return payment.DecryptedPayment(_encryptor);
        }

        public async Task Save(Payment payment)
        {
            await _payments.ReplaceOneAsync(p => p.Id == payment.Id,
                payment.EncryptedPayment(_encryptor));
        }
    }
}