namespace PaymentGatewayService.Common.Security
{
    public class EncryptorParameters
    {
        public byte[] Key { get; set; }

        public byte[] Iv { get; set; }
    }
}