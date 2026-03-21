using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TravelRequestService.Functions;

public class BlobTriggerFunction
{
    private readonly ILogger<BlobTriggerFunction> _logger;

    public BlobTriggerFunction(ILogger<BlobTriggerFunction> logger)
    {
        _logger = logger;
    }

    [Function("ProcessTravelDocument")]
    public async Task Run(
        [BlobTrigger("travel-documents/{name}", Connection = "AzureWebJobsStorage")] Stream stream,
        string name)
    {
        _logger.LogInformation("Processing uploaded travel document: {Name}", name);

        // Process the uploaded document (e.g., extract metadata, validate, etc.)
        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();
        _logger.LogInformation("Document {Name} processed, size: {Length} bytes", name, content.Length);
    }
}
