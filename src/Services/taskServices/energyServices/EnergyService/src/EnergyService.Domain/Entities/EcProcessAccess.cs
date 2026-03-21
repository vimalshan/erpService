using EnergyService.Domain.Common;

namespace EnergyService.Domain.Entities;

public class EcProcessAccess : BaseEntity
{
    public int? PaId { get; set; }
    public int PaProcessId { get; set; }
    public int PaEmpSysId { get; set; }
    public DateTime PaStartDate { get; set; }
    public DateTime? PaCloseDate { get; set; }
    public int PaLastModifiedBy { get; set; }
    public string PaLastModifiedOn { get; set; } = string.Empty;

    // Navigation
    public EcProcess? Process { get; set; }
}
