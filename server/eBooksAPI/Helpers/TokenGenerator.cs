using eBooksAPI.Database;
using System.Text;

namespace eBooksAPI.Helpers
{
    public class TokenGenerator
    {
        private static readonly string charSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random random = new Random();

        public static string GenerateUniqueToken(eBooksContext db, int length = 20)
        {
            string token = GenerateToken(length);
            while (db.AuthTokens.Any(x => x.Token == token))
            {
                token = GenerateToken(length);
            }
            return token;
        }

        private static string GenerateToken(int length)
        {
            var token = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(charSet.Length);
                token.Append(charSet[index]);
            }
            return token.ToString();
        }
    }
}
