namespace ExpenseService.Application.DTOs;

public record ConveyanceDto
{
    public long SerialNumber { get; init; }
    public long RequestNumber { get; init; }
    public DateTime? Date { get; init; }
    public string? Particulars { get; init; }
    public long? Mode { get; init; }
    public long? Amount { get; init; }
    public long? BookRequestNumber { get; init; }
    public string? BookStatus { get; init; }
}
