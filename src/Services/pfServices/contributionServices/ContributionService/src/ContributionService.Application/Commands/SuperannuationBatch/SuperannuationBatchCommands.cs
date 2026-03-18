using ContributionService.Application.DTOs;
using MediatR;

namespace ContributionService.Application.Commands.SuperannuationBatch;

public record CreateSuperannuationBatchCommand(
    long? TrustCode,
    string? Category,
    string? PayunitCode,
    string? PayMonthStart,
    DateTime? PayMonthEnd,
    string? ConAmt,
    DateTime? PayDate
) : IRequest<SuperannuationBatchDto>;

public record ApproveSuperannuationBatchCommand(long BatchNo) : IRequest<SuperannuationBatchDto>;
