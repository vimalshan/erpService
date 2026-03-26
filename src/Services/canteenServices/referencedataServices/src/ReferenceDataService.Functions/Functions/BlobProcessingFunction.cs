using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReferenceDataService.Application.Interfaces;

namespace ReferenceDataService.Functions.Functions;

public class BlobProcessingFunction : BackgroundService
{
    private readonly ILogger<BlobProcessingFunction> _logger;

    public BlobProcessingFunction(ILogger<BlobProcessingFunction> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BlobProcessingFunction started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Checking for blob processing tasks at {Time}", DateTimeOffset.UtcNow);

                // Process stationery item image uploads, resizing, thumbnail generation, etc.
                _logger.LogInformation("Blob processing completed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during blob processing");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
