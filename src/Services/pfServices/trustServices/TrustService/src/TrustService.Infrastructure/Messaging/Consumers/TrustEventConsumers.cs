using MassTransit;
using Microsoft.Extensions.Logging;

namespace TrustService.Infrastructure.Messaging.Consumers;

// Message contracts
public record TrustCreatedMessage
{
    public string TrustCode { get; init; } = string.Empty;
    public string TrustName { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
}

public record TrustUpdatedMessage
{
    public string TrustCode { get; init; } = string.Empty;
    public string TrustName { get; init; } = string.Empty;
    public DateTime UpdatedDate { get; init; }
}

public record TrustClosedMessage
{
    public string TrustCode { get; init; } = string.Empty;
    public DateTime ClosureDate { get; init; }
}

public record TrustStatusChangedMessage
{
    public string TrustCode { get; init; } = string.Empty;
    public string NewStatus { get; init; } = string.Empty;
    public DateTime ChangedDate { get; init; }
}

// Consumers
public class TrustCreatedConsumer : IConsumer<TrustCreatedMessage>
{
    private readonly ILogger<TrustCreatedConsumer> _logger;

    public TrustCreatedConsumer(ILogger<TrustCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TrustCreatedMessage> context)
    {
        _logger.LogInformation("Processing TrustCreated message: Trust {TrustCode} - {TrustName}",
            context.Message.TrustCode, context.Message.TrustName);

        // Add business logic here: notifications, downstream syncing, etc.
        return Task.CompletedTask;
    }
}

public class TrustUpdatedConsumer : IConsumer<TrustUpdatedMessage>
{
    private readonly ILogger<TrustUpdatedConsumer> _logger;

    public TrustUpdatedConsumer(ILogger<TrustUpdatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TrustUpdatedMessage> context)
    {
        _logger.LogInformation("Processing TrustUpdated message: Trust {TrustCode} - {TrustName}",
            context.Message.TrustCode, context.Message.TrustName);

        return Task.CompletedTask;
    }
}

public class TrustClosedConsumer : IConsumer<TrustClosedMessage>
{
    private readonly ILogger<TrustClosedConsumer> _logger;

    public TrustClosedConsumer(ILogger<TrustClosedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TrustClosedMessage> context)
    {
        _logger.LogInformation("Processing TrustClosed message: Trust {TrustCode}, Closure: {ClosureDate}",
            context.Message.TrustCode, context.Message.ClosureDate);

        return Task.CompletedTask;
    }
}

public class TrustStatusChangedConsumer : IConsumer<TrustStatusChangedMessage>
{
    private readonly ILogger<TrustStatusChangedConsumer> _logger;

    public TrustStatusChangedConsumer(ILogger<TrustStatusChangedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TrustStatusChangedMessage> context)
    {
        _logger.LogInformation("Processing TrustStatusChanged message: Trust {TrustCode}, New Status: {NewStatus}",
            context.Message.TrustCode, context.Message.NewStatus);

        return Task.CompletedTask;
    }
}
