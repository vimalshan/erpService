namespace EmployeeManagement.Domain.ValueObjects;

public sealed class DateRange
{
    public DateTime? EffectiveDate { get; }
    public DateTime? ClosureDate { get; }

    public DateRange(DateTime? effectiveDate, DateTime? closureDate)
    {
        if (effectiveDate.HasValue && closureDate.HasValue && closureDate < effectiveDate)
            throw new ArgumentException("Closure date cannot be before effective date.");

        EffectiveDate = effectiveDate;
        ClosureDate = closureDate;
    }

    public bool IsActive => EffectiveDate.HasValue &&
        (!ClosureDate.HasValue || ClosureDate > DateTime.UtcNow);
}
