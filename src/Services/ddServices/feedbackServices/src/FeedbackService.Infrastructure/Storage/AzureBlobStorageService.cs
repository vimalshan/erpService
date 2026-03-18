namespace FeedbackService.Infrastructure.Storage;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

/// <summary>
/// Interface for blob storage operations
/// </summary>
public interface IBlobStorageService
{
    /// <summary>
    /// Uploads a blob
    /// </summary>
    Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a blob
    /// </summary>
    Task<Stream> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a blob
    /// </summary>
    Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a blob URI
    /// </summary>
    Uri GetBlobUri(string containerName, string blobName);
}

/// <summary>
/// Implementation of blob storage service using Azure Blob Storage
/// </summary>
public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;

    /// <summary>
    /// Initializes a new instance of the AzureBlobStorageService class
    /// </summary>
    public AzureBlobStorageService(string connectionString, string containerName)
    {
        var client = new BlobContainerClient(connectionString, containerName);
        _containerClient = client;
    }

    /// <summary>
    /// Uploads a blob to Azure Blob Storage
    /// </summary>
    public async Task<string> UploadAsync(string containerName, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        
        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };

        await blobClient.UploadAsync(content, overwrite: true, cancellationToken: cancellationToken);

        return blobClient.Uri.ToString();
    }

    /// <summary>
    /// Downloads a blob from Azure Blob Storage
    /// </summary>
    public async Task<Stream> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        var download = await blobClient.DownloadAsync(cancellationToken);
        return download.Value.Content;
    }

    /// <summary>
    /// Deletes a blob from Azure Blob Storage
    /// </summary>
    public async Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var blobClient = _containerClient.GetBlobClient(blobName);
        await blobClient.DeleteAsync(cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Gets the URI for a blob
    /// </summary>
    public Uri GetBlobUri(string containerName, string blobName)
    {
        return _containerClient.GetBlobClient(blobName).Uri;
    }
}
