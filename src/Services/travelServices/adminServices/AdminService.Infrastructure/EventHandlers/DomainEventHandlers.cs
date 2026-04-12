using MediatR;
using Microsoft.Extensions.Logging;
using AdminService.Domain.Events;
using AdminService.Infrastructure.Messaging;

namespace AdminService.Infrastructure.EventHandlers;

/// <summary>
/// Handles AdminUnitCreatedEvent by publishing to RabbitMQ
/// </summary>
public class AdminUnitCreatedEventHandler : INotificationHandler<AdminUnitCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<AdminUnitCreatedEventHandler> _logger;

    public AdminUnitCreatedEventHandler(IMessagePublisher publisher, ILogger<AdminUnitCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(AdminUnitCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing AdminUnitCreatedEvent for AdminCode: {AdminCode}", notification.AdminCode);
        try
        {
            await _publisher.PublishAsync("admin.events", "admin.unit.created", notification, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to publish AdminUnitCreatedEvent to RabbitMQ. Event will not be retried.");
        }
    }
}

/// <summary>
/// Handles AdminUnitUpdatedEvent by publishing to RabbitMQ
/// </summary>
public class AdminUnitUpdatedEventHandler : INotificationHandler<AdminUnitUpdatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<AdminUnitUpdatedEventHandler> _logger;

    public AdminUnitUpdatedEventHandler(IMessagePublisher publisher, ILogger<AdminUnitUpdatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(AdminUnitUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing AdminUnitUpdatedEvent for AdminCode: {AdminCode}", notification.AdminCode);
        try
        {
            await _publisher.PublishAsync("admin.events", "admin.unit.updated", notification, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to publish AdminUnitUpdatedEvent to RabbitMQ. Event will not be retried.");
        }
    }
}

/// <summary>
/// Handles AdminUnitDeletedEvent by publishing to RabbitMQ
/// </summary>
public class AdminUnitDeletedEventHandler : INotificationHandler<AdminUnitDeletedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<AdminUnitDeletedEventHandler> _logger;

    public AdminUnitDeletedEventHandler(IMessagePublisher publisher, ILogger<AdminUnitDeletedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(AdminUnitDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing AdminUnitDeletedEvent for AdminCode: {AdminCode}", notification.AdminCode);
        try
        {
            await _publisher.PublishAsync("admin.events", "admin.unit.deleted", notification, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to publish AdminUnitDeletedEvent to RabbitMQ. Event will not be retried.");
        }
    }
}

/// <summary>
/// Handles FinanceUnitCreatedEvent by publishing to RabbitMQ
/// </summary>
public class FinanceUnitCreatedEventHandler : INotificationHandler<FinanceUnitCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<FinanceUnitCreatedEventHandler> _logger;

    public FinanceUnitCreatedEventHandler(IMessagePublisher publisher, ILogger<FinanceUnitCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(FinanceUnitCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing FinanceUnitCreatedEvent for UnitId: {UnitId}", notification.UnitId);
        try
        {
            await _publisher.PublishAsync("admin.events", "finance.unit.created", notification, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to publish FinanceUnitCreatedEvent to RabbitMQ. Event will not be retried.");
        }
    }
}
