using VehicleTracking.Domain.Common;

namespace VehicleTracking.Domain.Entities;

public class SparshNavigation : BaseEntity
{
    public long RequestNumber { get; set; }
    public string UserId { get; set; } = string.Empty;
    public long UserNumber { get; set; }
    public string? RandomNumber { get; set; }
    public DateTime UpdateDate { get; set; }
    public char SciId { get; set; }
    public char? StatusFlag { get; set; }
}
