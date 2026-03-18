using MediatR;

namespace SettlementService.Application.Commands.AddDeduction;

public record AddDeductionCommand : IRequest<Unit>
{
    public long SettlementNumber { get; init; }
    public string DeductionType { get; init; } = string.Empty;
    public decimal Amount { get; init; }
}
