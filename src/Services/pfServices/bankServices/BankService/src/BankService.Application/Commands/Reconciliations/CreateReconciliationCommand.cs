using BankService.Application.DTOs;
using MediatR;

namespace BankService.Application.Commands.Reconciliations;

public record CreateReconciliationCommand : IRequest<PaymentReconciliationDto>
{
    public long ChequeId { get; init; }
    public string ReconReference { get; init; } = null!;
    public decimal ReconAmount { get; init; }
    public DateTime ReconDate { get; init; }
}
