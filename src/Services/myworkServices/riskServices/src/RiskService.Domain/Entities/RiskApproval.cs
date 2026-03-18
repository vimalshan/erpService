using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskApproval : BaseEntity
{
    public long RiskId { get; set; }
    public long ApproverEmployeeSysId { get; set; }
    public char Status { get; set; }  // A/R
    public string Remarks { get; set; } = default!;
    public long LastModifiedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
    public char? ApprovalType { get; set; }  // R=Risk, S=SelfAssessment
}
