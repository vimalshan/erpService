using IntegrationService.Application.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace IntegrationService.Functions;

public class BlobProcessingFunction(
    IBlobStorageService blobStorageService,
    ILogger<BlobProcessingFunction> logger)
{
    [Function("ProcessStationeryImage")]
    public async Task ProcessImage(
        [BlobTrigger("stationery-images/{name}", Connection = "AzureBlobStorage")] Stream imageStream,
        string name,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing stationery image: {Name}", name);

        var processedName = $"processed/{name}";
        await blobStorageService.UploadAsync(
            "stationery-images-processed",
            processedName,
            imageStream,
            "image/jpeg",
            cancellationToken);

        logger.LogInformation("Stationery image processed and uploaded: {ProcessedName}", processedName);
    }
}
