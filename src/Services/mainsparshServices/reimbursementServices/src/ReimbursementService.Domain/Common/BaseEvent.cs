namespace ReimbursementService.Domain.Common;

public abstract class BaseEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
