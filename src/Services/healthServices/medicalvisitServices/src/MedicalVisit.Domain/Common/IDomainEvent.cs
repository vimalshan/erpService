using MediatR;

namespace MedicalVisit.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
