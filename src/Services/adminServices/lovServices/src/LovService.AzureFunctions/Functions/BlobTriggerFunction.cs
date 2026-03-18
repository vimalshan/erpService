using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LovService.AzureFunctions.Functions;

/// <summary>
/// Blob Storage trigger Azure Function.
/// Fires when a new blob is uploaded to the 'lov-imports' container.
/// </summary>
public class BlobTriggerFunction(ILogger<BlobTriggerFunction> logger)
{
    [Function("ProcessLovBlobImport")]
    public async Task RunAsync(
        [BlobTrigger("lov-imports/{name}", Connection = "AzureWebJobsStorage")] Stream blobStream,
        string name,
        FunctionContext context)
    {
        logger.LogInformation("Blob trigger fired for: {BlobName} ({Bytes} bytes)", name, blobStream.Length);

        using var reader = new StreamReader(blobStream);
        var content = await reader.ReadToEndAsync();

        logger.LogInformation("Processing LOV import blob '{Name}'", name);

        // In production: parse CSV/JSON, validate, and upsert via LOV service or EF
        // e.g. var lines = content.Split('\n'); foreach (var line in lines) { ... }

        logger.LogInformation("Completed processing blob '{Name}'", name);
    }
}
