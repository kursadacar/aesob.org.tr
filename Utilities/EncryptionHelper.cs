using System.Security.Cryptography;
using System.Text;

namespace aesob.org.tr.Utilities
{
    public static class EncryptionHelper
    {
        public static string Encrypt(string text)
        {
            using (var encryptor = SHA256.Create())
            {
                var hash = encryptor.ComputeHash(Encoding.UTF8.GetBytes(text));

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
