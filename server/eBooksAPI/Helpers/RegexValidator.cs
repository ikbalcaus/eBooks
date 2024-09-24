using System.Text.RegularExpressions;

namespace eBooksAPI.Helpers
{
    public class RegexValidator
    {
        public static bool IsValidEmail(string email)
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }

        public static bool IsValidPassword(string password)
        {
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+[\]{};':""\\|,.<>\/?]).{8,}$");
            return regex.IsMatch(password);
        }
    }
}
