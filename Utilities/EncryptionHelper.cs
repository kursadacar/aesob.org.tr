using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace aesob.org.tr.Utilities
{
	public static class EncryptionHelper
	{
		public static string Encrypt(string text)
		{
			using (SHA256 encryptor = SHA256.Create())
			{
				byte[] hash = encryptor.ComputeHash(Encoding.UTF8.GetBytes(text));
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < hash.Length; i++)
				{
					sb.Append(hash[i].ToString("x2"));
				}
				return sb.ToString();
			}
		}

		public static string EncryptRFC(string text, string key)
		{
			byte[] clearBytes = Encoding.Unicode.GetBytes(text);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[13]
				{
					73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
					100, 101, 118
				});
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					text = Convert.ToBase64String(ms.ToArray());
					return text;
				}
			}
		}

		public static string DecryptRFC(string encryptedText, string key)
		{
			encryptedText = encryptedText.Replace(" ", "+");
			byte[] cipherBytes = Convert.FromBase64String(encryptedText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[13]
				{
					73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
					100, 101, 118
				});
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(cipherBytes, 0, cipherBytes.Length);
						cs.Close();
					}
					encryptedText = Encoding.Unicode.GetString(ms.ToArray());
					return encryptedText;
				}
			}
		}
	}
}
