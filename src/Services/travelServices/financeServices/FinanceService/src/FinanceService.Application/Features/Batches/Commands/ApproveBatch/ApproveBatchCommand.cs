using MediatR;

namespace FinanceService.Application.Features.Batches.Commands.ApproveBatch;

public record ApproveBatchCommand : IRequest<bool>
{
    public string UnitCode { get; init; } = string.Empty;
    public decimal BatchNumber { get; init; }
    public long ApprovedBy { get; init; }
    public string? ApprovalRemarks { get; init; }
}
