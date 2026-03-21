using LookupService.Application.Commands;
using MediatR;

namespace LookupService.API.GraphQL;

public class LookupMutation
{
    public async Task<long> CreateLov(string lovType, string lovName, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateLovCommand(lovType, lovName), ct);

    public async Task<bool> UpdateLov(long lovId, string lovName, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new UpdateLovCommand(lovId, lovName), ct);

    public async Task<bool> DeleteLov(long lovId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteLovCommand(lovId), ct);

    public async Task<string> CreateLovType(string lovTypeCode, string? lovTypeName, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateLovTypeCommand(lovTypeCode, lovTypeName), ct);

    public async Task<decimal> CreateProcess(decimal processId, string processName, string liveFlag, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateProcessCommand(processId, processName, liveFlag), ct);

    public async Task<bool> UpdateProcess(decimal processId, string processName, string liveFlag, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new UpdateProcessCommand(processId, processName, liveFlag), ct);

    public async Task<decimal> CreatePanel(decimal panelId, string panelName, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreatePanelCommand(panelId, panelName), ct);

    public async Task<decimal> MapLovToUnit(long lovId, string unitCode, string flag, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new MapLovToUnitCommand(lovId, unitCode, flag), ct);

    public async Task<decimal> MapUnitProcess(string unitCode, decimal processId, [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new MapUnitProcessCommand(unitCode, processId), ct);
}
