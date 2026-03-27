using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TransactionProcessing.Domain.Interfaces;

namespace TransactionProcessing.Infrastructure.BlobStorage;

public sealed class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<AzureBlobStorageService> _logger;

    public AzureBlobStorageService(IConfiguration configuration, ILogger<AzureBlobStorageService> logger)
    {
        var connectionString = configuration.GetConnectionString("BlobStorage")
            ?? "UseDevelopmentStorage=true";
        _blobServiceClient = new BlobServiceClient(connectionString);
        _logger = logger;
    }

    public async Task<string> UploadAsync(string containerName, string fileName, Stream content, CancellationToken ct = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);
        var blob = container.GetBlobClient(fileName);
        await blob.UploadAsync(content, overwrite: true, ct);
        _logger.LogInformation("Uploaded blob {FileName} to {Container}", fileName, containerName);
        return blob.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string containerName, string fileName, CancellationToken ct = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(fileName);
        if (!await blob.ExistsAsync(ct)) return null;
        var response = await blob.DownloadStreamingAsync(cancellationToken: ct);
        return response.Value.Content;
    }

    public async Task<IReadOnlyList<string>> ListAsync(string containerName, string? prefix = null, CancellationToken ct = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);
        if (!await container.ExistsAsync(ct)) return [];
        var results = new List<string>();
        await foreach (var blob in container.GetBlobsAsync(BlobTraits.None, BlobStates.None, prefix, ct))
            results.Add(blob.Name);
        return results;
    }
}
