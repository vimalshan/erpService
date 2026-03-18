using TaxService.Domain.Common;
using TaxService.Domain.ValueObjects;

namespace TaxService.Domain.Entities;

/// <summary>
/// Aggregate root for Conditional Master (CONDED_MAST)
/// Represents payee information and conditional tax details
/// </summary>
public sealed class ConditionalMaster : AuditableEntity
{
    public long Id { get; set; }
    public string PayeeId { get; set; } = null!;
    public string PayeeName { get; set; } = null!;
    public string PayeeAddress { get; set; } = null!;
    public string PayeePAN { get; set; } = string.Empty;
    public string TaxRegime { get; set; } = "Old"; // Old or New
    public int FinancialYear { get; set; }
    public List<TaxExemption> Exemptions { get; } = new();
    public List<TaxDeduction> Deductions { get; } = new();
    public Money TotalExemption { get; set; } = null!;
    public Money TotalDeduction { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private ConditionalMaster() { }

    public static ConditionalMaster Create(
        string payeeId,
        string payeeName,
        string payeeAddress,
        string payeePAN,
        string taxRegime,
        int financialYear,
        string createdBy)
    {
        var master = new ConditionalMaster
        {
            PayeeId = payeeId,
            PayeeName = payeeName,
            PayeeAddress = payeeAddress,
            PayeePAN = payeePAN,
            TaxRegime = taxRegime,
            FinancialYear = financialYear,
            TotalExemption = Money.Zero(),
            TotalDeduction = Money.Zero(),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        master._domainEvents.Add(new ConditionalMasterCreatedEvent(payeeId, payeeName, financialYear));

        return master;
    }

    public void AddExemption(TaxExemption exemption)
    {
        if (Exemptions.Any(e => e.Id == exemption.Id))
            throw new InvalidOperationException($"Exemption {exemption.Id} already exists");

        Exemptions.Add(exemption);
        RecalculateTotals();
    }

    public void AddDeduction(TaxDeduction deduction)
    {
        if (Deductions.Any(d => d.Id == deduction.Id))
            throw new InvalidOperationException($"Deduction {deduction.Id} already exists");

        Deductions.Add(deduction);
        RecalculateTotals();
    }

    public void RemoveExemption(long exemptionId)
    {
        var exemption = Exemptions.FirstOrDefault(e => e.Id == exemptionId);
        if (exemption != null)
        {
            Exemptions.Remove(exemption);
            RecalculateTotals();
        }
    }

    public void RemoveDeduction(long deductionId)
    {
        var deduction = Deductions.FirstOrDefault(d => d.Id == deductionId);
        if (deduction != null)
        {
            Deductions.Remove(deduction);
            RecalculateTotals();
        }
    }

    private void RecalculateTotals()
    {
        var totalExemption = Exemptions.Aggregate(
            Money.Zero(),
            (acc, e) => acc + e.Amount);
        
        var totalDeduction = Deductions.Aggregate(
            Money.Zero(),
            (acc, d) => acc + d.Amount);

        TotalExemption = totalExemption;
        TotalDeduction = totalDeduction;
    }

    public void Deactivate()
    {
        IsActive = false;
        _domainEvents.Add(new ConditionalMasterDeactivatedEvent(Id, PayeeId));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}

/// <summary>
/// Value object representing a tax exemption
/// </summary>
public sealed class TaxExemption
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Money Amount { get; set; } = null!;
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
}

/// <summary>
/// Value object representing a tax deduction
/// </summary>
public sealed class TaxDeduction
{
    public long Id { get; set; }
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Money Amount { get; set; } = null!;
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
}

/// <summary>
/// Domain event raised when conditional master is created
/// </summary>
public sealed class ConditionalMasterCreatedEvent : DomainEvent
{
    public string PayeeId { get; }
    public string PayeeName { get; }
    public int FinancialYear { get; }

    public ConditionalMasterCreatedEvent(string payeeId, string payeeName, int financialYear)
    {
        PayeeId = payeeId;
        PayeeName = payeeName;
        FinancialYear = financialYear;
    }
}

/// <summary>
/// Domain event raised when conditional master is deactivated
/// </summary>
public sealed class ConditionalMasterDeactivatedEvent : DomainEvent
{
    public long ConditionalMasterId { get; }
    public string PayeeId { get; }

    public ConditionalMasterDeactivatedEvent(long conditionalMasterId, string payeeId)
    {
        ConditionalMasterId = conditionalMasterId;
        PayeeId = payeeId;
    }
}
