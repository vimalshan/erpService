using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace MobileExpenseManagement.AzureFunctions;

/// <summary>
/// Azure Function for processing expense file uploads
/// </summary>
public class ProcessExpenseFileFunction
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<ProcessExpenseFileFunction> _logger;

    public ProcessExpenseFileFunction(BlobServiceClient blobServiceClient, ILogger<ProcessExpenseFileFunction> logger)
    {
        _blobServiceClient = blobServiceClient;
        _logger = logger;
    }

    [Function("ProcessExpenseFile")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "expenses/files/process")] HttpRequestData req,
        FunctionContext executionContext)
    {
        _logger.LogInformation("Processing expense file upload");

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var container = _blobServiceClient.GetBlobContainerClient("expenses");
            
            // TODO: Process file, validate, scan for viruses, optimize, etc.
            
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new { message = "File processed successfully" });
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing expense file");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            await response.WriteAsJsonAsync(new { error = ex.Message });
            return response;
        }
    }
}

/// <summary>
/// Azure Function for generating expense reports (timer-triggered)
/// </summary>
public class GenerateExpenseReportFunction
{
    private readonly ILogger<GenerateExpenseReportFunction> _logger;

    public GenerateExpenseReportFunction(ILogger<GenerateExpenseReportFunction> logger)
    {
        _logger = logger;
    }

    [Function("GenerateExpenseReport")]
    public async Task Run(
        [TimerTrigger("0 0 1 * * *")] TimerInfo myTimer,
        FunctionContext context)
    {
        _logger.LogInformation($"Expense report generation started at {DateTime.UtcNow}");

        try
        {
            // TODO: Generate monthly expense reports, aggregate statistics, send notifications
            
            _logger.LogInformation("Expense reports generated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating expense reports");
        }

        await Task.CompletedTask;
    }
}

/// <summary>
/// Azure Function for cleanup of old files (timer-triggered)
/// </summary>
public class CleanupOldFilesFunction
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly ILogger<CleanupOldFilesFunction> _logger;

    public CleanupOldFilesFunction(BlobServiceClient blobServiceClient, ILogger<CleanupOldFilesFunction> logger)
    {
        _blobServiceClient = blobServiceClient;
        _logger = logger;
    }

    [Function("CleanupOldFiles")]
    public async Task Run(
        [TimerTrigger("0 0 2 * * *")] TimerInfo myTimer,
        FunctionContext context)
    {
        _logger.LogInformation("Cleanup of old files started at {0}", DateTime.UtcNow);

        try
        {
            var container = _blobServiceClient.GetBlobContainerClient("expenses");
            var currentDate = DateTime.UtcNow.AddDays(-90); // Delete files older than 90 days

            await foreach (var blobItem in container.GetBlobsAsync())
            {
                if (blobItem.Properties.CreatedOn < currentDate)
                {
                    await container.DeleteBlobAsync(blobItem.Name);
                    _logger.LogInformation($"Deleted old file: {blobItem.Name}");
                }
            }

            _logger.LogInformation("Cleanup of old files completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during cleanup of old files");
        }
    }
}
