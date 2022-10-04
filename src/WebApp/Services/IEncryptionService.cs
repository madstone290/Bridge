using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
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
        public class Config
        {
            /// <summary>
            /// AES 암호화 키(16자)
            /// </summary>
            [Required]
            [StringLength(16, MinimumLength = 16)]
            public string Key { get; set; } = string.Empty;

            /// <summary>
            /// AES 초기화 벡터 문자열(16자)
            /// </summary>
            [Required]
            [StringLength(16, MinimumLength = 16)]
            public string IV { get; set; } = string.Empty;
        }

        private readonly Aes aes;

        public EncryptionService(IOptions<Config> configOptions)
        {
            var config = configOptions.Value;

            aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(config.Key);
            aes.IV = Encoding.UTF8.GetBytes(config.IV);
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
