using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using UserSecurityService.Application.Common;

namespace UserSecurityService.Infrastructure.Services;

public sealed class BlobStorageService(IConfiguration configuration) : IBlobStorageService
{
    private string ConnectionString => configuration["AzureStorage:ConnectionString"]!;

    public async Task<string> UploadAsync(
        string containerName, string fileName, Stream content,
        string contentType, CancellationToken ct = default)
    {
        var serviceClient = new BlobServiceClient(ConnectionString);
        var containerClient = serviceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);

        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        return blobClient.Uri.ToString();
    }

    public async Task DeleteAsync(string containerName, string blobName, CancellationToken ct = default)
    {
        var serviceClient = new BlobServiceClient(ConnectionString);
        var containerClient = serviceClient.GetBlobContainerClient(containerName);
        await containerClient.DeleteBlobIfExistsAsync(blobName, cancellationToken: ct);
    }
}
