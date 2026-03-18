using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;

namespace Todos.Infrastructure.MessageBrokers;

/// <summary>
/// Interface for Blob Storage operations
/// </summary>
public interface IBlobStorageService
{
    /// <summary>
    /// Uploads a file to blob storage
    /// </summary>
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a file from blob storage
    /// </summary>
    Task<Stream> DownloadAsync(string fileName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a file from blob storage
    /// </summary>
    Task DeleteAsync(string fileName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all files in the container
    /// </summary>
    Task<IEnumerable<string>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a file exists
    /// </summary>
    Task<bool> ExistsAsync(string fileName, CancellationToken cancellationToken = default);
}

/// <summary>
/// Blob Storage service implementation using Azure SDK
/// </summary>
public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(BlobServiceClient blobServiceClient, BlobStorageConfiguration config, ILogger<BlobStorageService> logger)
    {
        _containerClient = blobServiceClient.GetBlobContainerClient(config.ContainerName);
        _logger = logger;
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true, cancellationToken: cancellationToken);
            _logger.LogInformation("File {FileName} uploaded successfully", fileName);
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file {FileName}", fileName);
            throw;
        }
    }

    public async Task<Stream> DownloadAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            var download = await blobClient.DownloadAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("File {FileName} downloaded successfully", fileName);
            return download.Value.Content;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file {FileName}", fileName);
            throw;
        }
    }

    public async Task DeleteAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("File {FileName} deleted successfully", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {FileName}", fileName);
            throw;
        }
    }

    public async Task<IEnumerable<string>> ListAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var files = new List<string>();
            await foreach (var blobItem in _containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
            {
                files.Add(blobItem.Name);
            }
            return files;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing files from blob storage");
            throw;
        }
    }

    public async Task<bool> ExistsAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            return await blobClient.ExistsAsync(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if file {FileName} exists", fileName);
            throw;
        }
    }
}
