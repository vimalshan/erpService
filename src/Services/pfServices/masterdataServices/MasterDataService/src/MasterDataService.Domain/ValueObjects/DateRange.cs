namespace MasterDataService.Domain.ValueObjects;

public sealed record DateRange
{
    public DateTime EffectiveDate { get; init; }
    public DateTime? ClosingDate { get; init; }

    public DateRange(DateTime effectiveDate, DateTime? closingDate = null)
    {
        if (closingDate.HasValue && closingDate.Value < effectiveDate)
            throw new ArgumentException("Closing date cannot be before effective date.");

        EffectiveDate = effectiveDate;
        ClosingDate = closingDate;
    }

    public bool IsActive => !ClosingDate.HasValue || ClosingDate.Value >= DateTime.UtcNow;
}
