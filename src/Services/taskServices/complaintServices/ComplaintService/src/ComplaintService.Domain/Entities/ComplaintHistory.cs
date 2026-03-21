using ComplaintService.Domain.Common;

namespace ComplaintService.Domain.Entities;

/// <summary>Maps to COMPL_HIST — audit trail of all changes.</summary>
public class ComplaintHistory : BaseEntity
{
    public decimal HistoryNum { get; private set; }     // CH_HISTORY_NUM (PK)
    public decimal ActionNum { get; private set; }      // CH_ACTION_NUM
    public decimal SerialNum { get; private set; }      // CH_SERIAL_NUM
    public string? From { get; private set; }           // CH_FROM
    public string? To { get; private set; }             // CH_TO
    public DateTime ActionDate { get; private set; }    // CH_ACTION_DATE
    public char ActionType { get; private set; }        // CH_ACTION_TYPE
    public string? Remarks { get; private set; }        // CH_REMARKS
    public decimal? UpdatedBy { get; private set; }     // CH_UPDATEDBY
    public DateTime? UpdatedOn { get; private set; }    // CH_UPDATEDON
    public string? FilePath { get; private set; }       // CH_FILEPATH

    protected ComplaintHistory() { }

    public static ComplaintHistory Create(
        decimal historyNum, decimal actionNum, decimal serialNum,
        string? from, string? to, char actionType, string? remarks, decimal? updatedBy = null) =>
        new()
        {
            HistoryNum = historyNum,
            ActionNum = actionNum,
            SerialNum = serialNum,
            From = from,
            To = to,
            ActionDate = DateTime.UtcNow,
            ActionType = actionType,
            Remarks = remarks,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        };
}
