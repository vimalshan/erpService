using LetTransactionService.Application.EventHandlers;
using LetTransactionService.Application.Interfaces;
using LetTransactionService.Domain.Common;
using LetTransactionService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LetTransactionService.Infrastructure.DomainEvents;

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
                LetRequestCreatedEvent e => new LetRequestCreatedNotification(e),
                FeedbackSubmittedEvent e => new FeedbackSubmittedNotification(e),
                ReviewCreatedEvent e => new ReviewCreatedNotification(e),
                ReviewApprovedEvent e => new ReviewApprovedNotification(e),
                _ => null
            };

            if (notification is not null)
                await mediator.Publish(notification, ct);
        }
    }
}
