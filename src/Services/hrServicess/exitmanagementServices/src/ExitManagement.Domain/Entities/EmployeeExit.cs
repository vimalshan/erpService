using ExitManagement.Domain.Common;

namespace ExitManagement.Domain.Entities;

/// <summary>
/// Maps to TTBT_EXIT_TEV - Main employee exit record.
/// </summary>
public class EmployeeExit : BaseEntity
{
    public decimal ExitNo { get; private set; }
    public decimal EmployeeSysId { get; private set; }
    public DateTime? LetterGivenOn { get; private set; }
    public DateTime? ExpectedRelieveDate { get; private set; }
    public string? ResignationType { get; private set; }
    public decimal ResignationId { get; private set; }
    public string? Remarks { get; private set; }
    public string? Status { get; private set; }
    public DateTime? RelieveGivenOn { get; private set; }
    public DateTime? InterviewCondductedOn { get; private set; }
    public string? InterviewConductedBy { get; private set; }
    public string? RevokeReason { get; private set; }
    public DateTime? RevokeDate { get; private set; }
    public decimal? SignId { get; private set; }
    public string? RevokeResignation { get; private set; }
    public string? PayrollSettlement { get; private set; }
    public DateTime? StopSalaryDate { get; private set; }
    public decimal? NextOfficer { get; private set; }
    public decimal? SettlementTypeId { get; private set; }
    public DateTime? MailDisableDate { get; private set; }
    public DateTime? MailDeleteDate { get; private set; }
    public decimal? MailForwardSysId { get; private set; }
    public decimal? FormalityBy { get; private set; }
    public DateTime? FormalityOn { get; private set; }
    public string? UserConfirmStatus { get; private set; }
    public string? BypassFormality { get; private set; }
    public string? ApprovalStatus { get; private set; }
    public decimal? ApprovedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    public DateTime? NoticeDate { get; private set; }
    public string? MailStatus { get; private set; }
    public string? CheckStatus { get; private set; }
    public decimal? LastSerialNo { get; private set; }
    public decimal? FirstSerialNo { get; private set; }
    public decimal? FsBy { get; private set; }
    public DateTime? FsOn { get; private set; }
    public decimal? NoticePeriodPaid { get; private set; }
    public string? MailToUser { get; private set; }
    public string? ConductDescription { get; private set; }
    public decimal? JvBatchId { get; private set; }
    public decimal? JvPostedBy { get; private set; }
    public DateTime? JvPostedOn { get; private set; }
    public string? DesignationOnJoining { get; private set; }
    public string? ReasonForLeaving { get; private set; }

    private EmployeeExit() { }

    public static EmployeeExit Create(
        decimal exitNo,
        decimal employeeSysId,
        decimal resignationId,
        string? resignationType,
        DateTime? expectedRelieveDate,
        string? remarks)
    {
        return new EmployeeExit
        {
            ExitNo = exitNo,
            EmployeeSysId = employeeSysId,
            ResignationId = resignationId,
            ResignationType = resignationType,
            ExpectedRelieveDate = expectedRelieveDate,
            Remarks = remarks,
            Status = "I",
            LetterGivenOn = DateTime.UtcNow
        };
    }

    public void Approve(decimal approvedBy)
    {
        ApprovalStatus = "A";
        ApprovedBy = approvedBy;
        ApprovedOn = DateTime.UtcNow;
        Status = "A";
    }

    public void Revoke(string reason, decimal revokedBy)
    {
        RevokeReason = reason;
        RevokeDate = DateTime.UtcNow;
        RevokeResignation = "Y";
        Status = "R";
        UpdatedBy = revokedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void UpdateFormalityStatus(decimal formalityBy)
    {
        FormalityBy = formalityBy;
        FormalityOn = DateTime.UtcNow;
        Status = "F";
    }

    public void CompletePayrollSettlement(DateTime stopSalaryDate, decimal updatedBy)
    {
        PayrollSettlement = "Y";
        StopSalaryDate = stopSalaryDate;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
