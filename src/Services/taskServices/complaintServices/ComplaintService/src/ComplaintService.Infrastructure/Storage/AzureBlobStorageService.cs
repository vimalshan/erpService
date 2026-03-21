using ComplaintService.Application.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace ComplaintService.Infrastructure.Storage;

public class AzureBlobStorageService(IConfiguration configuration) : IBlobStorageService
{
    private readonly BlobContainerClient _container = new(
        configuration["AzureStorage:ConnectionString"],
        configuration["AzureStorage:ContainerName"] ?? "complaint-attachments");

    public async Task<string> UploadFileAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default)
    {
        await _container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: ct);
        var blobName = $"{Guid.NewGuid()}-{SanitizeFileName(fileName)}";
        var blob = _container.GetBlobClient(blobName);
        await blob.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        return blobName;
    }

    public async Task<Stream> DownloadFileAsync(string blobName, CancellationToken ct = default)
    {
        var blob = _container.GetBlobClient(blobName);
        var download = await blob.DownloadStreamingAsync(cancellationToken: ct);
        return download.Value.Content;
    }

    public async Task DeleteFileAsync(string blobName, CancellationToken ct = default)
    {
        var blob = _container.GetBlobClient(blobName);
        await blob.DeleteIfExistsAsync(cancellationToken: ct);
    }

    private static string SanitizeFileName(string fileName) =>
        Path.GetFileName(fileName).Replace(" ", "_");
}
