using InsuranceService.Domain.Events;
using InsuranceService.Infrastructure.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InsuranceService.Infrastructure.EventHandlers;

public class InsuranceRegisteredEventHandler : INotificationHandler<InsuranceRegisteredEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<InsuranceRegisteredEventHandler> _logger;

    public InsuranceRegisteredEventHandler(IMessagePublisher publisher, ILogger<InsuranceRegisteredEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(InsuranceRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Insurance registered: Company={Company}, Plan={Plan}, Type={Type}",
            notification.CompanyCode, notification.PlanNumber, notification.InsuranceType);

        await _publisher.PublishAsync("insurance.events", "insurance.registered", notification, cancellationToken);
    }
}

public class InsuranceStatusChangedEventHandler : INotificationHandler<InsuranceStatusChangedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<InsuranceStatusChangedEventHandler> _logger;

    public InsuranceStatusChangedEventHandler(IMessagePublisher publisher, ILogger<InsuranceStatusChangedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(InsuranceStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Insurance status changed: Company={Company}, Plan={Plan}, {OldStatus}->{NewStatus}",
            notification.CompanyCode, notification.PlanNumber, notification.OldStatus, notification.NewStatus);

        await _publisher.PublishAsync("insurance.events", "insurance.status.changed", notification, cancellationToken);
    }
}
