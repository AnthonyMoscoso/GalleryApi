using Azure.Storage.Sas;
using Bsn.AzureServices.Enums;
using Core.Utilities.Enums;

namespace Bsn.AzureServices.BlobStorage.Interfaces
{
    public interface IAzureBlobStorageManager
    {
        /// <summary>
        /// Upload a base64 file to Azure Blob Storage
        /// </summary>
        /// <param name="base64File">Base64 file</param>
        /// <param name="filename">Name to save the file with</param>
        /// <param name="blobContainerNames">Azure Blob Storage container to upload the file to</param>
        /// <returns>Returns true or false if the upload was successfull</returns>
        public Task<bool> Upload(string base64File, string filename, BlobContainers blobContainerNames);
        /// <summary>
        /// Deletes a file from Azure Blob Storage.
        /// </summary>
        /// <param name="blobContainerNames">Azure Blob Storage container where the file is located</param>
        /// <param name="filename">Name of the file to be deleted</param>
        /// <returns>Returns a task that represents the asynchronous operation. The task result is true if the file was successfully deleted, otherwise false.</returns>
        public Task<bool> Delete(BlobContainers blobContainerNames, string filename);
        /// <summary>
        /// Downloads a file from Azure Blob Storage as a Stream.
        /// </summary>
        /// <param name="blobContainerNames">Azure Blob Storage container where the file is located</param>
        /// <param name="filename">Name of the file to be downloaded</param>
        /// <returns>Returns a task that represents the asynchronous operation. The task result is a Stream containing the downloaded file.</returns>
        public Task<Stream> DownloadFile(BlobContainers blobContainerNames, string filename);
        /// <summary>
        /// Downloads a file from Azure Blob Storage as a Base64 string.
        /// </summary>
        /// <param name="blobContainerNames">Azure Blob Storage container where the file is located</param>
        /// <param name="filename">Name of the file to be downloaded</param>
        /// <returns>Returns a task that represents the asynchronous operation. The task result is a Base64 string of the downloaded file.</returns>
        public Task<string> DownloadFileOnBase64(BlobContainers blobContainerNames, string filename);
        /// <summary>
        /// Retrieves a shared access signature (SAS) URL for a file in Azure Blob Storage.
        /// </summary>
        /// <param name="blobContainerNames">Azure Blob Storage container where the file is located</param>
        /// <param name="filename">Name of the file to generate the SAS URL for</param>
        /// <returns>Returns a task that represents the asynchronous operation. The task result is a string containing the SAS URL for the file.</returns>
        public Task<string> GetSasUrl(BlobContainers blobContainerNames, string filename, BlobSasPermissions blobSasPermissions = BlobSasPermissions.Read, BlobResources blobResources = BlobResources.Blob, string? storedPolicyName = null, DateTimeOffset? expiresOn = null);
    }
}
