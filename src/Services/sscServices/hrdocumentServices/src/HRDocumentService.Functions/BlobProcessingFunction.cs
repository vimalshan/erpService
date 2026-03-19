using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace HRDocumentService.Functions;

public class BlobProcessingFunction(ILogger<BlobProcessingFunction> logger)
{
    [Function("BlobProcessingFunction")]
    public async Task Run(
        [BlobTrigger("hr-documents/{name}", Connection = "BlobStorage")] Stream blobStream,
        string name,
        CancellationToken ct)
    {
        logger.LogInformation("Processing blob: {BlobName}, Size: {Size} bytes", name, blobStream.Length);

        // Process the uploaded document (e.g., virus scan, thumbnail generation, indexing)
        await Task.Delay(100, ct); // Simulate processing

        logger.LogInformation("Blob processing completed for: {BlobName}", name);
    }
}
