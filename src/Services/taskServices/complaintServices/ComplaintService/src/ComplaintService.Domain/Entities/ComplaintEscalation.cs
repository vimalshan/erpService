using ComplaintService.Domain.Common;

namespace ComplaintService.Domain.Entities;

/// <summary>Maps to COMPL_ESC — escalation tracking.</summary>
public class ComplaintEscalation : BaseEntity
{
    public decimal TicketNum { get; private set; }      // CE_TICKET_NUM
    public decimal LevelNum { get; private set; }       // CE_LEVEL_NUM
    public decimal EscNoHrs { get; private set; }       // CE_ESC_NOHRS
    public decimal UserPin { get; private set; }        // CE_USER_PIN
    public DateTime EffDate { get; private set; }       // CE_EFF_DATE
    public DateTime? ClsDate { get; private set; }      // CE_CLS_DATE
    public char? Exclude { get; private set; }          // CE_EXCLUDE
    public decimal? UpdatedBy { get; private set; }     // CE_UPDATEDBY
    public DateTime? UpdatedOn { get; private set; }    // CE_UPDATEDON

    protected ComplaintEscalation() { }

    public static ComplaintEscalation Create(decimal ticketNum, decimal levelNum, decimal noHrs, decimal userPin) =>
        new()
        {
            TicketNum = ticketNum,
            LevelNum = levelNum,
            EscNoHrs = noHrs,
            UserPin = userPin,
            EffDate = DateTime.UtcNow
        };

    public void Close(decimal updatedBy)
    {
        ClsDate = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
