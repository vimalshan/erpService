using BankService.Application.DTOs;
using MediatR;

namespace BankService.Application.Commands.ChequeRegisters;

public record CreateChequeRegisterCommand : IRequest<ChequeRegisterDto>
{
    public decimal ChequeNoFrom { get; init; }
    public decimal ChequeNoTo { get; init; }
    public string ChequeBookId { get; init; } = null!;
    public long AccountId { get; init; }
    public DateTime IssuedDate { get; init; }
}
