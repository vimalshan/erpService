using FinanceService.Domain.Common;

namespace FinanceService.Domain.Events;

public class PaymentProcessedEvent : IDomainEvent
{
    public long TransactionNumber { get; }
    public decimal Amount { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public PaymentProcessedEvent(long transactionNumber, decimal amount)
    {
        TransactionNumber = transactionNumber;
        Amount = amount;
    }
}
