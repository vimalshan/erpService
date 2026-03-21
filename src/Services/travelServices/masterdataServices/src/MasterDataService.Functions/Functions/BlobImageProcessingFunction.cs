using MasterDataService.Domain.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MasterDataService.Functions;

public class BlobImageProcessingFunction
{
    private readonly IBlobStorageService _blobService;
    private readonly ILogger<BlobImageProcessingFunction> _logger;

    public BlobImageProcessingFunction(IBlobStorageService blobService, ILogger<BlobImageProcessingFunction> logger)
    {
        _blobService = blobService;
        _logger = logger;
    }

    [Function("BlobImageProcessing")]
    public async Task Run(
        [BlobTrigger("stationery-images/{blobName}", Connection = "AzureBlobStorage:ConnectionString")] Stream blobStream,
        string blobName)
    {
        _logger.LogInformation("Processing uploaded image: {BlobName}, Size: {Size} bytes", blobName, blobStream.Length);

        // Image processing logic would go here (resize, compress, generate thumbnails, etc.)
        await Task.CompletedTask;

        _logger.LogInformation("Completed processing for image: {BlobName}", blobName);
    }
}
