using TaskTransactional.Domain.Common;
using TaskTransactional.Domain.Events;

namespace TaskTransactional.Domain.Entities;

public class ComplaintAction : AggregateRoot
{
    public decimal CaActionNum { get; private set; }
    public decimal CaTaskNum { get; private set; }
    public string? CaPrmResp { get; private set; }
    public decimal? CaPrmActBy { get; private set; }
    public DateTime? CaPrmActDate { get; private set; }
    public string? CaPrmSolution { get; private set; }
    public decimal? CaSecEscHrs { get; private set; }
    public string? CaSecResp { get; private set; }
    public decimal? CaSecActBy { get; private set; }
    public DateTime? CaSecActDate { get; private set; }
    public string? CaSecSolution { get; private set; }
    public string? CaFwdRemarks { get; private set; }
    public string? CaFwdResp { get; private set; }
    public decimal? CaFwdActBy { get; private set; }
    public DateTime? CaFwdActDate { get; private set; }
    public string? CaFwdSolution { get; private set; }
    public decimal? CaCurEscLevel { get; private set; }
    public string? CaCorrActReq { get; private set; }
    public string? CaCorrRemarks { get; private set; }
    public string? CaCorrResp { get; private set; }
    public decimal? CaCorrActBy { get; private set; }
    public DateTime? CaCorrActDate { get; private set; }
    public string? CaCorrSolution { get; private set; }
    public string? CaReopenFlg { get; private set; }
    public string? CaReopenRemarks { get; private set; }
    public DateTime? CaTrgDat { get; private set; }
    public DateTime? CaClsDat { get; private set; }
    public decimal? CaUpdatedBy { get; private set; }

    private ComplaintAction() { }

    public static ComplaintAction Create(decimal actionNum, decimal taskNum)
    {
        var entity = new ComplaintAction
        {
            CaActionNum = actionNum,
            CaTaskNum = taskNum,
            CaTrgDat = DateTime.UtcNow,
            CaCurEscLevel = 0
        };

        entity.AddDomainEvent(new ActionCreatedEvent(actionNum, taskNum));
        return entity;
    }

    public void SetPrimaryAction(string? resp, decimal actBy, string? solution)
    {
        CaPrmResp = resp;
        CaPrmActBy = actBy;
        CaPrmActDate = DateTime.UtcNow;
        CaPrmSolution = solution;
        CaUpdatedBy = actBy;
        AddDomainEvent(new ActionUpdatedEvent(CaActionNum, "Primary"));
    }

    public void SetSecondaryAction(string? resp, decimal actBy, string? solution, decimal? escHrs)
    {
        CaSecResp = resp;
        CaSecActBy = actBy;
        CaSecActDate = DateTime.UtcNow;
        CaSecSolution = solution;
        CaSecEscHrs = escHrs;
        CaUpdatedBy = actBy;
        AddDomainEvent(new ActionUpdatedEvent(CaActionNum, "Secondary"));
    }

    public void SetForwardAction(string? remarks, string? resp, decimal actBy, string? solution)
    {
        CaFwdRemarks = remarks;
        CaFwdResp = resp;
        CaFwdActBy = actBy;
        CaFwdActDate = DateTime.UtcNow;
        CaFwdSolution = solution;
        CaUpdatedBy = actBy;
        AddDomainEvent(new ActionUpdatedEvent(CaActionNum, "Forward"));
    }

    public void SetCorrectiveAction(string? actReq, string? remarks, string? resp, decimal actBy, string? solution)
    {
        CaCorrActReq = actReq;
        CaCorrRemarks = remarks;
        CaCorrResp = resp;
        CaCorrActBy = actBy;
        CaCorrActDate = DateTime.UtcNow;
        CaCorrSolution = solution;
        CaUpdatedBy = actBy;
        AddDomainEvent(new ActionUpdatedEvent(CaActionNum, "Corrective"));
    }

    public void Close()
    {
        CaClsDat = DateTime.UtcNow;
    }

    public void Reopen(string? remarks)
    {
        CaReopenFlg = "Y";
        CaReopenRemarks = remarks;
        CaClsDat = null;
    }

    public void Escalate(decimal level)
    {
        CaCurEscLevel = level;
    }
}
