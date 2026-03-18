using EmployeeManagement.Application.Common.Interfaces;
using EmployeeManagement.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.EventHandlers;

public sealed class EmployeeCreatedEventHandler : INotificationHandler<EmployeeCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<EmployeeCreatedEventHandler> _logger;

    public EmployeeCreatedEventHandler(IMessagePublisher publisher, ILogger<EmployeeCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(EmployeeCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Employee created: {EmployeeId} - {EmployeeNo}", notification.EmployeeId, notification.EmployeeNo);
        await _publisher.PublishAsync("hr.events", "employee.created", notification, cancellationToken);
    }
}

public sealed class EmployeePromotedEventHandler : INotificationHandler<EmployeePromotedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<EmployeePromotedEventHandler> _logger;

    public EmployeePromotedEventHandler(IMessagePublisher publisher, ILogger<EmployeePromotedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(EmployeePromotedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Employee promoted: {EmployeeId}, Promotion: {PromotionNo}", notification.EmployeeId, notification.PromotionNo);
        await _publisher.PublishAsync("hr.events", "employee.promoted", notification, cancellationToken);
    }
}

public sealed class EmployeeTransferredEventHandler : INotificationHandler<EmployeeTransferredEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<EmployeeTransferredEventHandler> _logger;

    public EmployeeTransferredEventHandler(IMessagePublisher publisher, ILogger<EmployeeTransferredEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(EmployeeTransferredEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Employee transferred: {EmployeeId} from {OldUnit} to {NewUnit}",
            notification.EmployeeId, notification.OldUnit, notification.NewUnit);
        await _publisher.PublishAsync("hr.events", "employee.transferred", notification, cancellationToken);
    }
}
