using MediatR;

namespace StrategicStock.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
