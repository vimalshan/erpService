namespace AccessService.Infrastructure.BlobStorage
{
    /// <summary>
    /// Interface for Azure Blob Storage operations
    /// </summary>
    public interface IAzureBlobStorageService
    {
        /// <summary>
        /// Upload a blob to Azure Storage
        /// </summary>
        Task<string> UploadBlobAsync(string blobName, Stream content, string contentType = "application/octet-stream");

        /// <summary>
        /// Download a blob from Azure Storage
        /// </summary>
        Task<Stream> DownloadBlobAsync(string blobName);

        /// <summary>
        /// Delete a blob from Azure Storage
        /// </summary>
        Task DeleteBlobAsync(string blobName);

        /// <summary>
        /// Check if a blob exists
        /// </summary>
        Task<bool> BlobExistsAsync(string blobName);

        /// <summary>
        /// Get all blobs in the container
        /// </summary>
        Task<IEnumerable<string>> ListBlobsAsync(string prefix = null);

        /// <summary>
        /// Get a shared access signature URL for a blob
        /// </summary>
        Task<string> GetBlobSasUrlAsync(string blobName, TimeSpan expirationTime);

        /// <summary>
        /// Check connectivity to Azure Storage
        /// </summary>
        Task<bool> IsConnectedAsync();
    }
}
