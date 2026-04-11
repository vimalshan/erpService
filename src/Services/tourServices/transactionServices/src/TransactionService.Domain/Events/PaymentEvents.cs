using TransactionService.Domain.Common;

namespace TransactionService.Domain.Events;

public sealed record EmployeePaymentCreatedEvent(
    Guid EventId,
    long PaymentId,
    long EmployeeSysId,
    string PaymentType,
    decimal Amount,
    DateTime OccurredOn) : IDomainEvent;

public sealed record AirlineInvoiceCreatedEvent(
    Guid EventId,
    string InvoiceId,
    string BookConfirmationId,
    string InvoiceNumber,
    DateTime OccurredOn) : IDomainEvent;
