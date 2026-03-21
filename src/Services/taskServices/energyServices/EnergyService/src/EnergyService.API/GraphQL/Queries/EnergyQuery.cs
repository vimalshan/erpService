using EnergyService.Application.DTOs;
using EnergyService.Application.Features.Processes.Queries.GetAllProcesses;
using EnergyService.Application.Features.Processes.Queries.GetProcessById;
using EnergyService.Application.Features.ProcessAccess.Queries.GetProcessAccessByProcess;
using EnergyService.Application.Features.ProcessMail.Queries.GetMailIdsByProcess;
using EnergyService.Application.Features.Readings.Queries.GetReadingById;
using EnergyService.Application.Features.Readings.Queries.GetReadingsByProcess;
using MediatR;

namespace EnergyService.API.GraphQL.Queries;

public class EnergyQuery
{
    public async Task<IReadOnlyList<EcProcessDto>> GetProcesses([Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllProcessesQuery(), ct);

    public async Task<EcProcessDto?> GetProcessById([Service] IMediator mediator, int id, CancellationToken ct)
        => await mediator.Send(new GetProcessByIdQuery(id), ct);

    public async Task<IReadOnlyList<EcReadingDto>> GetReadingsByProcess([Service] IMediator mediator, int processId, CancellationToken ct)
        => await mediator.Send(new GetReadingsByProcessQuery(processId), ct);

    public async Task<EcReadingDto?> GetReadingById([Service] IMediator mediator, int id, CancellationToken ct)
        => await mediator.Send(new GetReadingByIdQuery(id), ct);

    public async Task<IReadOnlyList<EcProcessAccessDto>> GetProcessAccess([Service] IMediator mediator, int processId, CancellationToken ct)
        => await mediator.Send(new GetProcessAccessByProcessQuery(processId), ct);

    public async Task<IReadOnlyList<EcProcessMailIdDto>> GetProcessMailIds([Service] IMediator mediator, int processId, CancellationToken ct)
        => await mediator.Send(new GetMailIdsByProcessQuery(processId), ct);
}
