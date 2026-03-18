using BankService.Application.DTOs;
using MediatR;

namespace BankService.Application.Commands.Cheques;

public record IssueChequeCommand : IRequest<ChequeDetailDto>
{
    public long ChequeId { get; init; }
    public decimal ChequeNo { get; init; }
    public decimal Amount { get; init; }
    public DateTime ChequeDate { get; init; }
    public string Payee { get; init; } = null!;
    public long? AccountId { get; init; }
}
