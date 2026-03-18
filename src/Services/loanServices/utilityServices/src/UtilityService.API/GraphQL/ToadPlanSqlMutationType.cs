using MediatR;
using UtilityService.Application.Commands.CreateToadPlanSql;
using UtilityService.Application.Commands.DeleteToadPlanSql;
using UtilityService.Application.Commands.UpdateToadPlanSql;
using UtilityService.Application.DTOs;

namespace UtilityService.API.GraphQL;

public class ToadPlanSqlMutationType
{
    public async Task<ToadPlanSqlDto> CreateToadPlanSql(
        CreateToadPlanSqlInput input,
        IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(
            new CreateToadPlanSqlCommand(input.Username, input.StatementId, input.Statement, input.Timestamp),
            cancellationToken);

    public async Task<bool> UpdateToadPlanSql(
        int id,
        UpdateToadPlanSqlInput input,
        IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(
            new UpdateToadPlanSqlCommand(id, input.Username, input.Statement, input.Timestamp),
            cancellationToken);

    public async Task<bool> DeleteToadPlanSql(
        int id,
        IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new DeleteToadPlanSqlCommand(id), cancellationToken);
}

public record CreateToadPlanSqlInput(
    string? Username,
    string StatementId,
    string? Statement,
    DateTime? Timestamp);

public record UpdateToadPlanSqlInput(
    string? Username,
    string? Statement,
    DateTime? Timestamp);
