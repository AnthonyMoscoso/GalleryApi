using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Return the stream converter on base64
        /// </summary>
        /// <param name="stream">Stream to converter</param>
        /// <returns>string value of stream on base64</returns>
        public static string ConvertToBase64(this Stream stream)
        {
            if (stream is MemoryStream memoryStream)
            {
                return Convert.ToBase64String(memoryStream.ToArray());
            }
            byte[] bytes = new byte[(int)stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);
            return Convert.ToBase64String(bytes);
        }
    }
}
