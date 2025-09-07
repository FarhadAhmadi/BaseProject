using System;
using System.Security.Cryptography;
using System.Text;

namespace BaseProject.Application.Common.Utilities
{
    public static class CryptoHelper
    {
        private static readonly byte[] _iv = new byte[16]; // For AES initialization vector (all zeros for simplicity)

        /// <summary>
        /// Encrypts a plaintext string using AES.
        /// </summary>
        public static string Encrypt(string plaintext, string key)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32)); // Ensure 256-bit key
            aes.IV = _iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var plainBytes = Encoding.UTF8.GetBytes(plaintext);

            var encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// Decrypts an AES-encrypted string.
        /// </summary>
        public static string Decrypt(string ciphertext, string key)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32));
            aes.IV = _iv;

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var cipherBytes = Convert.FromBase64String(ciphertext);

            var decrypted = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
