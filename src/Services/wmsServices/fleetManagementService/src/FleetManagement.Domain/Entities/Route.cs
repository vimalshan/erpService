using FleetManagement.Domain.Common;

namespace FleetManagement.Domain.Entities;

public class Route : BaseEntity
{
    public int RouteId { get; set; }
    public string RouteName { get; set; } = null!;
    public string? Description { get; set; }
    public string? StartLocation { get; set; }
    public string? EndLocation { get; set; }
    public int? EstimatedDuration { get; set; } // in minutes
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Trip> Trips { get; set; } = [];
}
