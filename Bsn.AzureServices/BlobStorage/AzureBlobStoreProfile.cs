using Bsn.AzureServices.BlobStorage.Interfaces;
using Bsn.Utilities.Configuration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.AzureServices.BlobStorage
{
    public class AzureBlobStoreProfile : IAzureBlobStoreProfile
    {
        private readonly IKeys _keys;
        public AzureBlobStoreProfile(IKeys keys) 
        {  
            _keys = keys;
        }
        public string? AccountName => _keys.Azure.StorageAccount;

        public string? AccountPassword => _keys.Azure.StorageKey;

        public string? Connections => string.IsNullOrWhiteSpace( _keys.Azure.StorageUri) ? string.Empty : string.Format(_keys.Azure.StorageUri, AccountName, AccountPassword);
    }
}
