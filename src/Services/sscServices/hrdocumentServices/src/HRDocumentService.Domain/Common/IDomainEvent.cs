using MediatR;

namespace HRDocumentService.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
