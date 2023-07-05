using Bsn.Utilities.Configuration.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.Utilities.Configuration
{
    public class Keys : IKeys
    {
        private readonly IAzureKeys _azureKeys;
     
        private readonly IJWTKeys _jWTKeys;
        public Keys(IAzureKeys azureKeys, IJWTKeys jWTKeys) 
        { 
            _azureKeys = azureKeys;
            _jWTKeys = jWTKeys;
        }

        public IAzureKeys Azure => _azureKeys;
        public IJWTKeys JWT => _jWTKeys;
    }
}
