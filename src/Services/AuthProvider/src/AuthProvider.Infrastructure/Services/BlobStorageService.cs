using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthProvider.Infrastructure.Services;

/// <summary>
/// Azure Blob Storage service – used to store user profile images and audit exports.
/// </summary>
public sealed class BlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<BlobStorageService> _logger;
    private const string ProfileImagesContainer = "auth-profile-images";
    private const string AuditExportsContainer = "auth-audit-exports";

    public BlobStorageService(IConfiguration config, ILogger<BlobStorageService> logger)
    {
        var connectionString = config["AzureStorage:ConnectionString"]
            ?? "UseDevelopmentStorage=true";
        _blobServiceClient = new BlobServiceClient(connectionString);
        _logger = logger;
    }

    public async Task<string> UploadProfileImageAsync(
        Guid userId, Stream imageStream, string contentType, CancellationToken ct = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(ProfileImagesContainer);
        await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobName = $"{userId}/profile-{DateTime.UtcNow:yyyyMMddHHmmss}";
        var blobClient = container.GetBlobClient(blobName);

        await blobClient.UploadAsync(imageStream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);

        _logger.LogInformation("Uploaded profile image for user {UserId} to blob {BlobName}", userId, blobName);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadProfileImageAsync(Guid userId, string blobName, CancellationToken ct = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(ProfileImagesContainer);
        var blobClient = container.GetBlobClient($"{userId}/{blobName}");

        if (!await blobClient.ExistsAsync(ct)) return null;

        var download = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        return download.Value.Content;
    }

    public async Task<string> UploadAuditExportAsync(
        string fileName, Stream data, CancellationToken ct = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(AuditExportsContainer);
        await container.CreateIfNotExistsAsync(cancellationToken: ct);

        var blobClient = container.GetBlobClient(fileName);
        await blobClient.UploadAsync(data, overwrite: true, cancellationToken: ct);

        _logger.LogInformation("Audit export uploaded: {FileName}", fileName);
        return blobClient.Uri.ToString();
    }
}
