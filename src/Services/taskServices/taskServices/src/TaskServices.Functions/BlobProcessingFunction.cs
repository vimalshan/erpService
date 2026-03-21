using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TaskServices.Functions;

public class BlobProcessingFunction
{
    private readonly ILogger<BlobProcessingFunction> _logger;

    public BlobProcessingFunction(ILogger<BlobProcessingFunction> logger)
    {
        _logger = logger;
    }

    [Function("BlobProcessing")]
    public async Task Run(
        [BlobTrigger("task-attachments/{name}", Connection = "AzureBlobStorage:ConnectionString")] Stream blobStream,
        string name)
    {
        _logger.LogInformation("Blob trigger fired for: {BlobName}, Size: {Size} bytes", name, blobStream.Length);

        // Process uploaded blob (e.g., validate, resize images, extract metadata)
        await Task.CompletedTask;

        _logger.LogInformation("Blob processing completed for: {BlobName}", name);
    }
}
