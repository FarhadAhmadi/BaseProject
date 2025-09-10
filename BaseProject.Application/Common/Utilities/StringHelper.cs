using BaseProject.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;

namespace BaseProject.Application.Common.Utilities
{
    public static class StringHelper
    {
        private static readonly PasswordHasher<ApplicationUser> _hasher = new();

        /// <summary>
        /// Hashes the given string using Identity's PasswordHasher.
        /// </summary>
        public static string HashPassword(this string password)
        {
            // Pass null if you don't have a user object
            return _hasher.HashPassword(null, password);
        }

        /// <summary>
        /// Verifies that a plaintext string matches a hashed password using Identity's PasswordHasher.
        /// </summary>
        public static bool VerifyPassword(this string password, string hashedPassword)
        {
            var result = _hasher.VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success
                || result == PasswordVerificationResult.SuccessRehashNeeded;
        }

        private static readonly Random _random = new();

        /// <summary>
        /// Generates a random alphanumeric string of given length.
        /// </summary>
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Range(0, length)
                .Select(_ => chars[_random.Next(chars.Length)]).ToArray());
        }
    }
}
