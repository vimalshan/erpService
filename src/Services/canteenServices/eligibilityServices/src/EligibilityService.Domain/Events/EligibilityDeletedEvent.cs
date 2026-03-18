using EligibilityService.Domain.Common;

namespace EligibilityService.Domain.Events;

public sealed class EligibilityDeletedEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public long CanteenUnit { get; }
    public string ShiftCode { get; }
    public decimal ItemCode { get; }

    public EligibilityDeletedEvent(long canteenUnit, string shiftCode, decimal itemCode)
    {
        CanteenUnit = canteenUnit;
        ShiftCode = shiftCode;
        ItemCode = itemCode;
    }
}
