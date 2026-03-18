using Microsoft.Extensions.Logging;

namespace InsuranceManagement.Infrastructure.MessageConsumers;

/// <summary>
/// Interface for message consumer
/// </summary>
public interface IMessageConsumer
{
    /// <summary>
    /// Start consuming messages
    /// </summary>
    Task StartAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Stop consuming messages
    /// </summary>
    Task StopAsync();
}

/// <summary>
/// Base class for message consumers
/// </summary>
public abstract class BaseMessageConsumer : IMessageConsumer
{
    protected readonly ILogger<BaseMessageConsumer> _logger;

    protected BaseMessageConsumer(ILogger<BaseMessageConsumer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public abstract Task StartAsync(CancellationToken cancellationToken);

    public abstract Task StopAsync();

    protected virtual void LogMessage(string message, LogLevel level = LogLevel.Information)
    {
        _logger.Log(level, message);
    }
}
