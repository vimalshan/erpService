using MediatR;
using CompetencyService.Application.Commands.Competencies;
using CompetencyService.Application.Commands.EmpCompetencies;
using CompetencyService.Application.Commands.RoleSpecific;
using CompetencyService.Application.DTOs;

namespace CompetencyService.API.GraphQL.Mutations;

public class CompetencyMutation
{
    public async Task<CompetencyDto> CreateCompetency(
        [Service] IMediator mediator,
        CreateCompetencyCommand input,
        CancellationToken ct = default)
        => await mediator.Send(input, ct);

    public async Task<CompetencyDto> UpdateCompetency(
        [Service] IMediator mediator,
        UpdateCompetencyCommand input,
        CancellationToken ct = default)
        => await mediator.Send(input, ct);

    public async Task<bool> CloseCompetency(
        [Service] IMediator mediator,
        CloseCompetencyCommand input,
        CancellationToken ct = default)
        => await mediator.Send(input, ct);

    public async Task<bool> DeleteCompetency(
        [Service] IMediator mediator,
        decimal id,
        CancellationToken ct = default)
        => await mediator.Send(new DeleteCompetencyCommand(id), ct);

    public async Task<EmpSpecificCompetencyDto> AssignEmpCompetency(
        [Service] IMediator mediator,
        AssignEmpCompetencyCommand input,
        CancellationToken ct = default)
        => await mediator.Send(input, ct);

    public async Task<RoleSpecificDto> AssignRoleCompetency(
        [Service] IMediator mediator,
        AssignRoleCompetencyCommand input,
        CancellationToken ct = default)
        => await mediator.Send(input, ct);
}
