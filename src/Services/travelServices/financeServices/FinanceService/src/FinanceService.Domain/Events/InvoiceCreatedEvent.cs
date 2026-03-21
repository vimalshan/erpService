using FinanceService.Domain.Common;

namespace FinanceService.Domain.Events;

public class InvoiceCreatedEvent : IDomainEvent
{
    public long InvoiceId { get; }
    public string? InvoiceNum { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public InvoiceCreatedEvent(long invoiceId, string? invoiceNum)
    {
        InvoiceId = invoiceId;
        InvoiceNum = invoiceNum;
    }
}
