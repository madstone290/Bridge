using System.Security.Cryptography;
using System.Text;

namespace Bridge.WebApp.Services
{
    /// <summary>
    /// 암호화 서비스
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// 평문을 암호화한다
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        string Encrypt(string plainText);

        /// <summary>
        /// 암호문을 복호화한다
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        string? Decrypt(string cipherText);
    }

    public class EncryptionService : IEncryptionService
    {
        /// <summary>
        /// 16바이트 키. 128, 192, 256비트 중 하나의 길이만 가능하다.
        /// </summary>
        static readonly string aes_key = "super_secret_123";

        /// <summary>
        /// 16바이트 IV
        /// </summary>
        static readonly string aes_iv = "super_secret_456";

        private readonly Aes aes;

        public EncryptionService()
        {
            aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(aes_key);
            aes.IV = Encoding.UTF8.GetBytes(aes_iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
        }

        public string Encrypt(string plainText)
        {
            ICryptoTransform encryptor = aes.CreateEncryptor();

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                }
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public string? Decrypt(string cipherText)
        {
            ICryptoTransform decryptor = aes.CreateDecryptor();

            try
            {
                byte[] buffer = Convert.FromBase64String(cipherText);
                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
