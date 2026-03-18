using MediatR;
using SwipeTransactionService.Application.DTOs;
using SwipeTransactionService.Domain.Interfaces.Repositories;

namespace SwipeTransactionService.Application.Features.CanteenPunch.Queries;

public sealed class GetPunchByEmployeeDateQueryHandler
    : IRequestHandler<GetPunchByEmployeeDateQuery, CanteenPunchDto?>
{
    private readonly ICanteenPunchRepository _repository;

    public GetPunchByEmployeeDateQueryHandler(ICanteenPunchRepository repository)
        => _repository = repository;

    public async Task<CanteenPunchDto?> Handle(
        GetPunchByEmployeeDateQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByEmployeeAndDateAsync(
            request.EmployeeSysId, request.Date.Date, cancellationToken);

        if (entity is null) return null;

        return new CanteenPunchDto(
            entity.SerialNumber, entity.CompanyCode, entity.EmployeeSysId,
            entity.CanteenUnit, entity.PunchDate, entity.TimeIn, entity.TimeOut, entity.WorkHours);
    }
}

public sealed class GetPunchesByEmployeeQueryHandler
    : IRequestHandler<GetPunchesByEmployeeQuery, IEnumerable<CanteenPunchDto>>
{
    private readonly ICanteenPunchRepository _repository;

    public GetPunchesByEmployeeQueryHandler(ICanteenPunchRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<CanteenPunchDto>> Handle(
        GetPunchesByEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _repository.GetByEmployeeAsync(
            request.EmployeeSysId, request.From, request.To, cancellationToken);

        return entities.Select(e => new CanteenPunchDto(
            e.SerialNumber, e.CompanyCode, e.EmployeeSysId, e.CanteenUnit,
            e.PunchDate, e.TimeIn, e.TimeOut, e.WorkHours));
    }
}
