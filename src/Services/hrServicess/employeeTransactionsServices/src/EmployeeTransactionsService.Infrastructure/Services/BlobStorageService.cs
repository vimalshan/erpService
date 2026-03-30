using Azure.Storage.Blobs;
using EmployeeTransactionsService.Application.Contracts;
using Microsoft.Extensions.Configuration;
using Polly;

namespace EmployeeTransactionsService.Infrastructure.Services;

public sealed class BlobStorageService(BlobServiceClient blobServiceClient, IConfiguration configuration, ResiliencePipeline pipeline) : IBlobStorageService
{
    private readonly string _containerName = configuration["BlobStorage:ContainerName"] ?? "stationery-images";

    public async Task<string> UploadAsync(string blobName, byte[] content, string contentType, CancellationToken cancellationToken = default)
    {
        return await pipeline.ExecuteAsync(async token =>
        {
            var container = blobServiceClient.GetBlobContainerClient(_containerName);
            await container.CreateIfNotExistsAsync(cancellationToken: token);
            var blob = container.GetBlobClient(blobName);
            await using var stream = new MemoryStream(content, writable: false);
            await blob.UploadAsync(stream, overwrite: true, token);
            return blob.Uri.ToString();
        }, cancellationToken);
    }
}