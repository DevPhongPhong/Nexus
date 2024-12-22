using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Nexus
{
    public class Common
    {
        public static string ToSHA256HashString(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                var builder = new System.Text.StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
