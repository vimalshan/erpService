using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Entities;

public class RuleDa : BaseEntity
{
    public string? UnitCode { get; set; }
    public string? GradeCode { get; set; }
    public string? LocationGroup { get; set; }
    public string? TypeCode { get; set; }
    public string? ArrangementSelf { get; set; }
    public string? CurrencyCode { get; set; }
    public string? DaType { get; set; }
    public decimal? BudgetAmount { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ClosureDate { get; set; }
}
