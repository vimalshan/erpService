using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TdsService.Application.Files.Queries.GetAllTdsFiles;
using TdsService.Application.Files.Commands.UpdateEmailStatus;

namespace TdsService.Functions;

/// <summary>
/// Timer-triggered function that checks for TDS files with pending email notifications
/// and dispatches them every hour.
/// </summary>
public sealed class TdsEmailDispatchFunction
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TdsEmailDispatchFunction> _logger;

    public TdsEmailDispatchFunction(
        IServiceScopeFactory scopeFactory,
        ILogger<TdsEmailDispatchFunction> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    [Function(nameof(TdsEmailDispatchFunction))]
    public async Task Run(
        [TimerTrigger("0 0 * * * *")] TimerInfo timer,   // every hour
        CancellationToken ct = default)
    {
        _logger.LogInformation("TdsEmailDispatchFunction triggered at: {Time}", DateTimeOffset.UtcNow);

        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var pendingFiles = await mediator.Send(new GetAllTdsFilesQuery(1, 100), ct);

        var toSend = pendingFiles.Items
            .Where(f => f.EmailStatus == "N")
            .ToList();

        _logger.LogInformation("Found {Count} TDS files with pending email notifications.", toSend.Count);

        foreach (var file in toSend)
        {
            try
            {
                // NOTE: In production, integrate with an email service (SendGrid, SES, etc.)
                // before marking as sent.
                _logger.LogInformation("Processing email dispatch for FileId={FileId}, PAN={Pan}",
                    file.FileId, file.PanNo);

                // Uncomment after email service integration:
                // await mediator.Send(new UpdateEmailStatusCommand(file.FileId), ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process email dispatch for FileId={FileId}", file.FileId);
            }
        }

        _logger.LogInformation("TdsEmailDispatchFunction completed.");
    }
}
