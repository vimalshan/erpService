using EnergyService.Domain.Common;

namespace EnergyService.Domain.Entities;

public class EcProcess : AuditableEntity
{
    public int EcProcessId { get; set; }
    public string EcProcessDesc { get; set; } = string.Empty;
    public string EcUnitCode { get; set; } = string.Empty;
    public string EcCloseFlag { get; set; } = "N";

    // Navigation properties
    public ICollection<EcProcessAccess> ProcessAccesses { get; set; } = [];
    public ICollection<EcProcessMailId> ProcessMailIds { get; set; } = [];
    public ICollection<EcReading> Readings { get; set; } = [];
}
