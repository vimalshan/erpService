using MediatR;

namespace OrganizationSetup.Domain.Common;

/// <summary>Marker interface for domain events. Dispatched via MediatR in Application layer.</summary>
public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}

