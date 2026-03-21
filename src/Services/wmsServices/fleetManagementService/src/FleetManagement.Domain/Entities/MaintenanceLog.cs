using FleetManagement.Domain.Common;

namespace FleetManagement.Domain.Entities;

public class MaintenanceLog : BaseEntity
{
    public int LogId { get; set; }
    public int VehicleId { get; set; }
    public DateTime MaintenanceDate { get; set; }
    public string MaintenanceType { get; set; } = null!;
    public string? Description { get; set; }
    public decimal? Cost { get; set; }
    public int? OdometerReading { get; set; }
    public DateTime? NextDueDate { get; set; }
    public string? PerformedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation
    public Vehicle Vehicle { get; set; } = null!;
}
