using Azure.Storage.Blobs;

namespace ApiGateway.API.BlobStorage;

public interface IGatewayBlobService
{
    Task<string> UploadLogAsync(string fileName, Stream content, CancellationToken ct = default);
    Task<Stream?> DownloadLogAsync(string fileName, CancellationToken ct = default);
    Task<IReadOnlyList<string>> ListLogsAsync(string? prefix = null, CancellationToken ct = default);
}

public sealed class AzureGatewayBlobService : IGatewayBlobService
{
    private readonly BlobContainerClient _container;
    private readonly ILogger<AzureGatewayBlobService> _logger;

    public AzureGatewayBlobService(IConfiguration configuration, ILogger<AzureGatewayBlobService> logger)
    {
        _logger = logger;
        var connectionString = configuration["Azure:BlobStorage:ConnectionString"] ?? "UseDevelopmentStorage=true";
        var containerName = configuration["Azure:BlobStorage:ContainerName"] ?? "gateway-logs";
        _container = new BlobContainerClient(connectionString, containerName);
    }

    public async Task<string> UploadLogAsync(string fileName, Stream content, CancellationToken ct = default)
    {
        try
        {
            await _container.CreateIfNotExistsAsync(cancellationToken: ct);
            var blobClient = _container.GetBlobClient(fileName);
            await blobClient.UploadAsync(content, overwrite: true, cancellationToken: ct);
            _logger.LogInformation("Uploaded gateway log: {FileName}", fileName);
            return blobClient.Uri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to upload gateway log: {FileName}", fileName);
            return string.Empty;
        }
    }

    public async Task<Stream?> DownloadLogAsync(string fileName, CancellationToken ct = default)
    {
        try
        {
            var blobClient = _container.GetBlobClient(fileName);
            var response = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
            return response.Value.Content;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to download gateway log: {FileName}", fileName);
            return null;
        }
    }

    public async Task<IReadOnlyList<string>> ListLogsAsync(string? prefix = null, CancellationToken ct = default)
    {
        var names = new List<string>();
        try
        {
            await foreach (var item in _container.GetBlobsAsync(prefix: prefix, cancellationToken: ct))
            {
                names.Add(item.Name);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to list gateway logs");
        }
        return names;
    }
}
