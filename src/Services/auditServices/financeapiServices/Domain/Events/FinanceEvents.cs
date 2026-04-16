using MediatR;

namespace FinanceService.Domain.Events;

public interface IDomainEvent : INotification { }

public record InvoiceCreatedEvent(int InvoiceId, string InvoiceNumber, int CompanyId, decimal TotalAmount) : IDomainEvent;
public record InvoiceStatusChangedEvent(int InvoiceId, string OldStatus, string NewStatus) : IDomainEvent;
public record InvoicePaidEvent(int InvoiceId, string InvoiceNumber, DateTime PaidDate, string? PaymentReference) : IDomainEvent;
public record InvoiceOverdueEvent(int InvoiceId, string InvoiceNumber, DateTime DueDate) : IDomainEvent;
