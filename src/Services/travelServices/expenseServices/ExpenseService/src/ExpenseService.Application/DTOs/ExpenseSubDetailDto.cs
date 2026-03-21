namespace ExpenseService.Application.DTOs;

public record ExpenseSubDetailDto
{
    public long? RequestNumber { get; init; }
    public long? SerialNumber { get; init; }
    public long? ExpenseType { get; init; }
    public string? BillAttached { get; init; }
    public string? CityName { get; init; }
    public long? TotalAmount { get; init; }
    public string? StatusCode { get; init; }
    public string? Remarks { get; init; }
    public DateTime? BillDate { get; init; }
}
