using CSA.Service.Domain.Common;

namespace CSA.Service.Domain.Entities;

public class UnitMapDetail : AuditableEntity
{
    public long MapId { get; set; }
    public long ControlId { get; set; }
    public long UnitId { get; set; }
    public long OwnerId { get; set; }
    public long ApproverId { get; set; }
    public char ReportingManager { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ClosureDate { get; set; }
    public DateTime DueDate { get; set; }

    // Navigation
    public Control? Control { get; set; }
    public Unit? Unit { get; set; }
}
