using AimsTransactionService.Domain.Common;

namespace AimsTransactionService.Domain.Entities;

public class AttendanceLopDetail : Entity
{
    public long LopMainId { get; private set; }
    public DateTime LopDate { get; private set; }
    public decimal LopHours { get; private set; }
    public string? LopReason { get; private set; }

    private AttendanceLopDetail() { }

    public static AttendanceLopDetail Create(
        long id,
        long lopMainId,
        DateTime lopDate,
        decimal lopHours,
        string? lopReason)
    {
        return new AttendanceLopDetail
        {
            Id = id,
            LopMainId = lopMainId,
            LopDate = lopDate,
            LopHours = lopHours,
            LopReason = lopReason
        };
    }
}
