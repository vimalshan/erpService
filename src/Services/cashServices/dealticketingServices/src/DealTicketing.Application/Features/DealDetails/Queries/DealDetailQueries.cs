using DealTicketing.Application.DTOs;
using MediatR;

namespace DealTicketing.Application.Features.DealDetails.Queries;

public record GetDealDetailByIdQuery(long DealId) : IRequest<DealDetailDto?>;

public record GetDealDetailsByBatchQuery(long BatchId) : IRequest<IReadOnlyList<DealDetailDto>>;

public record GetPendingApprovalsQuery : IRequest<IReadOnlyList<DealDetailDto>>;

public record GetDealSettlementsByDealQuery(long DealId) : IRequest<IReadOnlyList<DealSettlementDto>>;

public record GetDealSummaryQuery(DateTime FromDate, DateTime ToDate) : IRequest<IReadOnlyList<DealSummaryDto>>;
