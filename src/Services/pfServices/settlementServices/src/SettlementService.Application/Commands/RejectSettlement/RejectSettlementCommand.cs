using MediatR;
using SettlementService.Application.DTOs;

namespace SettlementService.Application.Commands.RejectSettlement;

public record RejectSettlementCommand : IRequest<SettlementDto>
{
    public long SettlementNumber { get; init; }
    public long RejectedBy { get; init; }
    public string? Remarks { get; init; }
}
