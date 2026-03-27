using AutoMapper;
using DealTicketing.Application.Common.Interfaces;
using DealTicketing.Application.DTOs;
using DealTicketing.Domain.Entities;
using DealTicketing.Domain.Exceptions;
using DealTicketing.Domain.Interfaces;
using MediatR;

namespace DealTicketing.Application.Features.DealBatches.Commands;

public class CreateDealBatchCommandHandler(
    IDealBatchRepository repository,
    IApplicationDbContext dbContext,
    IMapper mapper)
    : IRequestHandler<CreateDealBatchCommand, DealBatchDto>
{
    public async Task<DealBatchDto> Handle(CreateDealBatchCommand request, CancellationToken ct)
    {
        var batch = new DealBatch(
            request.DealBatchId,
            request.DealDate,
            request.DealDerType,
            request.DealBankId,
            request.DealBookedBy,
            request.DealBankTrader,
            request.DealBusinessId,
            request.DealModifiedBy,
            request.DealUnitId);

        await repository.AddAsync(batch, ct);
        await dbContext.SaveChangesAsync(ct);

        return mapper.Map<DealBatchDto>(batch);
    }
}

public class RejectDealBatchCommandHandler(
    IDealBatchRepository repository,
    IApplicationDbContext dbContext)
    : IRequestHandler<RejectDealBatchCommand, Unit>
{
    public async Task<Unit> Handle(RejectDealBatchCommand request, CancellationToken ct)
    {
        var batch = await repository.GetByIdAsync(request.DealBatchId, ct)
            ?? throw new DealBatchNotFoundException(request.DealBatchId);

        batch.Reject(request.RejectionReason, request.ModifiedBy);
        repository.Update(batch);
        await dbContext.SaveChangesAsync(ct);
        return Unit.Value;
    }
}

public class UpdateDealBatchScreenshotCommandHandler(
    IDealBatchRepository repository,
    IApplicationDbContext dbContext)
    : IRequestHandler<UpdateDealBatchScreenshotCommand, Unit>
{
    public async Task<Unit> Handle(UpdateDealBatchScreenshotCommand request, CancellationToken ct)
    {
        var batch = await repository.GetByIdAsync(request.DealBatchId, ct)
            ?? throw new DealBatchNotFoundException(request.DealBatchId);

        batch.SetScreenshot(request.Screenshot, request.ModifiedBy);
        repository.Update(batch);
        await dbContext.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
