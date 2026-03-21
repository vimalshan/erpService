namespace ExpenseService.Application.DTOs;

public record ExpenseAllocationDto
{
    public long? RequestNumber { get; init; }
    public long? AllocationSerialNumber { get; init; }
    public long? ExpenseSerialNumber { get; init; }
    public string? UnitCode { get; init; }
    public string? CostCentreCode { get; init; }
    public string? AllocationType { get; init; }
    public decimal? AllocationPercentage { get; init; }
}
