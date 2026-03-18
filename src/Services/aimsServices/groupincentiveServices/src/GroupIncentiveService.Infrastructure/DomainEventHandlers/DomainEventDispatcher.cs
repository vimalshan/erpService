using GroupIncentiveService.Domain.Events;
using GroupIncentiveService.Domain.Interfaces;
using GroupIncentiveService.Infrastructure.Messaging.RabbitMQ;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GroupIncentiveService.Infrastructure.DomainEventHandlers;

/// <summary>
/// Dispatches domain events to MediatR and also publishes them to RabbitMQ.
/// </summary>
public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher _publisher;
    private readonly IMessagePublisher _messagePub;
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(IPublisher publisher, IMessagePublisher messagePub,
        ILogger<DomainEventDispatcher> logger)
    {
        _publisher = publisher;
        _messagePub = messagePub;
        _logger = logger;
    }

    public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in events)
        {
            _logger.LogInformation("Dispatching domain event {EventType}", domainEvent.GetType().Name);
            await _publisher.Publish(domainEvent, cancellationToken);

            var routingKey = domainEvent switch
            {
                GroupIncentiveCreatedEvent => "incentive.created",
                GroupIncentiveApprovedEvent => "incentive.approved",
                GroupIncentiveRejectedEvent => "incentive.rejected",
                GroupCreatedEvent => "group.created",
                EmployeeAddedToGroupEvent => "group.employee.added",
                _ => "incentive.event"
            };

            await _messagePub.PublishAsync(routingKey, domainEvent, cancellationToken);
        }
    }
}
