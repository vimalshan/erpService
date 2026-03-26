using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EmployeeService.Infrastructure.Storage;

/// <summary>Azure Blob Storage service for employee-related file uploads.</summary>
public sealed class BlobStorageService
{
    private readonly BlobServiceClient? _client;
    private readonly ILogger<BlobStorageService> _logger;
    private const string ContainerName = "employee-documents";

    public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
    {
        _logger = logger;
        var connectionString = configuration["AzureStorage:ConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            _logger.LogWarning("AzureStorage:ConnectionString is not configured. Blob storage will be unavailable.");
            return;
        }
        try
        {
            _client = new BlobServiceClient(connectionString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialise BlobServiceClient. Blob storage will be unavailable.");
        }
    }

    private void EnsureAvailable()
    {
        if (_client is null)
            throw new InvalidOperationException("Blob storage is not available: AzureStorage:ConnectionString is missing or invalid.");
    }

    public async Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken ct = default)
    {
        EnsureAvailable();
        var containerClient = _client!.GetBlobContainerClient(ContainerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobName = $"{Guid.NewGuid()}/{Path.GetFileName(fileName)}";
        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        _logger.LogInformation("Uploaded blob: {BlobName}", blobName);

        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default)
    {
        EnsureAvailable();
        var containerClient = _client!.GetBlobContainerClient(ContainerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        var response = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string blobName, CancellationToken ct = default)
    {
        EnsureAvailable();
        var containerClient = _client!.GetBlobContainerClient(ContainerName);
        await containerClient.GetBlobClient(blobName).DeleteIfExistsAsync(cancellationToken: ct);
        _logger.LogInformation("Deleted blob: {BlobName}", blobName);
    }
}
