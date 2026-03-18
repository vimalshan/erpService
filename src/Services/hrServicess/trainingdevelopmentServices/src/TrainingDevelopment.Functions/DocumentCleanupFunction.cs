using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using TrainingDevelopment.Infrastructure.BlobStorage;

namespace TrainingDevelopment.Functions;

public class DocumentCleanupFunction
{
    private readonly ILogger<DocumentCleanupFunction> _logger;
    private readonly BlobStorageService _blobStorage;

    public DocumentCleanupFunction(ILogger<DocumentCleanupFunction> logger, BlobStorageService blobStorage)
    {
        _logger = logger;
        _blobStorage = blobStorage;
    }

    /// <summary>
    /// Runs weekly every Sunday at 1am UTC — cleans up orphaned training documents.
    /// </summary>
    [Function(nameof(DocumentCleanupFunction))]
    public async Task Run(
        [TimerTrigger("0 0 1 * * 0")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Document Cleanup Function triggered at {Time}", DateTime.UtcNow);

        // In production: query orphaned blob names vs DB records and delete
        _logger.LogInformation("Document Cleanup Function completed.");
        await Task.CompletedTask;
    }
}
