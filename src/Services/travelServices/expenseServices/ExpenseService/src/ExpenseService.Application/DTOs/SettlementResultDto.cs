namespace ExpenseService.Application.DTOs;

public record SettlementResultDto
{
    public decimal SettlementAmount { get; init; }
    public decimal RefundAmount { get; init; }
    public string Status { get; init; } = "Settled";
}
