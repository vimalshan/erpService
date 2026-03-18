using AutoMapper;
using DealTicketing.Application.DTOs;
using DealTicketing.Domain.Interfaces;
using MediatR;

namespace DealTicketing.Application.Features.DealDetails.Queries;

public class GetDealDetailByIdQueryHandler(IDealDetailRepository repo, IMapper mapper)
    : IRequestHandler<GetDealDetailByIdQuery, DealDetailDto?>
{
    public async Task<DealDetailDto?> Handle(GetDealDetailByIdQuery req, CancellationToken ct)
    {
        var deal = await repo.GetByIdAsync(req.DealId, ct);
        return deal is null ? null : mapper.Map<DealDetailDto>(deal);
    }
}

public class GetDealDetailsByBatchQueryHandler(IDealDetailRepository repo, IMapper mapper)
    : IRequestHandler<GetDealDetailsByBatchQuery, IReadOnlyList<DealDetailDto>>
{
    public async Task<IReadOnlyList<DealDetailDto>> Handle(GetDealDetailsByBatchQuery req, CancellationToken ct)
    {
        var deals = await repo.GetByBatchIdAsync(req.BatchId, ct);
        return deals.Select(mapper.Map<DealDetailDto>).ToList();
    }
}

public class GetPendingApprovalsQueryHandler(IDealDetailRepository repo, IMapper mapper)
    : IRequestHandler<GetPendingApprovalsQuery, IReadOnlyList<DealDetailDto>>
{
    public async Task<IReadOnlyList<DealDetailDto>> Handle(GetPendingApprovalsQuery _, CancellationToken ct)
    {
        var deals = await repo.GetPendingApprovalsAsync(ct);
        return deals.Select(mapper.Map<DealDetailDto>).ToList();
    }
}

public class GetDealSettlementsByDealQueryHandler(IDealSettlementRepository repo, IMapper mapper)
    : IRequestHandler<GetDealSettlementsByDealQuery, IReadOnlyList<DealSettlementDto>>
{
    public async Task<IReadOnlyList<DealSettlementDto>> Handle(GetDealSettlementsByDealQuery req, CancellationToken ct)
    {
        var settlements = await repo.GetByDealIdAsync(req.DealId, ct);
        return settlements.Select(mapper.Map<DealSettlementDto>).ToList();
    }
}
