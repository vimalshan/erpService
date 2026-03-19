using FilingAndArchiveService.Application.Files.Queries.GetAllFiles;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FilingAndArchiveService.Functions.Functions;

public class FileArchiveFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<FileArchiveFunction> _logger;

    public FileArchiveFunction(IMediator mediator, ILogger<FileArchiveFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>Timer-triggered function that runs daily to archive old files.</summary>
    [Function("ArchiveOldFilesTimer")]
    public async Task RunArchiveTimer(
        [TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo, // 2 AM daily
        FunctionContext context)
    {
        _logger.LogInformation("ArchiveOldFiles trigger executed at: {Time}", DateTime.UtcNow);

        try
        {
            var files = await _mediator.Send(new GetAllFilesQuery(1, 1000));
            var cutoffDate = DateTime.UtcNow.AddYears(-7);

            var toArchive = files
                .Where(f => f.FileCreatedOn < cutoffDate && f.FileStatus == "C")
                .ToList();

            _logger.LogInformation("Found {Count} files to archive (created before {Cutoff})",
                toArchive.Count, cutoffDate);

            // In a real implementation, dispatch ArchiveFileCommand for each
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during file archiving process");
            throw;
        }
    }

    /// <summary>HTTP-triggered function to trigger archiving on demand.</summary>
    [Function("TriggerArchive")]
    public async Task<HttpResponseData> TriggerArchive(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "archive/trigger")] HttpRequestData req,
        [DurableClient] DurableTaskClient durableClient,
        FunctionContext context)
    {
        _logger.LogInformation("Manual archive trigger received");

        var instanceId = await durableClient.ScheduleNewOrchestrationInstanceAsync(
            "ArchiveOrchestration");

        _logger.LogInformation("Started archive orchestration with ID: {InstanceId}", instanceId);

        var response = req.CreateResponse(HttpStatusCode.Accepted);
        response.Headers.Add("Location", $"/api/archive/status/{instanceId}");
        await response.WriteStringAsync($"Archive started with ID: {instanceId}");
        return response;
    }
}
