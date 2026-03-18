namespace PayrollServices.Domain.Events;

/// <summary>
/// Published when a payroll transaction is processed
/// </summary>
public record PayrollTransactionProcessedEvent(
    long TransactionId,
    long EmployeeSystemId,
    decimal NetSalary,
    string Month,
    long ProcessedBy) : DomainEvent;
