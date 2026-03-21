using ComplaintService.Domain.Common;

namespace ComplaintService.Domain.Entities;

/// <summary>Maps to COMPL_ACTION — multi-level resolution tracking.</summary>
public class ComplaintAction : BaseEntity
{
    public decimal ActionNum { get; private set; }          // CA_ACTION_NUM (PK)
    public decimal TaskNum { get; private set; }            // CA_TASK_NUM (FK -> COMPL_DET)
    public string? PrimaryResp { get; private set; }        // CA_PRM_RESP
    public decimal? PrimaryActBy { get; private set; }      // CA_PRM_ACTBY
    public DateTime? PrimaryActDate { get; private set; }   // CA_PRM_ACTDATE
    public string? PrimarySolution { get; private set; }    // CA_PRM_SOLUTION
    public decimal? SecEscHrs { get; private set; }         // CA_SEC_ESCHRS
    public string? SecResp { get; private set; }            // CA_SEC_RESP
    public decimal? SecActBy { get; private set; }          // CA_SEC_ACTBY
    public DateTime? SecActDate { get; private set; }       // CA_SEC_ACTDATE
    public string? SecSolution { get; private set; }        // CA_SEC_SOLUTION
    public string? FwdRemarks { get; private set; }         // CA_FWD_REMARKS
    public string? FwdResp { get; private set; }            // CA_FWD_RESP
    public decimal? FwdActBy { get; private set; }          // CA_FWD_ACTBY
    public DateTime? FwdActDate { get; private set; }       // CA_FWD_ACTDATE
    public string? FwdSolution { get; private set; }        // CA_FWD_SOLUTION
    public decimal? CurrentEscLevel { get; private set; }   // CA_CUR_ESCLEVEL
    public char? CorrActRequired { get; private set; }      // CA_CORR_ACTREQ
    public string? CorrRemarks { get; private set; }        // CA_CORR_REMARKS
    public string? CorrResp { get; private set; }           // CA_CORR_RESP
    public decimal? CorrActBy { get; private set; }         // CA_CORR_ACTBY
    public DateTime? CorrActDate { get; private set; }      // CA_CORR_ACTDATE
    public string? CorrSolution { get; private set; }       // CA_CORR_SOLUTION
    public char? ReopenFlag { get; private set; }           // CA_REOPEN_FLG
    public string? ReopenRemarks { get; private set; }      // CA_REOPEN_REMARKS
    public DateTime? TargetDate { get; private set; }       // CA_TRG_DAT
    public DateTime? ClosureDate { get; private set; }      // CA_CLS_DAT
    public decimal? UpdatedBy { get; private set; }         // CA_UPATEDBY

    // Navigation
    public ComplaintTicket? Ticket { get; private set; }
    public ICollection<ComplaintHistory> Histories { get; private set; } = [];

    protected ComplaintAction() { }

    public static ComplaintAction Create(decimal actionNum, decimal taskNum) =>
        new() { ActionNum = actionNum, TaskNum = taskNum, CurrentEscLevel = 0, TargetDate = DateTime.UtcNow };

    public void RecordPrimaryAction(decimal actBy, string solution, string? resp = null)
    {
        PrimaryActBy = actBy;
        PrimaryActDate = DateTime.UtcNow;
        PrimarySolution = solution;
        PrimaryResp = resp;
    }

    public void RecordSecondaryAction(decimal actBy, string solution, string? resp = null)
    {
        SecActBy = actBy;
        SecActDate = DateTime.UtcNow;
        SecSolution = solution;
        SecResp = resp;
        CurrentEscLevel = 1;
    }

    public void RecordForwardAction(decimal actBy, string solution, string? remarks = null, string? resp = null)
    {
        FwdActBy = actBy;
        FwdActDate = DateTime.UtcNow;
        FwdSolution = solution;
        FwdRemarks = remarks;
        FwdResp = resp;
        CurrentEscLevel = 2;
    }

    public void RecordCorrectiveAction(decimal actBy, string solution, string? remarks = null, string? resp = null)
    {
        CorrActBy = actBy;
        CorrActDate = DateTime.UtcNow;
        CorrSolution = solution;
        CorrRemarks = remarks;
        CorrResp = resp;
        CorrActRequired = 'Y';
    }

    public void Close(decimal closedBy)
    {
        ClosureDate = DateTime.UtcNow;
        UpdatedBy = closedBy;
    }

    public void Reopen(string remarks)
    {
        ReopenFlag = 'Y';
        ReopenRemarks = remarks;
        ClosureDate = null;
    }
}
