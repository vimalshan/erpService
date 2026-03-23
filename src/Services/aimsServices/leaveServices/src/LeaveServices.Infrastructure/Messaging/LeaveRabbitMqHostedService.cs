using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LeaveServices.Infrastructure.Messaging;

/// <summary>
/// Hosted service that starts the Leave RabbitMQ publisher and consumers
/// when the application starts, with exponential-backoff reconnect on failure.
/// </summary>
public sealed class LeaveRabbitMqHostedService : BackgroundService
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<LeaveRabbitMqHostedService>  _logger;
    private readonly ILogger<LeaveAppliedConsumer>        _appliedLogger;
    private readonly ILogger<LeaveApprovedConsumer>       _approvedLogger;

    private static readonly int[] RetryDelaysSeconds = [5, 10, 20, 30, 60];

    public LeaveRabbitMqHostedService(
        IOptions<RabbitMqSettings>                 settings,
        ILogger<LeaveRabbitMqHostedService>        logger,
        ILogger<LeaveAppliedConsumer>              appliedLogger,
        ILogger<LeaveApprovedConsumer>             approvedLogger)
    {
        _settings       = settings.Value;
        _logger         = logger;
        _appliedLogger  = appliedLogger;
        _approvedLogger = approvedLogger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Leave RabbitMQ Hosted Service starting");

        int attempt = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            LeaveAppliedConsumer?  applied  = null;
            LeaveApprovedConsumer? approved = null;

            try
            {
                applied  = new LeaveAppliedConsumer(_appliedLogger);
                approved = new LeaveApprovedConsumer(_approvedLogger);

                await Task.WhenAll(
                    applied.StartAsync(_settings, stoppingToken),
                    approved.StartAsync(_settings, stoppingToken)
                );

                attempt = 0; // reset on success
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Leave RabbitMQ Hosted Service stopping");
                break;
            }
            catch (Exception ex)
            {
                int delaySecs = attempt < RetryDelaysSeconds.Length
                    ? RetryDelaysSeconds[attempt]
                    : RetryDelaysSeconds[^1];

                _logger.LogError(ex,
                    "Leave RabbitMQ consumers failed. Retrying in {Delay}s (attempt {Attempt})",
                    delaySecs, attempt + 1);
                attempt++;

                if (applied  is not null) await applied.DisposeAsync();
                if (approved is not null) await approved.DisposeAsync();

                try   { await Task.Delay(TimeSpan.FromSeconds(delaySecs), stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
            finally
            {
                if (applied  is not null) await applied.DisposeAsync();
                if (approved is not null) await approved.DisposeAsync();
            }
        }
    }
}
