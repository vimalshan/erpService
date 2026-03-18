namespace VendorService.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
