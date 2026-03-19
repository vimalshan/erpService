using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace ClubMembershipService.Infrastructure.Services;

public class BlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private const string ContainerName = "club-images";

    public BlobStorageService(IConfiguration configuration)
    {
        var connectionString = configuration["Azure:BlobStorage:ConnectionString"]
            ?? "UseDevelopmentStorage=true";
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadImageAsync(Stream imageStream, string fileName, string contentType)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(imageStream, new BlobHttpHeaders { ContentType = contentType });
        return blobClient.Uri.AbsoluteUri;
    }

    public async Task DeleteImageAsync(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
        await containerClient.GetBlobClient(fileName).DeleteIfExistsAsync();
    }

    public async Task<Stream?> DownloadImageAsync(string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
        var blobClient = containerClient.GetBlobClient(fileName);
        if (!await blobClient.ExistsAsync()) return null;
        var response = await blobClient.DownloadAsync();
        return response.Value.Content;
    }
}
