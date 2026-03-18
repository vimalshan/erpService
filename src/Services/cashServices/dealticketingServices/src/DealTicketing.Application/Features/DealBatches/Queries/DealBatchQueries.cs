using DealTicketing.Application.DTOs;
using MediatR;

namespace DealTicketing.Application.Features.DealBatches.Queries;

public record GetDealBatchByIdQuery(long DealBatchId) : IRequest<DealBatchDto?>;

public record GetDealBatchesByDateQuery(DateTime Date) : IRequest<IReadOnlyList<DealBatchDto>>;

public record GetAllDealBatchesQuery(int Page = 1, int PageSize = 20) : IRequest<IReadOnlyList<DealBatchDto>>;
