using System.Threading.Tasks;
using MongoDB.Driver;

namespace PaymentGatewayService.Payments
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<Payment> _payments;

        public PaymentRepository(IMongoDatabase database)
        {
            _database = database;
            _payments = database.GetCollection<Payment>("Payments");
        }

        public async Task Create(Payment payment)
        {
            await _payments.InsertOneAsync(payment);
        }

        public async Task<Payment> Get(string id)
        {
            var payment = await _payments.Find<Payment>(p => p.Id == id).FirstOrDefaultAsync();
            return payment;
        }

        public async Task Save(Payment payment)
        {
            await _payments.ReplaceOneAsync(p => p.Id == payment.Id, payment);
        }
    }
}