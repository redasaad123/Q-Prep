using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class GenerateCodeVerify
    {
        public static string GenerateCode(int length = 6)
        {
            const string chars = "0123456789";
            Random random = new Random();
            StringBuilder code = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                code.Append(chars[random.Next(chars.Length)]);
            }
            return code.ToString();
        }
    }
}
