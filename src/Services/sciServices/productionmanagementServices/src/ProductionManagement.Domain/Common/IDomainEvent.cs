using MediatR;

namespace ProductionManagement.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
