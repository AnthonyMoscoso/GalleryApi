using Bsn.Utilities.Configuration.Interfaces;
using Bsn.Utilities.Constants;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.Utilities.Configuration
{
    public class AzureKeys : IAzureKeys
    {
        private readonly IConfiguration _configuration;
     
        public AzureKeys(IConfiguration configuration) 
        { 
            _configuration = configuration;
        }

        #region BLOB STORAGE
        public string? StorageKey => _configuration[AppSettings.storage_key];

        public string? StorageAccount => _configuration[AppSettings.storage_account];

        public string? StorageUri => _configuration[AppSettings.storage_uri];
        #endregion
    }
}
