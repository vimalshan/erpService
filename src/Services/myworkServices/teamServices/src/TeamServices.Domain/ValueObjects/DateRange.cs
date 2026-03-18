namespace TeamServices.Domain.ValueObjects;

using TeamServices.Domain.Common;

public class DateRange : ValueObject
{
    public DateTime EffectiveDate { get; private set; }
    public DateTime? CloseDate { get; private set; }

    private DateRange() { }

    public DateRange(DateTime effectiveDate, DateTime? closeDate = null)
    {
        if (closeDate.HasValue && closeDate.Value < effectiveDate)
            throw new ArgumentException("Close date cannot be before effective date.");

        EffectiveDate = effectiveDate;
        CloseDate = closeDate;
    }

    public bool IsActive(DateTime asOfDate)
    {
        return asOfDate >= EffectiveDate && (!CloseDate.HasValue || asOfDate <= CloseDate.Value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return EffectiveDate;
        yield return CloseDate ?? DateTime.MaxValue;
    }
}
