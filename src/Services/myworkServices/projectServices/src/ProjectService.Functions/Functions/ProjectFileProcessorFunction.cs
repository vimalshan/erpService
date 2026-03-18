using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProjectService.Domain.Interfaces;

namespace ProjectService.Functions;

public class ProjectFileProcessorFunction(
    ILogger<ProjectFileProcessorFunction> logger,
    IBlobStorageService blobStorage)
{
    [Function("ProjectFileProcessor")]
    public async Task Run(
        [BlobTrigger("project-files/{name}", Connection = "AzureBlobStorage")] Stream blobStream,
        string name,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing uploaded file: {FileName}", name);

        // Copy to processed container
        await blobStorage.UploadFileAsync(
            "project-files-processed",
            $"processed-{name}",
            blobStream,
            "application/octet-stream",
            cancellationToken);

        logger.LogInformation("File {FileName} processed successfully", name);
    }
}
