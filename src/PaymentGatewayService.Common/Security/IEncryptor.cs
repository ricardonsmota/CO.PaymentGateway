namespace PaymentGatewayService.Common.Security
{
    public interface IEncryptor
    {
        string Encrypt(string value);

        string Decrypt(string value);
    }
}