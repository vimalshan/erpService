namespace AdminService.Infrastructure.Azure;

/// <summary>
/// No-op (no operation) implementation of blob storage service for development without Azure
/// </summary>
public class NoOpBlobStorageService : IBlobStorageService
{
    /// <summary>
    /// Upload a blob (no-op in development)
    /// </summary>
    public Task<string> UploadAsync(string containerName, string blobName, Stream content, CancellationToken cancellationToken = default)
    {
        return Task.FromResult($"[DEV] Blob would be uploaded to {containerName}/{blobName}");
    }

    /// <summary>
    /// Download a blob (no-op in development)
    /// </summary>
    public Task<Stream> DownloadAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        var stream = new MemoryStream();
        return Task.FromResult((Stream)stream);
    }

    /// <summary>
    /// Delete a blob (no-op in development)
    /// </summary>
    public Task DeleteAsync(string containerName, string blobName, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Get blob URI (no-op in development)
    /// </summary>
    public Task<Uri> GetBlobUriAsync(string containerName, string blobName)
    {
        return Task.FromResult(new Uri($"https://dev.blob.core.windows.net/{containerName}/{blobName}"));
    }
}
