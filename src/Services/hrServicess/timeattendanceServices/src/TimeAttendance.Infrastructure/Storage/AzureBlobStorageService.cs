using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TimeAttendance.Domain.Interfaces;

namespace TimeAttendance.Infrastructure.Storage;

public class BlobStorageOptions
{
    public const string SectionName = "BlobStorage";
    public string ConnectionString { get; set; } = string.Empty;
    public string DefaultContainer { get; set; } = "timeattendance";
}

public class AzureBlobStorageService(
    IOptions<BlobStorageOptions> options,
    ILogger<AzureBlobStorageService> logger) : IBlobStorageService
{
    private readonly BlobServiceClient _serviceClient = new(options.Value.ConnectionString);

    public async Task<string> UploadAsync(
        string containerName, string blobName, Stream content,
        string contentType, CancellationToken cancellationToken = default)
    {
        var container = _serviceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

        var blobClient = container.GetBlobClient(blobName);
        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };

        await blobClient.UploadAsync(content, uploadOptions, cancellationToken);
        logger.LogInformation("Blob '{BlobName}' uploaded to '{Container}'", blobName, containerName);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(
        string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var blobClient = _serviceClient
            .GetBlobContainerClient(containerName)
            .GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync(cancellationToken))
            return null;

        var response = await blobClient.DownloadStreamingAsync(cancellationToken: cancellationToken);
        return response.Value.Content;
    }

    public async Task DeleteAsync(
        string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var blobClient = _serviceClient
            .GetBlobContainerClient(containerName)
            .GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        logger.LogInformation("Blob '{BlobName}' deleted from '{Container}'", blobName, containerName);
    }

    public async Task<string> GetSasUrlAsync(
        string containerName, string blobName, TimeSpan expiry, CancellationToken cancellationToken = default)
    {
        var blobClient = _serviceClient
            .GetBlobContainerClient(containerName)
            .GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync(cancellationToken))
            throw new FileNotFoundException($"Blob '{blobName}' not found in '{containerName}'.");

        var sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(expiry));
        return sasUri.ToString();
    }
}
