using MediatR;
using SwipeTransactionService.Application.DTOs;
using SwipeTransactionService.Domain.Interfaces.Repositories;

namespace SwipeTransactionService.Application.Features.SwipeTransactions.Queries;

public sealed class GetSwipesByEmployeeQueryHandler
    : IRequestHandler<GetSwipesByEmployeeQuery, IEnumerable<SwipeCardUploadDto>>
{
    private readonly ISwipeCardUploadRepository _repository;

    public GetSwipesByEmployeeQueryHandler(ISwipeCardUploadRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<SwipeCardUploadDto>> Handle(
        GetSwipesByEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _repository.GetByEmployeeAsync(
            request.EmployeeNumber, request.From, request.To, cancellationToken);

        return entities.Select(e => new SwipeCardUploadDto(
            e.CompanyCode, e.EmployeeNumber, e.SwipeTime, e.ItemCode, e.ItemQuantity,
            e.BatchNumber, e.SerialNumber, e.BatchDate, e.EntryDate,
            e.CanteenNumber, e.GateNumber, e.UpdateStatus, e.FlexField1, e.FlexField2));
    }
}

public sealed class GetPendingSwipesQueryHandler
    : IRequestHandler<GetPendingSwipesQuery, IEnumerable<SwipeCardUploadDto>>
{
    private readonly ISwipeCardUploadRepository _repository;

    public GetPendingSwipesQueryHandler(ISwipeCardUploadRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<SwipeCardUploadDto>> Handle(
        GetPendingSwipesQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _repository.GetPendingAsync(cancellationToken);
        return entities.Select(e => new SwipeCardUploadDto(
            e.CompanyCode, e.EmployeeNumber, e.SwipeTime, e.ItemCode, e.ItemQuantity,
            e.BatchNumber, e.SerialNumber, e.BatchDate, e.EntryDate,
            e.CanteenNumber, e.GateNumber, e.UpdateStatus, e.FlexField1, e.FlexField2));
    }
}
