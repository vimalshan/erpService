using HotChocolate;
using MediatR;
using ObjectiveService.Application.Common;
using ObjectiveService.Application.DTOs;
using ObjectiveService.Application.Features.Goals.Queries;
using ObjectiveService.Application.Features.ControlPoints.Queries;

namespace ObjectiveService.API.GraphQL;

public class Query
{
    // ── Goals ────────────────────────────────────────────────────────────────

    public async Task<CommandResult<GoalDto>> GetGoalById(
        decimal id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(new GetGoalByIdQuery(id), cancellationToken);

    public async Task<CommandResult<List<GoalDto>>> GetGoalsByEmployee(
        string userId,
        decimal pinNumber,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(new GetGoalsByEmployeeQuery(userId, pinNumber), cancellationToken);

    public async Task<CommandResult<List<GoalDto>>> GetGoalsByPeriod(
        DateTime periodFrom,
        DateTime periodTo,
        int pageNumber,
        int pageSize,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(
            new GetGoalsByPeriodQuery { PeriodFrom = periodFrom, PeriodTo = periodTo, PageNumber = pageNumber, PageSize = pageSize },
            cancellationToken);

    // ── Control Points ────────────────────────────────────────────────────────

    public async Task<CommandResult<ControlPointDto>> GetControlPointById(
        decimal id,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(new GetControlPointByIdQuery(id), cancellationToken);

    public async Task<CommandResult<List<ControlPointDto>>> GetControlPointsByEmployee(
        decimal employeeSysId,
        decimal ddYearId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(new GetControlPointsByEmployeeQuery(employeeSysId, ddYearId), cancellationToken);

    public async Task<CommandResult<List<ControlPointDto>>> GetAllControlPoints(
        int pageNumber,
        int pageSize,
        [Service] IMediator mediator,
        CancellationToken cancellationToken) =>
        await mediator.Send(
            new GetAllControlPointsQuery { PageNumber = pageNumber, PageSize = pageSize },
            cancellationToken);
}
