using TaxService.Domain.Common;
using TaxService.Domain.ValueObjects;

namespace TaxService.Domain.Entities;

/// <summary>
/// Aggregate root for Tax Marginal Details (TAX_MARDET)
/// Represents marginal tax computation details
/// </summary>
public sealed class TaxMarginalDetail : AuditableEntity
{
    public long Id { get; set; }
    public long EmployeeSystemId { get; set; }
    public int FinancialYear { get; set; }
    public Money GrossIncome { get; set; } = null!;
    public Money StandardDeduction { get; set; } = null!;
    public Money TaxableIncome { get; set; } = null!;
    public List<TaxRate> ApplicableTaxRates { get; } = new();
    public Money CalculatedTax { get; set; } = null!;
    public string[] Exemptions { get; set; } = Array.Empty<string>();
    public string Remarks { get; set; } = string.Empty;
    
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private TaxMarginalDetail() { }

    public static TaxMarginalDetail Create(
        long employeeSystemId,
        int financialYear,
        Money grossIncome,
        Money standardDeduction,
        string createdBy)
    {
        var taxableIncome = new Money(
            Math.Max(0, grossIncome.Amount - standardDeduction.Amount),
            grossIncome.Currency);

        var detail = new TaxMarginalDetail
        {
            EmployeeSystemId = employeeSystemId,
            FinancialYear = financialYear,
            GrossIncome = grossIncome,
            StandardDeduction = standardDeduction,
            TaxableIncome = taxableIncome,
            CalculatedTax = Money.Zero(grossIncome.Currency),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        detail._domainEvents.Add(new TaxMarginalDetailCreatedEvent(
            employeeSystemId, financialYear, taxableIncome));

        return detail;
    }

    public void CalculateTax(List<TaxRate> taxRates)
    {
        ApplicableTaxRates.Clear();
        ApplicableTaxRates.AddRange(taxRates);

        var totalTax = 0m;
        foreach (var rate in taxRates)
        {
            if (rate.IsApplicable(TaxableIncome.Amount))
            {
                totalTax += rate.CalculateTax(TaxableIncome.Amount);
            }
        }

        CalculatedTax = new Money(totalTax, GrossIncome.Currency);
        _domainEvents.Add(new TaxCalculatedEvent(Id, CalculatedTax));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}

/// <summary>
/// Domain event raised when tax marginal detail is created
/// </summary>
public sealed class TaxMarginalDetailCreatedEvent : DomainEvent
{
    public long EmployeeSystemId { get; }
    public int FinancialYear { get; }
    public Money TaxableIncome { get; }

    public TaxMarginalDetailCreatedEvent(long employeeSystemId, int financialYear, Money taxableIncome)
    {
        EmployeeSystemId = employeeSystemId;
        FinancialYear = financialYear;
        TaxableIncome = taxableIncome;
    }
}

/// <summary>
/// Domain event raised when tax is calculated
/// </summary>
public sealed class TaxCalculatedEvent : DomainEvent
{
    public long TaxMarginalDetailId { get; }
    public Money CalculatedTax { get; }

    public TaxCalculatedEvent(long taxMarginalDetailId, Money calculatedTax)
    {
        TaxMarginalDetailId = taxMarginalDetailId;
        CalculatedTax = calculatedTax;
    }
}
