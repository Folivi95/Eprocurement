using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EGPS.Application.Helpers
{
    public class TransactionHelper
    {
        public static string GenerateRandomNumber(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            string s;
            for (int i = 0; i < size; i++)
            {
                s = Convert.ToString(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(s);
            }

            return builder.ToString();
        }

        public static string ComputeSHAHash(string data)
        {
            using (SHA256 sHA256 = SHA256.Create())
            {
                //compute hash returns the byte array
                byte[] bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(data));

                //convert byte array to a string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public static string ComputeSHA256Hash(string data)
        {
            SHA256Managed sha256 = new SHA256Managed();
            byte[] EncryptedSHA256 = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            sha256.Clear();
            string hashed = BitConverter.ToString(EncryptedSHA256).Replace("-", "").ToLower();
            return hashed;
        }
    }
}
