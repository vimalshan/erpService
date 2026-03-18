using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace LeaveServices.Infrastructure.Storage;

/// <summary>
/// Azure Blob Storage service – used to store attachments (e.g. medical certificates).
/// </summary>
public sealed class BlobStorageService
{
    private readonly BlobServiceClient _client;
    private const string ContainerName = "leave-attachments";

    public BlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration["AzureBlobStorage:ConnectionString"]
            ?? "UseDevelopmentStorage=true";
        _client = new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadAsync(string fileName, Stream content, string contentType, CancellationToken ct = default)
    {
        var container = _client.GetBlobContainerClient(ContainerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobName = $"{Guid.NewGuid():N}_{Path.GetFileName(fileName)}";
        var blob     = container.GetBlobClient(blobName);
        await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);

        return blob.Uri.ToString();
    }

    public async Task DeleteAsync(string blobUri, CancellationToken ct = default)
    {
        var uri  = new Uri(blobUri);
        var name = uri.Segments.Last();
        var container = _client.GetBlobContainerClient(ContainerName);
        await container.GetBlobClient(name).DeleteIfExistsAsync(cancellationToken: ct);
    }
}
