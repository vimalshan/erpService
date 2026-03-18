using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using RecruitmentService.Application.Interfaces;

namespace RecruitmentService.Infrastructure.ExternalServices.BlobStorage;

public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public AzureBlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration["AzureBlobStorage:ConnectionString"]
            ?? throw new InvalidOperationException("AzureBlobStorage:ConnectionString is not configured.");
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadAsync(Stream content, string fileName, string containerName, CancellationToken ct = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: ct);

        var blob = container.GetBlobClient(fileName);
        await blob.UploadAsync(content, overwrite: true, ct);
        return blob.Uri.ToString();
    }

    public async Task<Stream> DownloadAsync(string fileName, string containerName, CancellationToken ct = default)
    {
        var blob = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(fileName);
        var response = await blob.DownloadAsync(ct);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string fileName, string containerName, CancellationToken ct = default)
    {
        var blob = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(fileName);
        await blob.DeleteIfExistsAsync(cancellationToken: ct);
    }

    public async Task<bool> ExistsAsync(string fileName, string containerName, CancellationToken ct = default)
    {
        var blob = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(fileName);
        return await blob.ExistsAsync(ct);
    }

    public string GetPublicUri(string fileName, string containerName)
    {
        var blob = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(fileName);
        return blob.Uri.ToString();
    }
}
