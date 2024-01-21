using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Security
{
    public static class Encryption
    {
        // Some basic encryption, can be upgraded later
        // Using a basic byte Caesar cypher for this

        public static string EncryptString(string value, int key)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            byte[] encrypted = new byte[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
            {
                encrypted[i] = (byte)(bytes[i] ^ key);
            }

            return Encoding.UTF8.GetString(encrypted);
        }

        // Absolutely no clue why, but the method I was using works both ways but it doesn't make sense without this function so I decided to add it anyway because why not
        public static string DecryptString(string value, int key) => EncryptString(value, key);
    }
}
