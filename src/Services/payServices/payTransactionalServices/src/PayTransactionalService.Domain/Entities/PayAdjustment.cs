using PayTransactionalService.Domain.Common;
using PayTransactionalService.Domain.ValueObjects;

namespace PayTransactionalService.Domain.Entities;

/// <summary>
/// Aggregate root for Pay Adjustment Work (PAY_ADJWRK)
/// Represents salary adjustments for employees
/// </summary>
public sealed class PayAdjustment : AuditableEntity
{
    public long Id { get; set; }
    public long EmployeeSystemId { get; set; }
    public string AdjustmentType { get; set; } = null!; // INCREMENT, BONUS, CORRECTION, ARREAR_SETTLE
    public Money Amount { get; set; } = null!;
    public string MonthYear { get; set; } = null!; // YYYY-MM
    public DateTime EffectiveDate { get; set; }
    public string? Reason { get; set; }
    public string Status { get; set; } = "P"; // P=Pending, A=Approved, R=Rejected
    public long? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }

    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private PayAdjustment() { }

    public static PayAdjustment Create(
        long employeeSystemId,
        string adjustmentType,
        decimal amount,
        string monthYear,
        DateTime effectiveDate,
        string createdBy,
        string? reason = null)
    {
        var adj = new PayAdjustment
        {
            EmployeeSystemId = employeeSystemId,
            AdjustmentType = adjustmentType,
            Amount = new Money(amount),
            MonthYear = monthYear,
            EffectiveDate = effectiveDate,
            Reason = reason,
            Status = "P",
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        adj._domainEvents.Add(new PayAdjustmentCreatedEvent(
            employeeSystemId, adjustmentType, amount, monthYear));

        return adj;
    }

    public void Approve(long approvedBy)
    {
        if (Status != "P")
            throw new InvalidOperationException($"Cannot approve adjustment in status '{Status}'");
        Status = "A";
        ApprovedBy = approvedBy;
        ApprovedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;
        _domainEvents.Add(new PayAdjustmentApprovedEvent(Id, EmployeeSystemId, approvedBy));
    }

    public void Reject(long rejectedBy, string? reason = null)
    {
        if (Status != "P")
            throw new InvalidOperationException($"Cannot reject adjustment in status '{Status}'");
        Status = "R";
        Reason = reason ?? Reason;
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = rejectedBy.ToString();
        _domainEvents.Add(new PayAdjustmentRejectedEvent(Id, EmployeeSystemId));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}

public sealed class PayAdjustmentCreatedEvent : DomainEvent
{
    public long EmployeeSystemId { get; }
    public string AdjustmentType { get; }
    public decimal Amount { get; }
    public string MonthYear { get; }

    public PayAdjustmentCreatedEvent(long employeeSystemId, string adjustmentType, decimal amount, string monthYear)
    {
        EmployeeSystemId = employeeSystemId;
        AdjustmentType = adjustmentType;
        Amount = amount;
        MonthYear = monthYear;
    }
}

public sealed class PayAdjustmentApprovedEvent : DomainEvent
{
    public long AdjustmentId { get; }
    public long EmployeeSystemId { get; }
    public long ApprovedBy { get; }

    public PayAdjustmentApprovedEvent(long adjustmentId, long employeeSystemId, long approvedBy)
    {
        AdjustmentId = adjustmentId;
        EmployeeSystemId = employeeSystemId;
        ApprovedBy = approvedBy;
    }
}

public sealed class PayAdjustmentRejectedEvent : DomainEvent
{
    public long AdjustmentId { get; }
    public long EmployeeSystemId { get; }

    public PayAdjustmentRejectedEvent(long adjustmentId, long employeeSystemId)
    {
        AdjustmentId = adjustmentId;
        EmployeeSystemId = employeeSystemId;
    }
}
