namespace BankService.Application.DTOs;

public record ChequeRegisterDto
{
    public long RegisterId { get; init; }
    public decimal ChequeNoFrom { get; init; }
    public decimal ChequeNoTo { get; init; }
    public string ChequeBookId { get; init; } = null!;
    public long AccountId { get; init; }
    public DateTime IssuedDate { get; init; }
    public string RegisterStatus { get; init; } = null!;
}
