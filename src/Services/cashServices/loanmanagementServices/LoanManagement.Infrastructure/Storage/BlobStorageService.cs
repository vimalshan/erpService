using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LoanManagement.Infrastructure.Storage;

public class BlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<BlobStorageService> _logger;
    private const string ContainerName = "loan-documents";

    public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
    {
        var connectionString = configuration["AzureStorage:ConnectionString"]
            ?? throw new InvalidOperationException("Azure Storage connection string not configured.");
        _blobServiceClient = new BlobServiceClient(connectionString);
        _logger = logger;
    }

    public async Task<string> UploadDocumentAsync(
        Stream content, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(ContainerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var blobName = $"{Guid.NewGuid()}/{fileName}";
        var blobClient = container.GetBlobClient(blobName);

        await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);

        _logger.LogInformation("Uploaded document {BlobName}", blobName);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadDocumentAsync(string blobUri, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(blobUri);
        var blobClient = new BlobClient(uri);
        var response = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
        return response.Value.Content;
    }

    public async Task DeleteDocumentAsync(string blobName, CancellationToken cancellationToken = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(ContainerName);
        await container.DeleteBlobIfExistsAsync(blobName, cancellationToken: cancellationToken);
        _logger.LogInformation("Deleted document {BlobName}", blobName);
    }
}
