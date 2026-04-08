using MediatR;
using PFTransactionalService.Application.DTOs;

namespace PFTransactionalService.Application.Commands.ProcessWithdrawal;

public record ProcessWithdrawalCommand : IRequest<PFAccumulationDto>
{
    public long EmpSysId { get; init; }
    public decimal Amount { get; init; }
    public string SettlementType { get; init; } = string.Empty;
    public long ApprovedBy { get; init; }
}
