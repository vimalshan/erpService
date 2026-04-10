using TaskTransactional.Domain.Common;

namespace TaskTransactional.Domain.Entities;

public class ComplaintEscalation : BaseEntity
{
    public decimal CeTicketNum { get; private set; }
    public decimal CeLevelNum { get; private set; }
    public decimal CeEscNoHrs { get; private set; }
    public decimal CeUserPin { get; private set; }
    public DateTime CeEffDate { get; private set; }
    public DateTime? CeClsDate { get; private set; }
    public string? CeExclude { get; private set; }
    public decimal? CeUpdatedBy { get; private set; }
    public DateTime? CeUpdatedOn { get; private set; }

    private ComplaintEscalation() { }

    public static ComplaintEscalation Create(
        decimal ticketNum, decimal levelNum, decimal escNoHrs, decimal userPin)
    {
        return new ComplaintEscalation
        {
            CeTicketNum = ticketNum,
            CeLevelNum = levelNum,
            CeEscNoHrs = escNoHrs,
            CeUserPin = userPin,
            CeEffDate = DateTime.UtcNow
        };
    }

    public void Close(decimal updatedBy)
    {
        CeClsDate = DateTime.UtcNow;
        CeUpdatedBy = updatedBy;
        CeUpdatedOn = DateTime.UtcNow;
    }

    public void SetExclude(string exclude, decimal updatedBy)
    {
        CeExclude = exclude;
        CeUpdatedBy = updatedBy;
        CeUpdatedOn = DateTime.UtcNow;
    }
}
