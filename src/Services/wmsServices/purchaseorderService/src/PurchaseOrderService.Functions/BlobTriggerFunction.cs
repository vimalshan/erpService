using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace PurchaseOrderService.Functions;

public class BlobTriggerFunction
{
    private readonly ILogger<BlobTriggerFunction> _logger;

    public BlobTriggerFunction(ILogger<BlobTriggerFunction> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Triggered when a new image is uploaded to the stationery-images container.
    /// </summary>
    [Function("ProcessStationeryImage")]
    public async Task ProcessImage(
        [BlobTrigger("stationery-images/{name}", Connection = "AzureBlobStorage:ConnectionString")] Stream blobStream,
        string name)
    {
        _logger.LogInformation("Processing uploaded stationery image: {Name}, Size: {Size} bytes", name, blobStream.Length);

        // In production: resize image, generate thumbnail, update product record
        // await imageProcessor.ResizeAsync(blobStream, name);

        await Task.CompletedTask;
    }
}
