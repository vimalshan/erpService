namespace FeedbackService.Infrastructure.Messaging;

using Application.Commands.Handlers;
using Domain.Common;

/// <summary>
/// Implementation of IDomainEventPublisher
/// </summary>
public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IMessagePublisher _messagePublisher;

    /// <summary>
    /// Initializes a new instance of the DomainEventPublisher class
    /// </summary>
    public DomainEventPublisher(IMessagePublisher messagePublisher)
    {
        _messagePublisher = messagePublisher;
    }

    /// <summary>
    /// Publishes domain events
    /// </summary>
    public async Task PublishAsync(IReadOnlyList<DomainEvent> events, CancellationToken cancellationToken = default)
    {
        foreach (var @event in events)
        {
            await _messagePublisher.PublishAsync(@event, cancellationToken);
        }
    }
}

/// <summary>
/// Interface for message publishing
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publishes a message
    /// </summary>
    Task PublishAsync(object message, CancellationToken cancellationToken = default);
}
