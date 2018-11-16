using System;
using System.Text;

namespace Todo.Core.Extensions
{
    public static class StringExtensions
    {
        public static string Decode(this string text)
        {
            byte[] data = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(data);
        }


        public static string Encode(this string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(bytes);
        }

        public static bool TryDecode(string encodedText, out string plainText)
        {
            plainText = string.Empty;
            try
            {
                plainText = Decode(encodedText);
                return true;
            }
            catch (Exception)
            {
            }

            return false;
        }
    }
}
