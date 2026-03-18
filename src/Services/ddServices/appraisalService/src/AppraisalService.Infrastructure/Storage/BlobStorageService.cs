using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AppraisalService.Infrastructure.Storage;

/// <summary>
/// Azure Blob Storage service for managing appraisal documents and stationery images
/// </summary>
public interface IBlobStorageService
{
    Task<string> UploadAsync(string containerName, string fileName, Stream fileStream, CancellationToken cancellationToken = default);
    Task<Stream> DownloadAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
    Task DeleteAsync(string containerName, string fileName, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> ListBlobsAsync(string containerName, CancellationToken cancellationToken = default);
    Task<Uri> GetBlobUriAsync(string containerName, string fileName);
}

/// <summary>
/// Azure Blob Storage implementation
/// </summary>
public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(BlobServiceClient blobServiceClient, ILogger<BlobStorageService> logger)
    {
        _blobServiceClient = blobServiceClient;
        _logger = logger;
    }

    public async Task<string> UploadAsync(
        string containerName,
        string fileName,
        Stream fileStream,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true, cancellationToken: cancellationToken);

            _logger.LogInformation($"File {fileName} uploaded to {containerName}");
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error uploading file {fileName} to {containerName}");
            throw;
        }
    }

    public async Task<Stream> DownloadAsync(
        string containerName,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            var download = await blobClient.DownloadAsync(cancellationToken: cancellationToken);
            return download.Value.Content;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error downloading file {fileName} from {containerName}");
            throw;
        }
    }

    public async Task DeleteAsync(
        string containerName,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.DeleteAsync(cancellationToken: cancellationToken);
            _logger.LogInformation($"File {fileName} deleted from {containerName}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting file {fileName} from {containerName}");
            throw;
        }
    }

    public async Task<IEnumerable<string>> ListBlobsAsync(
        string containerName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = new List<string>();

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
            {
                blobs.Add(blobItem.Name);
            }

            return blobs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error listing blobs in {containerName}");
            throw;
        }
    }

    public async Task<Uri> GetBlobUriAsync(string containerName, string fileName)
    {
        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            return blobClient.Uri;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting URI for {fileName} in {containerName}");
            throw;
        }
    }
}
