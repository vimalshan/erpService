using MediatR;
using SwipeTransactionService.Application.DTOs;
using SwipeTransactionService.Domain.Entities;
using SwipeTransactionService.Domain.Interfaces.Repositories;

namespace SwipeTransactionService.Application.Features.SwipeTransactions.Commands;

public sealed class RecordSwipeUploadCommandHandler
    : IRequestHandler<RecordSwipeUploadCommand, SwipeCardUploadDto>
{
    private readonly ISwipeCardUploadRepository _repository;

    public RecordSwipeUploadCommandHandler(ISwipeCardUploadRepository repository)
        => _repository = repository;

    public async Task<SwipeCardUploadDto> Handle(
        RecordSwipeUploadCommand request,
        CancellationToken cancellationToken)
    {
        var entity = SwipeCardUpload.Create(
            request.CompanyCode,
            request.EmployeeNumber,
            request.SwipeTime,
            request.ItemCode,
            request.ItemQuantity,
            request.BatchNumber,
            request.SerialNumber,
            request.CanteenNumber,
            request.GateNumber);

        await _repository.AddAsync(entity, cancellationToken);

        return new SwipeCardUploadDto(
            entity.CompanyCode,
            entity.EmployeeNumber,
            entity.SwipeTime,
            entity.ItemCode,
            entity.ItemQuantity,
            entity.BatchNumber,
            entity.SerialNumber,
            entity.BatchDate,
            entity.EntryDate,
            entity.CanteenNumber,
            entity.GateNumber,
            entity.UpdateStatus,
            entity.FlexField1,
            entity.FlexField2);
    }
}
