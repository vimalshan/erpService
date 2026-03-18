using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TeamServices.Functions;

public class BlobProcessingFunction
{
    private readonly ILogger<BlobProcessingFunction> _logger;

    public BlobProcessingFunction(ILogger<BlobProcessingFunction> logger)
    {
        _logger = logger;
    }

    [Function("ProcessTeamImage")]
    public Task RunAsync(
        [BlobTrigger("team-images/{name}", Connection = "AzureBlobStorage:ConnectionString")] Stream stream,
        string name)
    {
        _logger.LogInformation("Processing blob: {BlobName}, Size: {Size} bytes", name, stream.Length);

        // Add image processing logic here (e.g., resize, validate, generate thumbnails)
        _logger.LogInformation("Blob processing completed for: {BlobName}", name);
        return Task.CompletedTask;
    }
}
