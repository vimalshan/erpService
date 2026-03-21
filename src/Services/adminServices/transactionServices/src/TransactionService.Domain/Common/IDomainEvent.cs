namespace TransactionService.Domain.Common;

public interface IDomainEvent : MediatR.INotification
{
    DateTime OccurredOn { get; }
}
