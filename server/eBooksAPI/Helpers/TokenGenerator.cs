using System.Security.Cryptography;
using System.Text;

namespace BaseCureAPI.Helpers
{
    public class TokenGenerator
    {
        public static string GenerateToken(int size = 20)
        {
            var charSet = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var chars = charSet.ToCharArray();
            var data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(size);
            foreach (var i in data)
            {
                result.Append(chars[i % (chars.Length)]);
            }
            return result.ToString();
        }
    }
}
