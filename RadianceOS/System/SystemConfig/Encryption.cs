using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.SystemConfig
{
    public static class Encryption
    {
        // Not the most secure method of encryption in the world but it's the best we have so far.
        public static byte[] EncryptBytes(byte[] data, int key)
        {
            byte[] encrypted = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                encrypted[i] = unchecked((byte)(data[i] + key));
            }

            return encrypted;
        }

        public static byte[] DecryptBytes(byte[] data, int key)
        {
            byte[] decrypted = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                decrypted[i] = unchecked((byte)(data[i] - key));
            }

            return decrypted;
        }

        public static byte[] StringToBytes(string str, Encoding encoding = default) => encoding == default ? Encoding.UTF8.GetBytes(str) : encoding.GetBytes(str);
        public static string BytesToString(byte[] bytes, Encoding encoding = default) => encoding == default ? Encoding.UTF8.GetString(bytes) : encoding.GetString(bytes);
    }
}
