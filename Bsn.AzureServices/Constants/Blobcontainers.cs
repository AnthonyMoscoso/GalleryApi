using Bsn.AzureServices.Enums;
using Bsn.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.AzureServices.Constants
{
    public static class Collections
    {
        public static readonly IDictionary<BlobContainers, string> BlobContainer = new Dictionary<BlobContainers, string>()
        {
             {BlobContainers.images_container, BlobContainersNames.image_container },
               {BlobContainers.user_profile, BlobContainersNames.user_profile },
        };

        public static readonly IDictionary<BlobResources, string> BlobResource = new Dictionary<BlobResources, string>()
        {
            { BlobResources.Blob, "b"},
              { BlobResources.Container, "c"},
                { BlobResources.Snapshot, "bs"},
                  { BlobResources.BlobVersion, "bv"},
        };
    }
}
