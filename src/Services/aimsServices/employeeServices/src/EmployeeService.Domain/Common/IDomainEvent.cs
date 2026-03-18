using MediatR;

namespace EmployeeService.Domain.Common;

/// <summary>Marker interface for domain events — extends MediatR INotification for in-process dispatch.</summary>
public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}
