using AutoMapper;
using DealTicketing.Application.Common.Interfaces;
using DealTicketing.Application.DTOs;
using DealTicketing.Domain.Exceptions;
using DealTicketing.Domain.Interfaces;
using MediatR;

namespace DealTicketing.Application.Features.DealDetails.Commands;

public class CreateDealDetailCommandHandler(
    IDealBatchRepository batchRepo,
    IApplicationDbContext dbContext,
    IMapper mapper)
    : IRequestHandler<CreateDealDetailCommand, DealDetailDto>
{
    public async Task<DealDetailDto> Handle(CreateDealDetailCommand req, CancellationToken ct)
    {
        var batch = await batchRepo.GetByIdAsync(req.DealBatchId, ct)
            ?? throw new DealBatchNotFoundException(req.DealBatchId);

        var detail = batch.AddDealDetail(
            req.DealId, req.DealNo, req.DealVersionId,
            req.DealTranType, req.DealAmount,
            req.DealCurrency1, req.DealCurrency2,
            req.DealSpotRate, req.DealBookRate,
            req.DealMatDate, req.ModifiedBy);

        batchRepo.Update(batch);
        await dbContext.SaveChangesAsync(ct);

        return mapper.Map<DealDetailDto>(detail);
    }
}

public class ApproveDealCommandHandler(
    IDealDetailRepository dealRepo,
    IApplicationDbContext dbContext)
    : IRequestHandler<ApproveDealCommand, Unit>
{
    public async Task<Unit> Handle(ApproveDealCommand req, CancellationToken ct)
    {
        var deal = await dealRepo.GetByIdAsync(req.DealId, ct)
            ?? throw new DealNotFoundException(req.DealId);

        deal.Approve(req.AppBusiness, req.Remarks, req.ModifiedBy);
        dealRepo.Update(deal);
        await dbContext.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class RejectDealCommandHandler(
    IDealDetailRepository dealRepo,
    IApplicationDbContext dbContext)
    : IRequestHandler<RejectDealCommand, Unit>
{
    public async Task<Unit> Handle(RejectDealCommand req, CancellationToken ct)
    {
        var deal = await dealRepo.GetByIdAsync(req.DealId, ct)
            ?? throw new DealNotFoundException(req.DealId);

        deal.Reject(req.Remarks, req.ModifiedBy);
        dealRepo.Update(deal);
        await dbContext.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class CreateDealSettlementCommandHandler(
    IDealDetailRepository dealRepo,
    IApplicationDbContext dbContext,
    IMapper mapper)
    : IRequestHandler<CreateDealSettlementCommand, DealSettlementDto>
{
    public async Task<DealSettlementDto> Handle(CreateDealSettlementCommand req, CancellationToken ct)
    {
        var deal = await dealRepo.GetByIdAsync(req.DealId, ct)
            ?? throw new DealNotFoundException(req.DealId);

        var settlement = deal.AddSettlement(
            req.SetId, req.GainLossAmt, req.SetType,
            req.SpotRate, req.ExchangeRate, req.ModifiedBy);

        dealRepo.Update(deal);
        await dbContext.SaveChangesAsync(ct);
        return mapper.Map<DealSettlementDto>(settlement);
    }
}
