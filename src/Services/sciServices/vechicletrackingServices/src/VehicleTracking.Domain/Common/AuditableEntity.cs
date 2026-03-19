namespace VehicleTracking.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public string? UpdatedBy { get; set; }
    public long UpdateNumber { get; set; }
    public DateTime UpdatedDate { get; set; }
}
