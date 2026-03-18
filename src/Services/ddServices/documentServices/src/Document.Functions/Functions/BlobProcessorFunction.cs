using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Document.Functions.Functions;

/// <summary>
/// Background service that polls for new signatory images and processes them.
/// Replaces the former Azure Functions blob trigger on the "signatory-images" container.
/// Production usage: swap this polling loop with an Azure Service Bus / Storage Queue feed.
/// </summary>
public class BlobProcessorWorker : BackgroundService
{
    private static readonly TimeSpan PollInterval = TimeSpan.FromMinutes(5);
    private readonly ILogger<BlobProcessorWorker> _logger;

    public BlobProcessorWorker(ILogger<BlobProcessorWorker> logger)
        => _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(PollInterval, stoppingToken);

            // TODO: inject IBlobStorageService via IServiceScopeFactory, list new blobs,
            //       validate/resize images, and update Signatory.ImageFileName via MediatR.
            _logger.LogInformation("BlobProcessorWorker: polling signatory-images at {Time}", DateTime.UtcNow);
        }
    }
}
