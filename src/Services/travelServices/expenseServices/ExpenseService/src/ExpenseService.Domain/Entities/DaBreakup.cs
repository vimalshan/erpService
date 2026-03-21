using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Entities;

public class DaBreakup : BaseEntity
{
    public long RequestId { get; set; }
    public long SerialNumber { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? TypeCode { get; set; }
    public decimal? Hours { get; set; }
}
