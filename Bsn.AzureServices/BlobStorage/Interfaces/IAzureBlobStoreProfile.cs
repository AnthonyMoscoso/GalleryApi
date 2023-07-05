using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.AzureServices.BlobStorage.Interfaces
{
    public interface IAzureBlobStoreProfile
    {
        string? AccountName { get; }
        string? AccountPassword { get; }
        string? Connections { get; }
    }
}
