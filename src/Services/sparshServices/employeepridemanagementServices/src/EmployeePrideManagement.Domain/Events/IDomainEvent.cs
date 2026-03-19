using MediatR;

namespace EmployeePrideManagement.Domain.Events;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
