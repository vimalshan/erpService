namespace ExpenseService.Application.DTOs;

public record DaSummaryDto
{
    public long RequestId { get; init; }
    public decimal AdminHours { get; init; }
    public decimal AdminDays { get; init; }
    public decimal AdminRate { get; init; }
    public decimal AdminAmount { get; init; }
    public decimal SelfHours { get; init; }
    public decimal SelfDays { get; init; }
    public decimal SelfRate { get; init; }
    public decimal SelfAmount { get; init; }
    public decimal TotalAmount => AdminAmount + SelfAmount;
}
