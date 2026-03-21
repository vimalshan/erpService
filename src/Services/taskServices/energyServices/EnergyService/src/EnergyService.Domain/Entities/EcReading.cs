using EnergyService.Domain.Common;

namespace EnergyService.Domain.Entities;

public class EcReading : AuditableEntity
{
    public int? EbId { get; set; }
    public string EbUnitCode { get; set; } = string.Empty;
    public int EbProcessId { get; set; }
    public DateTime EbDate { get; set; }
    public long? EbTarget { get; set; }
    public long? EbReading { get; set; }
    public long? EbResetReading { get; set; }
    public long? EbActualUsage { get; set; }
    public long? EbToDate { get; set; }
    public string? EbRemarks { get; set; }

    // Navigation
    public EcProcess? Process { get; set; }
}
