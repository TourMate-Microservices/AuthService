using System;
using System.Security.Cryptography;
using System.Text;

namespace TourMate.AuthService.Services.Utilities
{
    public static class HashString
    {
        public static string ToHashString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static bool VerifyHash(string input, string hash)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(hash))
                return false;

            return ToHashString(input) == hash;
        }
    }
}
