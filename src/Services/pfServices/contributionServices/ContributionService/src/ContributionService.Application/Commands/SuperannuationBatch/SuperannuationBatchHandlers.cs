using AutoMapper;
using ContributionService.Application.DTOs;
using ContributionService.Domain.Exceptions;
using ContributionService.Domain.Interfaces;
using MediatR;

namespace ContributionService.Application.Commands.SuperannuationBatch;

public class CreateSuperannuationBatchHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<CreateSuperannuationBatchCommand, SuperannuationBatchDto>
{
    public async Task<SuperannuationBatchDto> Handle(CreateSuperannuationBatchCommand request, CancellationToken ct)
    {
        var entity = Domain.Entities.SuperannuationBatch.Create(
            0, request.TrustCode, request.Category, request.PayunitCode,
            request.PayMonthStart, request.PayMonthEnd, request.ConAmt, request.PayDate);

        await uow.SuperannuationBatches.AddAsync(entity, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<SuperannuationBatchDto>(entity);
    }
}

public class ApproveSuperannuationBatchHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<ApproveSuperannuationBatchCommand, SuperannuationBatchDto>
{
    public async Task<SuperannuationBatchDto> Handle(ApproveSuperannuationBatchCommand request, CancellationToken ct)
    {
        var batch = await uow.SuperannuationBatches.GetByIdAsync(request.BatchNo, ct)
            ?? throw new ContributionNotFoundException("SuperannuationBatch", request.BatchNo);

        batch.Approve();
        await uow.SuperannuationBatches.UpdateAsync(batch, ct);
        await uow.SaveChangesAsync(ct);
        return mapper.Map<SuperannuationBatchDto>(batch);
    }
}
