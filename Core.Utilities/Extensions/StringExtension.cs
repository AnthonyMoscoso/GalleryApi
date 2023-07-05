using Core.Utilities.Ensures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Extensions
{
    public static class StringExtension
    {
        public static byte[] Encoding_UTF8(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }
        public static bool IsBase64String(this string value)
        {
       
            Span<byte> bytes = stackalloc byte[(int)(value.Length * 0.75)];
            return Convert.TryFromBase64String(value, bytes, out int bytesWritten);
        }

    }
}
