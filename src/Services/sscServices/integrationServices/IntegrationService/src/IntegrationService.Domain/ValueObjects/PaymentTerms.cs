using IntegrationService.Domain.Common;

namespace IntegrationService.Domain.ValueObjects;

public class PaymentTerms : ValueObject
{
    public long DueDays { get; private set; }
    public long DueDayMonthOffset { get; private set; }
    public long MonthForward { get; private set; }

    private PaymentTerms() { }

    public PaymentTerms(long dueDays, long dueDayMonthOffset, long monthForward)
    {
        if (dueDays < 0) throw new ArgumentException("Due days cannot be negative.", nameof(dueDays));
        DueDays = dueDays;
        DueDayMonthOffset = dueDayMonthOffset;
        MonthForward = monthForward;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return DueDays;
        yield return DueDayMonthOffset;
        yield return MonthForward;
    }
}
