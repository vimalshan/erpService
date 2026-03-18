using AlertsNotifications.Domain.Common;

namespace AlertsNotifications.Domain.Entities;

public class Circular : AuditableEntity
{
    public long CircularId { get; set; }
    public string? CircularNo { get; set; }
    public int CircularYearId { get; set; }
    public long CircularType { get; set; }
    public long CircularOrgId { get; set; }
    public int CircularBuSpecific { get; set; }
    public int CircularUnitSpecific { get; set; }
    public int? CircularHrRoleId { get; set; }
    public int CircularVersionNo { get; set; }
    public long? CircularTemplateId { get; set; }
    public string? CircularPdfFileName { get; set; }
    public string? CircularRtf { get; set; }
    public long CircularSignatoryId { get; set; }
    public char CircularSparshFlag { get; set; }
    public DateTime? CircularPostDate { get; set; }
    public DateTime? CircularRemoveDate { get; set; }
    public string CircularDesc { get; set; } = string.Empty;
    public string CircularSubject { get; set; } = string.Empty;
    public string CircularToList { get; set; } = string.Empty;
    public string? CircularCcList { get; set; }
    public char CircularStatus { get; set; }
    public char? CircularAttachEmpFlag { get; set; }
    public long? CircularApprovedBy { get; set; }
    public DateTime? CircularApprovedOn { get; set; }
    public string? CircularAppRemarks { get; set; }

    // Navigation properties
    public ICollection<CircularSignatory> Signatories { get; set; } = new List<CircularSignatory>();
    public CircularTemplate? Template { get; set; }
}
