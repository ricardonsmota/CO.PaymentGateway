namespace PaymentGatewayService.Common.Security
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public class Encryptor : IEncryptor
    {
        private readonly EncryptorParameters _parameters;

        public Encryptor(EncryptorParameters parameters)
        {
            _parameters = parameters;
        }

        public string Encrypt(string value)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _parameters.Key;
                aes.IV = _parameters.Iv;

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                            streamWriter.Write(value);
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        public string Decrypt(string value)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _parameters.Key;
                aes.IV = _parameters.Iv;

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(value)))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                            return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}