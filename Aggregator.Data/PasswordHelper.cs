using System;
using System.Security.Cryptography;
using System.Text;

namespace Aggregator.Data
{
    internal class PasswordHelper
    {
        private const int SaltSize = 8;

        public string CreateSalt()
        {
            //Generate a cryptographic random number.
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[SaltSize];
            rng.GetBytes(buff);
            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

        public string GenerateSaltedHash(string password, string salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var saltBytes = Encoding.UTF8.GetBytes(salt);
            var plainTextWithSaltBytes = new byte[passwordBytes.Length + saltBytes.Length];
            for (var i = 0; i < passwordBytes.Length; i++)
            {
                plainTextWithSaltBytes[i] = passwordBytes[i];
            }
            for (var i = 0; i < saltBytes.Length; i++)
            {
                plainTextWithSaltBytes[passwordBytes.Length + i] = saltBytes[i];
            }

            var hashBytes = algorithm.ComputeHash(plainTextWithSaltBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}