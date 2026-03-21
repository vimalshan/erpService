using ExpenseService.Domain.Common;

namespace ExpenseService.Domain.Entities;

public class TravelExpenseSub : BaseEntity
{
    public long? RequestNumber { get; set; }
    public long? SerialNumber { get; set; }
    public long? ExpenseType { get; set; }
    public string? BillAttached { get; set; }
    public string? CityName { get; set; }
    public long? TotalAmount { get; set; }
    public string? StatusCode { get; set; }
    public string? Remarks { get; set; }
    public DateTime? BillDate { get; set; }

    // Navigation
    public TravelExpense? TravelExpense { get; set; }
}
