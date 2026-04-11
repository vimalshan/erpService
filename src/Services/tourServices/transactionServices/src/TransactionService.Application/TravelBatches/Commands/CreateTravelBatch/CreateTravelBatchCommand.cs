using FluentValidation;
using MediatR;
using TransactionService.Application.Common.Interfaces;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Aggregates;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.TravelBatches.Commands.CreateTravelBatch;

public sealed record CreateTravelBatchCommand : IRequest<TravelBatchDto>
{
    public string BatchId { get; init; } = default!;
    public string AdminId { get; init; } = default!;
    public string PayUnitId { get; init; } = default!;
    public string VendorId { get; init; } = default!;
    public string? InvNum { get; init; }
    public string? InvAmount { get; init; }
    public string? BatchType { get; init; }
    public string CreatedBy { get; init; } = default!;
    public List<CreateBatchSubItem> SubItems { get; init; } = [];
}

public sealed record CreateBatchSubItem
{
    public string BatchSubId { get; init; } = default!;
    public string CreditType { get; init; } = default!;
    public string? BookCnfId { get; init; }
    public string? BookNo { get; init; }
    public string? BasAmt { get; init; }
    public string? TotAmt { get; init; }
    public string? AppAmt { get; init; }
}

public sealed class CreateTravelBatchCommandValidator : AbstractValidator<CreateTravelBatchCommand>
{
    public CreateTravelBatchCommandValidator()
    {
        RuleFor(x => x.BatchId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdminId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.PayUnitId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.VendorId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(255);
        RuleForEach(x => x.SubItems).ChildRules(sub =>
        {
            sub.RuleFor(s => s.BatchSubId).NotEmpty().MaximumLength(255);
            sub.RuleFor(s => s.CreditType).NotEmpty().MaximumLength(1);
        });
    }
}

public sealed class CreateTravelBatchCommandHandler(
    ITravelBatchRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateTravelBatchCommand, TravelBatchDto>
{
    public async Task<TravelBatchDto> Handle(
        CreateTravelBatchCommand request,
        CancellationToken cancellationToken)
    {
        if (await repository.ExistsAsync(request.BatchId, cancellationToken))
            throw new InvalidOperationException($"Travel Batch '{request.BatchId}' already exists.");

        var batch = TravelBatch.Create(
            request.BatchId, request.AdminId, request.PayUnitId, request.VendorId,
            request.InvNum, request.InvAmount, request.BatchType, request.CreatedBy);

        foreach (var sub in request.SubItems)
        {
            batch.AddSubItem(TravelBatchSub.Create(
                sub.BatchSubId, request.BatchId, sub.CreditType,
                sub.BookCnfId, sub.BookNo, sub.BasAmt, sub.TotAmt, sub.AppAmt));
        }

        await repository.AddAsync(batch, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(batch);
    }

    private static TravelBatchDto MapToDto(TravelBatch batch) => new()
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
