using FleetManagement.Domain.Common;

namespace FleetManagement.Domain.Entities;

public class Driver : AuditableEntity
{
    public int DriverId { get; set; }
    public string Code { get; set; } = null!;
    public int? EmployeeId { get; set; }
    public string FullName { get; set; } = null!;
    public string LicenseNumber { get; set; } = null!;
    public DateTime LicenseExpiry { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<Trip> Trips { get; set; } = [];

    public bool IsLicenseValid() => LicenseExpiry > DateTime.UtcNow;
}
