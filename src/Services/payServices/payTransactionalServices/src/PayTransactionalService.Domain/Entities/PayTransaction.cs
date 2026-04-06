using PayTransactionalService.Domain.Common;
using PayTransactionalService.Domain.ValueObjects;

namespace PayTransactionalService.Domain.Entities;

/// <summary>
/// Aggregate root for Pay Transaction Detail (PAY_TRANDET)
/// Represents payroll disbursement transactions per employee per month
/// </summary>
public sealed class PayTransaction : AuditableEntity
{
    public long Id { get; set; }
    public long EmployeeSystemId { get; set; }
    public string MonthYear { get; set; } = null!; // YYYY-MM
    public Money GrossAmount { get; set; } = null!;
    public Money Deductions { get; set; } = null!;
    public Money NetAmount { get; set; } = null!;
    public long? BatchId { get; set; }
    public string Status { get; set; } = "P"; // P=Processing, C=Complete, R=Revoked
    public string? Remarks { get; set; }

    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private PayTransaction() { }

    public static PayTransaction Create(
        long employeeSystemId,
        string monthYear,
        decimal grossAmount,
        decimal deductions,
        string createdBy,
        long? batchId = null)
    {
        var gross = new Money(grossAmount);
        var ded = new Money(deductions);
        var net = new Money(grossAmount - deductions);

        var txn = new PayTransaction
        {
            EmployeeSystemId = employeeSystemId,
            MonthYear = monthYear,
            GrossAmount = gross,
            Deductions = ded,
            NetAmount = net,
            BatchId = batchId,
            Status = "P",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        txn._domainEvents.Add(new PayTransactionCreatedEvent(
            txn.EmployeeSystemId, txn.MonthYear, net));

        return txn;
    }

    public void Complete()
    {
        if (Status != "P")
            throw new InvalidOperationException($"Cannot complete transaction in status '{Status}'");
        Status = "C";
        ModifiedAt = DateTime.UtcNow;
        _domainEvents.Add(new PayTransactionCompletedEvent(Id, EmployeeSystemId, MonthYear, NetAmount));
    }

    public void Revoke(string revokedBy, string? reason = null)
    {
        if (Status == "R")
            throw new InvalidOperationException("Transaction is already revoked");
        Status = "R";
        Remarks = reason ?? "Revoked";
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = revokedBy;
        _domainEvents.Add(new PayTransactionRevokedEvent(Id, EmployeeSystemId, revokedBy));
    }

    public void Recalculate(decimal grossAmount, decimal deductions)
    {
        GrossAmount = new Money(grossAmount);
        Deductions = new Money(deductions);
        NetAmount = new Money(grossAmount - deductions);
        ModifiedAt = DateTime.UtcNow;
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}

// Domain Events
public sealed class PayTransactionCreatedEvent : DomainEvent
{
    public long EmployeeSystemId { get; }
    public string MonthYear { get; }
    public Money NetAmount { get; }

    public PayTransactionCreatedEvent(long employeeSystemId, string monthYear, Money netAmount)
    {
        EmployeeSystemId = employeeSystemId;
        MonthYear = monthYear;
        NetAmount = netAmount;
    }
}

public sealed class PayTransactionCompletedEvent : DomainEvent
{
    public long TransactionId { get; }
    public long EmployeeSystemId { get; }
    public string MonthYear { get; }
    public Money NetAmount { get; }

    public PayTransactionCompletedEvent(long transactionId, long employeeSystemId, string monthYear, Money netAmount)
    {
        TransactionId = transactionId;
        EmployeeSystemId = employeeSystemId;
        MonthYear = monthYear;
        NetAmount = netAmount;
    }
}

public sealed class PayTransactionRevokedEvent : DomainEvent
{
    public long TransactionId { get; }
    public long EmployeeSystemId { get; }
    public string RevokedBy { get; }

    public PayTransactionRevokedEvent(long transactionId, long employeeSystemId, string revokedBy)
    {
        TransactionId = transactionId;
        EmployeeSystemId = employeeSystemId;
        RevokedBy = revokedBy;
    }
}
