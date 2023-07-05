using Core.Utilities.Ensures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities
{
    public static class Base64
    {
        /// <summary>
        /// Text to encode on base64
        /// </summary>
        /// <param name="plainText">text to encode</param>
        /// <returns>string encode on base64</returns>
        public static string Encode(string plainText)
        {
            Ensure.That(plainText,nameof(plainText)).NotNullOrEmpty();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        /// <summary>
        /// Text on base to decode on string
        /// </summary>
        /// <param name="base64EncodedData">text on base64 to decode</param>
        /// <returns>string decode</returns>
        public static string Decode(string base64EncodedData)
        {
            try
            {
                Ensure.That(base64EncodedData, nameof(base64EncodedData)).NotNullOrEmpty();
                byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception)
            {
                return base64EncodedData;
            }
 
        }
        public static bool TryDecode(string base64EncodedData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(base64EncodedData))
                {
                    return false;
                }
                byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
                string val = Encoding.UTF8.GetString(base64EncodedBytes);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Convert 64 string to stream
        /// </summary>
        /// <param name="base64File">base64 text to convert</param>
        /// <returns>stream from base64</returns>
        public static Stream ConvertToStream(string base64File)
        {
            Ensure.That(base64File, nameof(base64File)).NotNullOrEmpty();
            byte[] newBytes = Convert.FromBase64String(base64File);
            Stream stream = new MemoryStream(newBytes);
            return stream;
        }
    }
}
