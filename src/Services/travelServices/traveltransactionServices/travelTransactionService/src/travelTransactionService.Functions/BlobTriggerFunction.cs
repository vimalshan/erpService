using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace travelTransactionService.Functions;

public class BlobTriggerFunction
{
    private readonly ILogger<BlobTriggerFunction> _logger;

    public BlobTriggerFunction(ILogger<BlobTriggerFunction> logger)
    {
        _logger = logger;
    }

    [Function("ProcessTransactionDocument")]
    public async Task Run(
        [BlobTrigger("transaction-documents/{name}", Connection = "AzureWebJobsStorage")] Stream stream,
        string name)
    {
        _logger.LogInformation("Processing uploaded transaction document: {Name}", name);

        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();
        _logger.LogInformation("Document {Name} processed, size: {Length} bytes", name, content.Length);
    }
}
