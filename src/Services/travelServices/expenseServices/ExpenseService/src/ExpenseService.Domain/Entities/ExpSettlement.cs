using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Entities;

public class ExpSettlement : BaseEntity
{
    public long? ExpenseCode { get; set; }
    public string? ExpenseName { get; set; }
    public decimal? BudgetAmount { get; set; }
    public decimal? CompanyAmount { get; set; }
    public decimal? SelfAmount { get; set; }
    public decimal? AnnexureAmount { get; set; }
    public string? Remarks { get; set; }
    public string? Remarks1 { get; set; }
}
