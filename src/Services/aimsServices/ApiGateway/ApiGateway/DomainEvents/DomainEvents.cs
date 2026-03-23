using MediatR;

namespace ApiGateway.DomainEvents;

/// <summary>
/// Raised when a downstream service reports a state change via RabbitMQ.
/// </summary>
public record ServiceStateChangedEvent(
    string ServiceName,
    string EventType,
    string Payload) : INotification;

/// <summary>
/// Raised when a circuit breaker state changes for a downstream service.
/// </summary>
public record CircuitBreakerStateChangedEvent(
    string ServiceName,
    string State,
    string? Reason = null) : INotification;

/// <summary>
/// Raised when a gateway request is rate-limited.
/// </summary>
public record RequestRateLimitedEvent(
    string ClientId,
    string Endpoint,
    DateTime Timestamp) : INotification;

// ── Handlers ──

public class ServiceStateChangedHandler : INotificationHandler<ServiceStateChangedEvent>
{
    private readonly ILogger<ServiceStateChangedHandler> _logger;

    public ServiceStateChangedHandler(ILogger<ServiceStateChangedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ServiceStateChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[DomainEvent] Service={Service} EventType={EventType} Payload={Payload}",
            notification.ServiceName,
            notification.EventType,
            notification.Payload);
        return Task.CompletedTask;
    }
}

public class CircuitBreakerStateChangedHandler : INotificationHandler<CircuitBreakerStateChangedEvent>
{
    private readonly ILogger<CircuitBreakerStateChangedHandler> _logger;

    public CircuitBreakerStateChangedHandler(ILogger<CircuitBreakerStateChangedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CircuitBreakerStateChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "[DomainEvent] CircuitBreaker Service={Service} State={State} Reason={Reason}",
            notification.ServiceName,
            notification.State,
            notification.Reason);
        return Task.CompletedTask;
    }
}

public class RequestRateLimitedHandler : INotificationHandler<RequestRateLimitedEvent>
{
    private readonly ILogger<RequestRateLimitedHandler> _logger;

    public RequestRateLimitedHandler(ILogger<RequestRateLimitedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(RequestRateLimitedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "[DomainEvent] RateLimited ClientId={ClientId} Endpoint={Endpoint} At={Timestamp}",
            notification.ClientId,
            notification.Endpoint,
            notification.Timestamp);
        return Task.CompletedTask;
    }
}
