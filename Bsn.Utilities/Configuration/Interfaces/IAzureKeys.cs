using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.Utilities.Configuration.Interfaces
{
    public interface IAzureKeys
    {
        /// <summary>
        /// Gets the storage key used for Azure storage.
        /// </summary>
        string? StorageKey { get; }

        /// <summary>
        /// Gets the storage account name used for Azure storage.
        /// </summary>
        string? StorageAccount { get; }

        /// <summary>
        /// Gets the storage URI used for Azure storage.
        /// </summary>
        string? StorageUri { get; }
    }
}
