using UtilityService.Application.DTOs;
using UtilityService.Application.Queries.GetAllToadPlanSql;
using UtilityService.Application.Queries.GetToadPlanSqlById;
using UtilityService.Application.Queries.GetToadPlanSqlByUser;
using MediatR;

namespace UtilityService.API.GraphQL;

public class ToadPlanSqlQueryType
{
    public async Task<PagedResultDto<ToadPlanSqlDto>> GetToadPlanSqls(
        IMediator mediator,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAllToadPlanSqlQuery(pageNumber, pageSize), cancellationToken);

    public async Task<ToadPlanSqlDto?> GetToadPlanSqlById(
        int id,
        IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetToadPlanSqlByIdQuery(id), cancellationToken);

    public async Task<IEnumerable<ToadPlanSqlDto>> GetToadPlanSqlByUser(
        string username,
        IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetToadPlanSqlByUserQuery(username), cancellationToken);
}
