using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.TravelBatches.Queries;

public sealed record GetTravelBatchByIdQuery(string BatchId) : IRequest<TravelBatchDto>;

public sealed class GetTravelBatchByIdQueryHandler(
    ITravelBatchRepository repository) : IRequestHandler<GetTravelBatchByIdQuery, TravelBatchDto>
{
    public async Task<TravelBatchDto> Handle(GetTravelBatchByIdQuery request, CancellationToken cancellationToken)
    {
        var batch = await repository.GetByIdAsync(request.BatchId, cancellationToken)
            ?? throw new TravelBatchNotFoundException(request.BatchId);

        return MapToDto(batch);
    }

    private static TravelBatchDto MapToDto(Domain.Aggregates.TravelBatch batch) => new()
    {
        BatchId = batch.BatchId,
        AdminId = batch.AdminId,
        PayUnitId = batch.PayUnitId,
        BatchDate = batch.BatchDate,
        InvNum = batch.InvNum,
        InvAmount = batch.InvAmount,
        Status = batch.Status,
        VendorId = batch.VendorId,
        ApprovedAmount = batch.ApprovedAmount,
        TotalPayable = batch.TotalPayable,
        JvId = batch.JvId,
        BatchType = batch.BatchType,
        CreatedBy = batch.CreatedBy,
        CreatedOn = batch.CreatedOn,
        SubItems = batch.SubItems.Select(s => new TravelBatchSubDto
        {
            BatchSubId = s.BatchSubId,
            BatchId = s.BatchId,
            BookCnfId = s.BookCnfId,
            BasAmt = s.BasAmt,
            TotAmt = s.TotAmt,
            AppAmt = s.AppAmt,
            CreditType = s.CreditType,
            TpId = s.TpId
        })
    };
}

public sealed record GetAllTravelBatchesQuery(
    int Page = 1, int PageSize = 20, string? Status = null, string? VendorId = null) : IRequest<IEnumerable<TravelBatchDto>>;

public sealed class GetAllTravelBatchesQueryHandler(
    ITravelBatchRepository repository) : IRequestHandler<GetAllTravelBatchesQuery, IEnumerable<TravelBatchDto>>
{
    public async Task<IEnumerable<TravelBatchDto>> Handle(GetAllTravelBatchesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Aggregates.TravelBatch> batches;

        if (!string.IsNullOrEmpty(request.Status))
            batches = await repository.GetByStatusAsync(request.Status, cancellationToken);
        else if (!string.IsNullOrEmpty(request.VendorId))
            batches = await repository.GetByVendorIdAsync(request.VendorId, cancellationToken);
        else
            batches = await repository.GetAllAsync(request.Page, request.PageSize, cancellationToken);

        return batches.Select(b => new TravelBatchDto
        {
            BatchId = b.BatchId,
            AdminId = b.AdminId,
            PayUnitId = b.PayUnitId,
            BatchDate = b.BatchDate,
            InvNum = b.InvNum,
            InvAmount = b.InvAmount,
            Status = b.Status,
            VendorId = b.VendorId,
            ApprovedAmount = b.ApprovedAmount,
            TotalPayable = b.TotalPayable,
            JvId = b.JvId,
            BatchType = b.BatchType,
            CreatedBy = b.CreatedBy,
            CreatedOn = b.CreatedOn
        });
    }
}
