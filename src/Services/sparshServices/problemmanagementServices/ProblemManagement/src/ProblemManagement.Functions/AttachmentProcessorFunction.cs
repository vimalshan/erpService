using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProblemManagement.Domain.Interfaces;

namespace ProblemManagement.Functions;

public class AttachmentProcessorFunction(
    IBlobStorageService blobService,
    ILogger<AttachmentProcessorFunction> logger)
{
    [Function("AttachmentProcessorFunction")]
    public async Task Run(
        [BlobTrigger("problem-attachments/{name}", Connection = "AzureBlobStorage")] Stream blobStream,
        string name,
        CancellationToken ct)
    {
        logger.LogInformation("Processing blob: {Name}, Size: {Size} bytes", name, blobStream.Length);

        // Process the uploaded attachment (e.g., virus scan, thumbnail generation, metadata extraction)
        // This runs automatically when a new blob is uploaded to the problem-attachments container

        await Task.CompletedTask;
        logger.LogInformation("Blob {Name} processed successfully", name);
    }
}
