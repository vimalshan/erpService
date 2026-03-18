namespace PayrollServices.Domain.Events;

/// <summary>
/// Published when a payroll batch is completed
/// </summary>
public record PayrollBatchCompletedEvent(
    long BatchId,
    string BatchMonth,
    int TransactionCount,
    long CompletedBy) : DomainEvent;
