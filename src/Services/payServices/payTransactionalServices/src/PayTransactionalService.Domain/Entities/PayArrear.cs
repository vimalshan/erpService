using PayTransactionalService.Domain.Common;
using PayTransactionalService.Domain.ValueObjects;

namespace PayTransactionalService.Domain.Entities;

/// <summary>
/// Aggregate root for Pay Arrear/Allowance/Deduction (PAY_ARR)
/// Represents arrear, allowance, or deduction line items
/// </summary>
public sealed class PayArrear : AuditableEntity
{
    public long Id { get; set; }
    public long EmployeeSystemId { get; set; }
    public Money Amount { get; set; } = null!;
    public string Type { get; set; } = null!; // A=Allowance, D=Deduction
    public string? Code { get; set; } // e.g. HRA, DA, LTA, PF, ESI
    public string? Description { get; set; }
    public DateTime PayDate { get; set; }
    public string MonthYear { get; set; } = null!; // YYYY-MM
    public bool IsProcessed { get; set; }

    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private PayArrear() { }

    public static PayArrear Create(
        long employeeSystemId,
        decimal amount,
        string type,
        string monthYear,
        string createdBy,
        string? code = null,
        string? description = null)
    {
        if (type != "A" && type != "D")
            throw new ArgumentException("Type must be 'A' (Allowance) or 'D' (Deduction)");

        if (amount < 0 && type != "D")
            throw new ArgumentException("Amount cannot be negative for non-deduction type");

        var arrear = new PayArrear
        {
            EmployeeSystemId = employeeSystemId,
            Amount = new Money(amount),
            Type = type,
            Code = code,
            Description = description,
            PayDate = DateTime.UtcNow,
            MonthYear = monthYear,
            IsProcessed = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        arrear._domainEvents.Add(new PayArrearCreatedEvent(
            employeeSystemId, amount, type, monthYear));

        return arrear;
    }

    public void MarkProcessed()
    {
        IsProcessed = true;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateAmount(decimal newAmount)
    {
        if (newAmount < 0 && Type != "D")
            throw new ArgumentException("Amount cannot be negative for non-deduction type");
        Amount = new Money(newAmount);
        ModifiedAt = DateTime.UtcNow;
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}

public sealed class PayArrearCreatedEvent : DomainEvent
{
    public long EmployeeSystemId { get; }
    public decimal Amount { get; }
    public string Type { get; }
    public string MonthYear { get; }

    public PayArrearCreatedEvent(long employeeSystemId, decimal amount, string type, string monthYear)
    {
        EmployeeSystemId = employeeSystemId;
        Amount = amount;
        Type = type;
        MonthYear = monthYear;
    }
}
