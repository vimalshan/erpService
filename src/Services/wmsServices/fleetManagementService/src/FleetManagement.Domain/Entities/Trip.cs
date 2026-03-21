using FleetManagement.Domain.Common;
using FleetManagement.Domain.Enums;
using FleetManagement.Domain.Events;

namespace FleetManagement.Domain.Entities;

public class Trip : AuditableEntity
{
    public int TripId { get; set; }
    public string TripNumber { get; set; } = null!;
    public int? RouteId { get; set; }
    public int VehicleId { get; set; }
    public int DriverId { get; set; }
    public DateTime TripDate { get; set; } = DateTime.UtcNow.Date;
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? OriginType { get; set; }
    public int? OriginId { get; set; }
    public string? DestinationType { get; set; }
    public int? DestinationId { get; set; }
    public TripStatus Status { get; set; } = TripStatus.PLANNED;
    public string? Notes { get; set; }
    public string? CreatedBy { get; set; }

    // Navigation properties
    public Vehicle Vehicle { get; set; } = null!;
    public Driver Driver { get; set; } = null!;
    public Route? Route { get; set; }
    public ICollection<TripStop> Stops { get; set; } = [];

    public void Start(DateTime? startTime = null)
    {
        if (Status != TripStatus.PLANNED)
            throw new InvalidOperationException("Trip can only be started from PLANNED status.");

        StartTime = startTime ?? DateTime.UtcNow;
        Status = TripStatus.IN_PROGRESS;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new TripStatusChangedEvent(TripId, TripStatus.IN_PROGRESS));
    }

    public void Complete(DateTime? endTime = null)
    {
        if (Status != TripStatus.IN_PROGRESS)
            throw new InvalidOperationException("Trip can only be completed from IN_PROGRESS status.");

        EndTime = endTime ?? DateTime.UtcNow;
        Status = TripStatus.COMPLETED;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new TripStatusChangedEvent(TripId, TripStatus.COMPLETED));
    }

    public void Cancel()
    {
        if (Status == TripStatus.COMPLETED)
            throw new InvalidOperationException("Cannot cancel a completed trip.");

        Status = TripStatus.CANCELLED;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new TripStatusChangedEvent(TripId, TripStatus.CANCELLED));
    }
}
