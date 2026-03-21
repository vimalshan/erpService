using MediatR;

namespace ReceivingService.Domain.Common;

/// <summary>
/// Marker interface for domain events.
/// Extends INotification so MediatR can dispatch them automatically.
/// </summary>
public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
