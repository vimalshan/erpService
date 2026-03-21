using LookupService.Application.DTOs;
using LookupService.Application.Queries;
using MediatR;

namespace LookupService.API.GraphQL;

public class LookupQuery
{
    public async Task<IEnumerable<LovTypeMasterDto>> GetLovTypes([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllLovTypesQuery(), ct);

    public async Task<LovTypeMasterDto?> GetLovTypeByCode(string typeCode, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetLovTypeByCodeQuery(typeCode), ct);

    public async Task<IEnumerable<LovMasterDto>> GetLovs([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllLovsQuery(), ct);

    public async Task<LovMasterDto?> GetLovById(long lovId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetLovByIdQuery(lovId), ct);

    public async Task<IEnumerable<LovMasterDto>> GetLovsByType(string lovType, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetLovsByTypeQuery(lovType), ct);

    public async Task<IEnumerable<ProcessMasterDto>> GetProcesses([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllProcessesQuery(), ct);

    public async Task<ProcessMasterDto?> GetProcessById(decimal processId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetProcessByIdQuery(processId), ct);

    public async Task<IEnumerable<PanelMasterDto>> GetPanels([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllPanelsQuery(), ct);

    public async Task<PanelMasterDto?> GetPanelById(decimal panelId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetPanelByIdQuery(panelId), ct);

    public async Task<IEnumerable<UnitProcessMapDto>> GetUnitProcesses(string unitCode, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetUnitProcessesByUnitCodeQuery(unitCode), ct);
}
