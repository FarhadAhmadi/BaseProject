using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BaseProject.Application.Common.Utilities
{
    public static class StringHelper
    {
        /// <summary>
        /// Hashes the given string using BCrypt.
        /// </summary>
        public static string Hash(this string input)
            => BCrypt.Net.BCrypt.HashPassword(input);

        /// <summary>
        /// Verifies that a plaintext string matches a hashed password.
        /// </summary>
        public static bool Verify(this string input, string hashedPassword)
            => BCrypt.Net.BCrypt.Verify(input, hashedPassword);

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
