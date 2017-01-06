using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Token
{
    public class TokenFactory
    {
        private DateTime TokenExpiration { get; set; }
        public TokenFactory(DateTime tokenExpiration)
        {
            this.TokenExpiration = tokenExpiration;
        }

        public Token GenerateToken()
        {
            string tokenString = "";
            var token = new Token();

            tokenString = GenerateRandomString(10) + (new Random().NextDouble() * 12570).ToString() + "-"
                + DateTime.Now.Millisecond.ToString() + DateTime.UtcNow.Second.ToString()
                + GenerateRandomString(7) + (new Random().NextDouble() * 1000).ToString() + "-"
                + Math.Log10(DateTime.UtcNow.Millisecond) + DateTime.Now.Minute * (new Random().NextDouble())
                + GenerateRandomString(12);

            TimeSpan span = TokenExpiration.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));

            token.ExpirationTime = span.TotalMilliseconds;
            token.TokenString = tokenString;

            return token;
        }

        private string GenerateRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;
        }
    }
}
