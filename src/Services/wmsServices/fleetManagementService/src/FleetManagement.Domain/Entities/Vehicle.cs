using FleetManagement.Domain.Common;
using FleetManagement.Domain.Enums;
using FleetManagement.Domain.Events;

namespace FleetManagement.Domain.Entities;

public class Vehicle : AuditableEntity
{
    public int VehicleId { get; set; }
    public string Code { get; set; } = null!;
    public string LicensePlate { get; set; } = null!;
    public VehicleType VehicleType { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
    public decimal? CapacityWeight { get; set; }
    public decimal? CapacityVolume { get; set; }
    public VehicleStatus Status { get; set; } = VehicleStatus.AVAILABLE;
    public int? WarehouseId { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<Trip> Trips { get; set; } = [];
    public ICollection<MaintenanceLog> MaintenanceLogs { get; set; } = [];
    public ICollection<FuelLog> FuelLogs { get; set; } = [];

    public void SetMaintenance()
    {
        Status = VehicleStatus.MAINTENANCE;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new VehicleStatusChangedEvent(VehicleId, VehicleStatus.MAINTENANCE));
    }

    public void SetAvailable()
    {
        Status = VehicleStatus.AVAILABLE;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new VehicleStatusChangedEvent(VehicleId, VehicleStatus.AVAILABLE));
    }

    public void Retire()
    {
        Status = VehicleStatus.RETIRED;
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new VehicleStatusChangedEvent(VehicleId, VehicleStatus.RETIRED));
    }
}
