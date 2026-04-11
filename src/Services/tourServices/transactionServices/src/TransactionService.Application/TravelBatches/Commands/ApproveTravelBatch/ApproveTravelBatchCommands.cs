using MediatR;
using TransactionService.Application.Common.Interfaces;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.TravelBatches.Commands.ApproveTravelBatch;

public sealed record AdminApproveTravelBatchCommand(
    string BatchId, string ApprovedBy, string? ApprovedAmount, string? Remarks = null) : IRequest;

public sealed class AdminApproveTravelBatchCommandHandler(
    ITravelBatchRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<AdminApproveTravelBatchCommand>
{
    public async Task Handle(AdminApproveTravelBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await repository.GetByIdAsync(request.BatchId, cancellationToken)
            ?? throw new TravelBatchNotFoundException(request.BatchId);

        batch.AdminApprove(request.ApprovedBy, request.ApprovedAmount, request.Remarks);

        repository.Update(batch);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

public sealed record FinanceApproveTravelBatchCommand(
    string BatchId, string ApprovedBy, string? Remarks = null) : IRequest;

public sealed class FinanceApproveTravelBatchCommandHandler(
    ITravelBatchRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<FinanceApproveTravelBatchCommand>
{
    public async Task Handle(FinanceApproveTravelBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await repository.GetByIdAsync(request.BatchId, cancellationToken)
            ?? throw new TravelBatchNotFoundException(request.BatchId);

        batch.FinanceApprove(request.ApprovedBy, request.Remarks);

        repository.Update(batch);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

public sealed record PostTravelBatchJVCommand(string BatchId, string JvId) : IRequest;

public sealed class PostTravelBatchJVCommandHandler(
    ITravelBatchRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<PostTravelBatchJVCommand>
{
    public async Task Handle(PostTravelBatchJVCommand request, CancellationToken cancellationToken)
    {
        var batch = await repository.GetByIdAsync(request.BatchId, cancellationToken)
            ?? throw new TravelBatchNotFoundException(request.BatchId);

        batch.PostJV(request.JvId);

        repository.Update(batch);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

public sealed record RejectTravelBatchCommand(
    string BatchId, string RejectedBy, string? Remarks = null) : IRequest;

public sealed class RejectTravelBatchCommandHandler(
    ITravelBatchRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<RejectTravelBatchCommand>
{
    public async Task Handle(RejectTravelBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await repository.GetByIdAsync(request.BatchId, cancellationToken)
            ?? throw new TravelBatchNotFoundException(request.BatchId);

        batch.Reject(request.RejectedBy, request.Remarks);

        repository.Update(batch);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
