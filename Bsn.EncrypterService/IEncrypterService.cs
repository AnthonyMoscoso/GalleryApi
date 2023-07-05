using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.EncrypterService
{
    public interface IEncrypterService
    {
        string Encrypt(string value);
        string Decrypt(string value);
    }
}
