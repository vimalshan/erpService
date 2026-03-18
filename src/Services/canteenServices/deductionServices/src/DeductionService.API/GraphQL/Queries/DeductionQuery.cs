using DeductionService.Application.DTOs;
using DeductionService.Application.CQRS.Queries.GetDeductionById;
using DeductionService.Application.CQRS.Queries.GetDeductionsByEmployee;
using DeductionService.Application.CQRS.Queries.GetDeductionHistory;
using DeductionService.Application.CQRS.Queries.GetDeductionAmount;
using MediatR;

namespace DeductionService.API.GraphQL.Queries;

[QueryType]
public class DeductionQuery
{
    public async Task<AdhocPayDeductionDto> GetDeductionByIdAsync(
        long systemId,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetDeductionByIdQuery(systemId), ct);

    public async Task<IEnumerable<AdhocPayDeductionDto>> GetDeductionsByEmployeeAsync(
        long employeeNumber,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetDeductionsByEmployeeQuery(employeeNumber), ct);

    public async Task<IEnumerable<AdhocPayDeductionHistoryDto>> GetDeductionHistoryAsync(
        long employeeNumber,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetDeductionHistoryQuery(employeeNumber), ct);

    public async Task<DeductionAmountDto> GetDeductionAmountAsync(
        long empSysId,
        long itemCode,
        DateTime dateTaken,
        [Service] IMediator mediator,
        CancellationToken ct)
        => await mediator.Send(new GetDeductionAmountQuery(empSysId, itemCode, dateTaken), ct);
}
