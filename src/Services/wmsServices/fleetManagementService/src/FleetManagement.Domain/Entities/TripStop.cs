using FleetManagement.Domain.Common;

namespace FleetManagement.Domain.Entities;

public class TripStop : BaseEntity
{
    public int StopId { get; set; }
    public int TripId { get; set; }
    public int StopSequence { get; set; }
    public string? StopType { get; set; }
    public string? LocationType { get; set; }
    public int? LocationId { get; set; }
    public string? Address { get; set; }
    public DateTime? PlannedArrival { get; set; }
    public DateTime? ActualArrival { get; set; }
    public DateTime? PlannedDeparture { get; set; }
    public DateTime? ActualDeparture { get; set; }
    public string Status { get; set; } = "PENDING";
    public string? Notes { get; set; }

    // Navigation
    public Trip Trip { get; set; } = null!;
}
