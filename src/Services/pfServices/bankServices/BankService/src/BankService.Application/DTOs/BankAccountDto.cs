namespace BankService.Application.DTOs;

public record BankAccountDto
{
    public long AccountId { get; init; }
    public string AccountNumber { get; init; } = null!;
    public string AccountTitle { get; init; } = null!;
    public string BankCode { get; init; } = null!;
    public string TrustCode { get; init; } = null!;
    public string AccountType { get; init; } = null!;
    public decimal AccountBalance { get; init; }
    public string AccountStatus { get; init; } = null!;
    public DateTime OpeningDate { get; init; }
    public DateTime? ClosingDate { get; init; }
}
