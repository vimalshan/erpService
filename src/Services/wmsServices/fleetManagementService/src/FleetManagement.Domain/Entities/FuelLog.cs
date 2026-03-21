using FleetManagement.Domain.Common;

namespace FleetManagement.Domain.Entities;

public class FuelLog : BaseEntity
{
    public int FuelLogId { get; set; }
    public int VehicleId { get; set; }
    public DateTime FuelDate { get; set; } = DateTime.UtcNow;
    public decimal? Gallons { get; set; }
    public decimal? Cost { get; set; }
    public int? OdometerReading { get; set; }
    public string? Notes { get; set; }

    // Navigation
    public Vehicle Vehicle { get; set; } = null!;
}
