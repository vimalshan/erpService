using EnergyService.Domain.Common;

namespace EnergyService.Domain.Entities;

public class EcProcessMailId : BaseEntity
{
    public int? PmId { get; set; }
    public int PmProcessId { get; set; }
    public string PmMailId { get; set; } = string.Empty;
    public string PmDeliveryType { get; set; } = string.Empty;
    public DateTime PmStartDate { get; set; }
    public DateTime? PmCloseDate { get; set; }
    public int PmLastModifiedBy { get; set; }
    public string PmLastModifiedOn { get; set; } = string.Empty;

    // Navigation
    public EcProcess? Process { get; set; }
}
