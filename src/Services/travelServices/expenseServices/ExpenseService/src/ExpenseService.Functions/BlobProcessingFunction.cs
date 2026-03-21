using ExpenseService.Application.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ExpenseService.Functions;

public class BlobProcessingFunction
{
    private readonly IBlobStorageService _blobService;
    private readonly ILogger<BlobProcessingFunction> _logger;

    public BlobProcessingFunction(IBlobStorageService blobService, ILogger<BlobProcessingFunction> logger)
    {
        _blobService = blobService;
        _logger = logger;
    }

    /// <summary>
    /// Triggered when a new blob is uploaded to the stationery-images container
    /// </summary>
    [Function("ProcessUploadedImage")]
    public async Task Run(
        [BlobTrigger("stationery-images/{name}", Connection = "AzureBlobStorage:ConnectionString")] Stream stream,
        string name)
    {
        _logger.LogInformation("Processing blob: {BlobName}, Size: {Size} bytes", name, stream.Length);

        // Process uploaded image (e.g., generate thumbnail, validate, etc.)
        _logger.LogInformation("Blob {BlobName} processed successfully", name);

        await Task.CompletedTask;
    }
}
