using PaymentGatewayService.Common.Security;

namespace PaymentGatewayService.Payments
{
    public static class PaymentExtensions
    {
        public static Payment EncryptedPayment(this Payment payment, IEncryptor encryptor)
        {
            payment.CardNumber = encryptor.Encrypt(payment.CardNumber);
            payment.Cvv = encryptor.Encrypt(payment.Cvv);

            return payment;
        }

        public static Payment DecryptedPayment(this Payment payment, IEncryptor encryptor)
        {
            payment.CardNumber = encryptor.Decrypt(payment.CardNumber);
            payment.Cvv = encryptor.Decrypt(payment.Cvv);

            return payment;
        }
    }
}