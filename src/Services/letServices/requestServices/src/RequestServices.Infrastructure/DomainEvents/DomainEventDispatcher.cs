using MediatR;
using Microsoft.Extensions.Logging;
using RequestServices.Application.EventHandlers;
using RequestServices.Application.Interfaces;
using RequestServices.Domain.Common;
using RequestServices.Domain.Events;

namespace RequestServices.Infrastructure.DomainEvents;

public class DomainEventDispatcher(IMediator mediator, ILogger<DomainEventDispatcher> logger)
    : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<DomainEvent> events, CancellationToken ct = default)
    {
        foreach (var @event in events)
        {
            logger.LogDebug("Dispatching domain event {EventType}", @event.GetType().Name);

            INotification? notification = @event switch
            {
                RequestCreatedEvent  e => new RequestCreatedDomainNotification(e),
                RequestApprovedEvent e => new RequestApprovedDomainNotification(e),
                RequestCancelledEvent e => new RequestCancelledDomainNotification(e),
                _ => null
            };

            if (notification is not null)
                await mediator.Publish(notification, ct);
        }
    }
}
