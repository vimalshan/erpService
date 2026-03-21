using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Entities;

public class RuleStay : BaseEntity
{
    public string? UnitCode { get; set; }
    public string? BandCode { get; set; }
    public long? StayType { get; set; }
    public decimal? BudgetAmount { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ClosureDate { get; set; }
}
