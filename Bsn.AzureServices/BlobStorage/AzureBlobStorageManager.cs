using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Bsn.AzureServices.BlobStorage.Interfaces;
using Bsn.AzureServices.Constants;
using Bsn.AzureServices.Enums;
using Bsn.Utilities.Constants;
using Core.Utilities;
using Core.Utilities.Constants;
using Core.Utilities.Ensures;
using Core.Utilities.Exceptions;
using Core.Utilities.Extensions;

namespace Bsn.AzureServices.BlobStorage
{
    public class AzureBlobStorageManager : IAzureBlobStorageManager
    {
        private readonly IAzureBlobStoreProfile _azureBlobStoreProfile;
       
        public AzureBlobStorageManager(IAzureBlobStoreProfile azureBlobStoreProfile)
        {
            _azureBlobStoreProfile = azureBlobStoreProfile;
        }
        public async Task<bool> Delete(BlobContainers blobContainerNames, string filename)
        {
            Ensure.That(blobContainerNames, nameof(blobContainerNames)).IsNotNull();
            Ensure.That(filename, nameof(filename)).NotNullOrEmpty();

            BlobServiceClient blobServiceClient = new(_azureBlobStoreProfile.Connections);
            BlobContainerClient cont = blobServiceClient.GetBlobContainerClient(GetContainerName(blobContainerNames));
            BlobClient blobClient = cont.GetBlobClient(filename);
            bool result = await blobClient.DeleteIfExistsAsync();
            return result;
        }

        public async Task<string> DownloadFileOnBase64(BlobContainers blobContainerNames, string filename)
        {
            Ensure.That(blobContainerNames, nameof(blobContainerNames)).IsNotNull();
            Ensure.That(filename, nameof(filename)).NotNullOrEmpty();

            Stream stream = await DownloadFile(blobContainerNames, filename);
            string streamInbase64 = stream.ConvertToBase64();
            return streamInbase64;
        }
        public async Task<Stream> DownloadFile(BlobContainers blobContainerNames, string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);
            BlobServiceClient blobServiceClient = new(_azureBlobStoreProfile.Connections);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(GetContainerName(blobContainerNames));
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            Stream stream = await blobClient.OpenReadAsync();
            return stream;
        }
        public async Task<string> GetSasUrl(BlobContainers blobContainerNames, string filename, BlobSasPermissions blobSasPermissions = BlobSasPermissions.Read, BlobResources blobResources = BlobResources.Blob, string? storedPolicyName = null, DateTimeOffset? expiresOn = null)
        {
            Ensure.That(blobContainerNames, nameof(blobContainerNames)).IsNotNull();
            Ensure.That(filename, nameof(filename)).NotNullOrEmpty();

            BlobServiceClient blobServiceClient = new(_azureBlobStoreProfile.Connections);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(GetContainerName(blobContainerNames));
            BlobClient blobClient = blobContainerClient.GetBlobClient(filename);

            UnauthorizedException.ThrowIfFalse(blobClient.CanGenerateSasUri, ErrorMessage.UnauthorizedBlobClient);
            Constants.Collections.BlobResource.TryGetValue(blobResources, out string? resource);
            Ensure.That(resource, nameof(resource)).NotNullOrEmpty();

            BlobSasBuilder sasBuilder = new()
            {
                BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                BlobName = blobClient.Name,
                Resource = resource
            };

            if (storedPolicyName == null)
            {
                sasBuilder.ExpiresOn = expiresOn ?? DateTimeOffset.Now.AddHours(1);
                sasBuilder.SetPermissions(blobSasPermissions);
            }
            else
            {
                sasBuilder.Identifier = storedPolicyName;
            }

            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);

            return await Task.FromResult(sasUri.ToString());
        }

        public async Task<bool> Upload(string base64File, string filename, BlobContainers blobContainerNames)
        {
            Ensure.That(filename, nameof(filename)).NotNullOrEmpty();
            Ensure.That(base64File, nameof(base64File)).NotNullOrEmpty();
            Ensure.That(blobContainerNames, nameof(blobContainerNames)).IsNotNull();
  
            string contentType =  FileMIME.GetMimeFromFileString(filename);
            contentType = string.IsNullOrWhiteSpace(contentType)? Constant.ApplicationOctetStream : contentType;
            BlobContainerClient containerClient = new(_azureBlobStoreProfile.Connections, GetContainerName(blobContainerNames));
            Stream stream = Base64.ConvertToStream(base64File);
          
            await containerClient.DeleteBlobIfExistsAsync(filename);
            await containerClient.UploadBlobAsync(filename, stream);
            var blobClient = containerClient.GetBlobClient(filename);
            var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };
            await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
            return true;
        }

       

        private static string GetContainerName(BlobContainers blobContainers)
        {
            Constants.Collections.BlobContainer.TryGetValue(blobContainers, out string? container);

            Ensure.That(container, nameof(container)).NotNullOrEmpty();
            return container!;
        }

       

       
    }
}
