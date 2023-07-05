
using Core.Utilities.Ensures;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.EncrypterService
{
    public class EncrypterManager : IEncrypterService
    {
        private readonly IConfiguration _config;
        public EncrypterManager(IConfiguration config)
        {
            _config = config;
        }
        private string? KEY => _config["EncrypterKey"];

        public string Encrypt(string toEncrypt)
        {
            try
            {
                Ensure.That(toEncrypt).NotNullOrEmpty();
                Ensure.That(KEY).NotNullOrEmpty();
                byte[] iv = new byte[16];
                byte[] array;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(KEY!);
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using MemoryStream memoryStream = new();
                    using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
                    using (StreamWriter streamWriter = new(cryptoStream))
                    {
                        streamWriter.Write(toEncrypt);
                    }

                    array = memoryStream.ToArray();
                }

                return Convert.ToBase64String(array);
            }
            catch (Exception)
            {
                return toEncrypt;
            }
        }



        public string Decrypt(string value)
        {
            try
            {
                Ensure.That(KEY).NotNullOrEmpty();
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(value);

                using Aes aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(KEY!);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new(buffer);
                using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new(cryptoStream);
                return streamReader.ReadToEnd();
            }
            catch (Exception)
            {
                return value;
            }
        }
    }
}
