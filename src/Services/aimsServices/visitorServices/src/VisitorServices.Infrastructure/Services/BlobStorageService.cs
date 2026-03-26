using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using VisitorServices.Application.Common.Interfaces;

namespace VisitorServices.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private const string ContainerName = "visitor-attachments";
    private readonly BlobServiceClient? _blobServiceClient;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(ILogger<BlobStorageService> logger, BlobServiceClient? blobServiceClient = null)
    {
        _logger = logger;
        _blobServiceClient = blobServiceClient;

        if (_blobServiceClient is null)
            _logger.LogWarning("Azure Blob Storage is not configured. Blob operations will fail.");
    }

    private BlobServiceClient GetClient() =>
        _blobServiceClient ?? throw new InvalidOperationException(
            "Azure Blob Storage is not configured. Set ConnectionStrings:AzureStorage.");

    public async Task<string> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        var containerClient = GetClient().GetBlobContainerClient(ContainerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var blobName = $"{Guid.NewGuid()}/{Path.GetFileName(fileName)}";
        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(
            fileStream,
            new BlobHttpHeaders { ContentType = contentType },
            cancellationToken: cancellationToken);

        return blobName;
    }

    public async Task DeleteAsync(string blobName, CancellationToken cancellationToken = default)
    {
        var containerClient = GetClient().GetBlobContainerClient(ContainerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<Stream> DownloadAsync(string blobName, CancellationToken cancellationToken = default)
    {
        var containerClient = GetClient().GetBlobContainerClient(ContainerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        var response = await blobClient.DownloadAsync(cancellationToken);
        return response.Value.Content;
    }
}
