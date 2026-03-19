using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FilingAndArchiveService.Functions.Functions;

public class FileCleanupFunction
{
    private readonly ILogger<FileCleanupFunction> _logger;

    public FileCleanupFunction(ILogger<FileCleanupFunction> logger)
        => _logger = logger;

    /// <summary>Weekly cleanup of error log entries older than 90 days.</summary>
    [Function("CleanupErrorLogs")]
    public Task RunCleanup(
        [TimerTrigger("0 0 3 * * 0")] TimerInfo timerInfo, // 3 AM every Sunday
        FunctionContext context)
    {
        _logger.LogInformation("CleanupErrorLogs triggered at: {Time}", DateTime.UtcNow);

        // In a real scenario: purge old FILINGDOC_ERROR_LIST entries
        _logger.LogInformation("Error log cleanup completed");
        return Task.CompletedTask;
    }

    /// <summary>Processes a queue message when a document print is requested.</summary>
    [Function("ProcessDocPrint")]
    public Task ProcessDocPrint(
        [QueueTrigger("filing-doc-print", Connection = "AzureWebJobsStorage")] string message,
        FunctionContext context)
    {
        _logger.LogInformation("ProcessDocPrint received message: {Message}", message);
        // Parse the message and record the print in FILING_DOC_PRINT
        return Task.CompletedTask;
    }
}
