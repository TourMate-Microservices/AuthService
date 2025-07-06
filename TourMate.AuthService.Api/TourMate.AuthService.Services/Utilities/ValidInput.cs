using System;
using System.Text.RegularExpressions;

namespace TourMate.AuthService.Services.Utilities
{
    public static class ValidInput
    {
        public static bool IsPasswordSecure(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            // Kiểm tra độ dài tối thiểu
            if (password.Length < 8)
                return false;

            // Kiểm tra có ít nhất một chữ hoa
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return false;

            // Kiểm tra có ít nhất một chữ thường
            if (!Regex.IsMatch(password, @"[a-z]"))
                return false;

            // Kiểm tra có ít nhất một chữ số
            if (!Regex.IsMatch(password, @"[0-9]"))
                return false;

            // Kiểm tra có ít nhất một ký tự đặc biệt
            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""':;{}|<>]"))
                return false;

            return true;
        }

        public static bool IsEmailValid(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
