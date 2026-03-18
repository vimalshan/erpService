using BankService.Application.DTOs;
using MediatR;

namespace BankService.Application.Commands.BankAccounts;

public record CreateBankAccountCommand : IRequest<BankAccountDto>
{
    public string AccountNumber { get; init; } = null!;
    public string AccountTitle { get; init; } = null!;
    public string BankCode { get; init; } = null!;
    public string TrustCode { get; init; } = null!;
    public string AccountType { get; init; } = null!;
    public DateTime OpeningDate { get; init; }
}
