using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Entities;

public class DaSummary : BaseEntity
{
    public long RequestId { get; set; }
    public decimal AdminHours { get; set; }
    public decimal AdminDays { get; set; }
    public decimal AdminRate { get; set; }
    public decimal AdminAmount { get; set; }
    public decimal SelfHours { get; set; }
    public decimal SelfDays { get; set; }
    public decimal SelfRate { get; set; }
    public decimal SelfAmount { get; set; }
}
