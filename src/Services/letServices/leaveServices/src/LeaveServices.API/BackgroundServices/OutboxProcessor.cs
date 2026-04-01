using LeaveServices.Infrastructure.Messaging;
using LeaveServices.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LeaveServices.API.BackgroundServices;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly TimeSpan _pollingInterval = TimeSpan.FromSeconds(30);
    private const int MaxRetries = 10;

    public OutboxProcessor(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox processor started — polling every {Interval}s", _pollingInterval.TotalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessOutboxAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Outbox processing cycle failed — will retry next interval");
            }

            await Task.Delay(_pollingInterval, stoppingToken);
        }
    }

    private async Task ProcessOutboxAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LeaveDbContext>();
        var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();

        var pending = await db.OutboxMessages
            .Where(m => m.ProcessedOn == null && m.RetryCount < MaxRetries)
            .OrderBy(m => m.CreatedOn)
            .Take(50)
            .ToListAsync(ct);

        if (pending.Count == 0) return;

        _logger.LogInformation("Outbox: processing {Count} pending message(s)", pending.Count);

        foreach (var msg in pending)
        {
            try
            {
                await publisher.PublishRawAsync(msg.RoutingKey, msg.Payload, ct);
                msg.ProcessedOn = DateTime.UtcNow;
                msg.Error = null;
                _logger.LogInformation("Outbox: delivered {EventType} (ID {Id})", msg.EventType, msg.Id);
            }
            catch (Exception ex)
            {
                msg.RetryCount++;
                msg.Error = ex.Message.Length > 2000 ? ex.Message[..2000] : ex.Message;
                _logger.LogWarning("Outbox: retry {Retry}/{Max} failed for {EventType} (ID {Id})",
                    msg.RetryCount, MaxRetries, msg.EventType, msg.Id);

                // If broker is still down, stop processing this batch
                break;
            }
        }

        await db.SaveChangesAsync(ct);
    }
}
