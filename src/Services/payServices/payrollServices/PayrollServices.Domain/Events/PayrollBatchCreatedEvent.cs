namespace PayrollServices.Domain.Events;

/// <summary>
/// Published when a payroll batch is created
/// </summary>
public record PayrollBatchCreatedEvent(
    long BatchId,
    string BatchMonth,
    long CreatedBy) : DomainEvent;
