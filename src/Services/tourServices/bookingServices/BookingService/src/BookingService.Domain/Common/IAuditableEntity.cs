namespace BookingService.Domain.Common;

public interface IAuditableEntity
{
    string? EnteredBy { get; set; }
    DateTime? EnteredOn { get; set; }
}
