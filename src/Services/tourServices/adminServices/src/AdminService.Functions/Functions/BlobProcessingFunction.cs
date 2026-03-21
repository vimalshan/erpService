using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AdminService.Functions;

public class BlobProcessingFunction
{
    private readonly ILogger<BlobProcessingFunction> _logger;

    public BlobProcessingFunction(ILogger<BlobProcessingFunction> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Triggered when a new blob is uploaded to the admin-images container.
    /// Can be used to resize images, validate content, etc.
    /// </summary>
    [Function("BlobProcessingFunction")]
    public async Task ProcessBlob(
        [BlobTrigger("admin-images/{name}", Connection = "AzureBlobStorage:ConnectionString")] Stream stream,
        string name)
    {
        _logger.LogInformation("Processing blob: {Name}, Size: {Size} bytes", name, stream.Length);

        // TODO: Image processing - resize, compress, validate format
        // e.g., ensure stationery item images are within size limits

        _logger.LogInformation("Blob {Name} processed successfully", name);
        await Task.CompletedTask;
    }
}
