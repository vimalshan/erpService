using InvestmentService.Domain.Common;

namespace InvestmentService.Domain.Entities;

public class ScheduleDetail : BaseEntity
{
    public long SchId { get; set; }
    public long InvNo { get; set; }
    public long SlId { get; set; }
    public string ScheduleType { get; set; } = null!;
    public DateTime InterestFrom { get; set; }
    public DateTime InterestTo { get; set; }
    public decimal InterestOption { get; set; }
    public decimal DueAmount { get; set; }
    public DateTime DueDate { get; set; }
    public decimal? ReceivedAmount { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public long? ReceivedTransactionId { get; set; }
    public long? LogSysId { get; set; }
    public long? Year { get; set; }

    // Navigation
    public Investment Investment { get; set; } = null!;
}
