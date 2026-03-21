using AdminService.Application.DTOs;
using AdminService.Application.Queries;
using MediatR;

namespace AdminService.API.GraphQL;

/// <summary>
/// GraphQL query type
/// </summary>
public class Query
{
    public async Task<IEnumerable<AdminUnitDto>> GetAdminUnits(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllAdminUnitsQuery(), cancellationToken);
    }

    public async Task<AdminUnitDto?> GetAdminUnitById(
        long id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAdminUnitByIdQuery(id), cancellationToken);
    }

    public async Task<IEnumerable<AdminUnitDto>> GetAdminUnitsByType(
        string adminType,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAdminUnitsByTypeQuery(adminType), cancellationToken);
    }

    public async Task<IEnumerable<FinanceUnitDto>> GetFinanceUnits(
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllFinanceUnitsQuery(), cancellationToken);
    }

    public async Task<FinanceUnitDto?> GetFinanceUnitById(
        long id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetFinanceUnitByIdQuery(id), cancellationToken);
    }
}
