namespace EmployeeTransactionsService.Domain.ValueObjects;

public sealed record EffectivePeriod(DateTime EffectiveDate, DateTime? CloseDate)
{
    public static EffectivePeriod Create(DateTime effectiveDate, DateTime? closeDate)
    {
        if (closeDate.HasValue && closeDate.Value < effectiveDate)
            throw new ArgumentException("Close date cannot be before effective date.", nameof(closeDate));

        return new EffectivePeriod(effectiveDate, closeDate);
    }
}