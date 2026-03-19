namespace MobileExpenseManagement.Application.DTOs;

/// <summary>
/// DTO for expense response
/// </summary>
public class ExpenseDto
{
    public decimal Id { get; set; }
    public decimal TripId { get; set; }
    public decimal CategoryId { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string Comment { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal? CurrencyId { get; set; }
    public decimal EnteredBy { get; set; }
    public DateTime EnteredOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public decimal? ModifiedBy { get; set; }
    public List<ExpenseFileDto> Files { get; set; } = new();
}

/// <summary>
/// DTO for expense file response
/// </summary>
public class ExpenseFileDto
{
    public decimal Id { get; set; }
    public decimal ExpenseId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public DateTime UploadedOn { get; set; }
    public decimal UploadedBy { get; set; }
    public string BlobStoragePath { get; set; } = string.Empty;
}

/// <summary>
/// DTO for trip expense summary
/// </summary>
public class TripExpenseSummaryDto
{
    public decimal TripId { get; set; }
    public decimal ProjectId { get; set; }
    public DateTime TripStartDate { get; set; }
    public DateTime TripEndDate { get; set; }
    public decimal TotalExpenseAmount { get; set; }
    public int ExpenseCount { get; set; }
    public bool IsApproved { get; set; }
    public List<ExpenseDto> Expenses { get; set; } = new();
}

/// <summary>
/// DTO for paginated expenses
/// </summary>
public class PaginatedExpenseDto
{
    public List<ExpenseDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool HasNextPage => (PageNumber * PageSize) < TotalCount;
    public bool HasPreviousPage => PageNumber > 1;
}

/// <summary>
/// DTO for expense creation request
/// </summary>
public class CreateExpenseDto
{
    public decimal TripId { get; set; }
    public decimal CategoryId { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string Comment { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal? CurrencyId { get; set; }
}

/// <summary>
/// DTO for expense update request
/// </summary>
public class UpdateExpenseDto
{
    public string Comment { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal? CurrencyId { get; set; }
}
