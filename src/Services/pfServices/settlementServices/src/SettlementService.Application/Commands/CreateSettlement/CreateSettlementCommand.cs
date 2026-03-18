using MediatR;
using SettlementService.Application.DTOs;

namespace SettlementService.Application.Commands.CreateSettlement;

public record CreateSettlementCommand : IRequest<SettlementDto>
{
    public long SettlementNumber { get; init; }
    public long MemberNo { get; init; }
    public string SettlementType { get; init; } = string.Empty;
    public decimal SettlementAmount { get; init; }
    public DateTime SettlementDate { get; init; }
    public long CreatedBy { get; init; }
    public string? TrustCode { get; init; }
    public string? Reason { get; init; }
}
