namespace MasterDataService.Domain.ValueObjects;

public sealed record DateRange
{
    public DateTime EffectiveDate { get; }
    public DateTime? ClosingDate { get; }

    private DateRange(DateTime effectiveDate, DateTime? closingDate)
    {
        EffectiveDate = effectiveDate;
        ClosingDate = closingDate;
    }

    public static DateRange Create(DateTime effectiveDate, DateTime? closingDate = null)
    {
        if (closingDate.HasValue && closingDate.Value < effectiveDate)
            throw new ArgumentException("Closing date cannot be before the effective date.");

        return new DateRange(effectiveDate, closingDate);
    }

    public bool IsActive(DateTime? asOf = null)
    {
        var date = asOf ?? DateTime.UtcNow;
        return date >= EffectiveDate && (!ClosingDate.HasValue || date <= ClosingDate.Value);
    }
}
