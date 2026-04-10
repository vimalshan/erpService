using TaskTransactional.Domain.Common;

namespace TaskTransactional.Domain.Entities;

public class ComplaintHistory : BaseEntity
{
    public decimal ChHistoryNum { get; private set; }
    public decimal ChActionNum { get; private set; }
    public decimal ChSerialNum { get; private set; }
    public string? ChFrom { get; private set; }
    public string? ChTo { get; private set; }
    public DateTime ChActionDate { get; private set; }
    public string ChActionType { get; private set; } = null!;
    public string? ChRemarks { get; private set; }
    public decimal? ChUpdatedBy { get; private set; }
    public DateTime? ChUpdatedOn { get; private set; }
    public string? ChFilePath { get; private set; }

    private ComplaintHistory() { }

    public static ComplaintHistory Create(
        decimal historyNum, decimal actionNum, decimal serialNum,
        string from, string to, string actionType, string? remarks = null)
    {
        return new ComplaintHistory
        {
            ChHistoryNum = historyNum,
            ChActionNum = actionNum,
            ChSerialNum = serialNum,
            ChFrom = from,
            ChTo = to,
            ChActionDate = DateTime.UtcNow,
            ChActionType = actionType,
            ChRemarks = remarks
        };
    }
}
