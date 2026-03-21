using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Entities;

public class RuleMode : BaseEntity
{
    public string? UnitCode { get; set; }
    public string? BandCode { get; set; }
    public string? TravelType { get; set; }
    public long? ModeCode { get; set; }
    public string? ClassType { get; set; }
    public decimal? BudgetAmount { get; set; }
}
