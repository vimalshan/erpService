using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Entities;

public class DaRule : BaseEntity
{
    public long SerialNumber { get; set; }
    public long BandId { get; set; }
    public long CountryCode { get; set; }
    public string SelfBookingFlag { get; set; } = "N";
    public string CurrencyCode { get; set; } = "INR";
    public decimal BudgetAmount { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ClosureDate { get; set; }
}
