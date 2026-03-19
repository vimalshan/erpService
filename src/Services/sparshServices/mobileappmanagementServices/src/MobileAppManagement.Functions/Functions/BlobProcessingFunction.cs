using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MobileAppManagement.Functions;

public class BlobProcessingFunction(ILogger<BlobProcessingFunction> logger)
{
    [Function("ProcessUploadedImage")]
    public async Task RunAsync(
        [BlobTrigger("mobile-app-images/{name}", Connection = "AzureBlobStorage:ConnectionString")]
        Stream stream, string name, CancellationToken ct)
    {
        logger.LogInformation("Processing uploaded image: {Name}, Size: {Size} bytes", name, stream.Length);

        // Process the uploaded image (e.g., resize, validate, create thumbnails)
        // In production, add image processing logic

        logger.LogInformation("Image processing completed for: {Name}", name);
    }
}
