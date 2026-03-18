using MediatR;
using SettlementService.Application.DTOs;

namespace SettlementService.Application.Commands.ApproveSettlement;

public record ApproveSettlementCommand : IRequest<SettlementDto>
{
    public long SettlementNumber { get; init; }
    public long ApprovedBy { get; init; }
    public string? Remarks { get; init; }
}
