using EnergyService.Application.DTOs;
using EnergyService.Application.Features.Processes.Commands.CreateProcess;
using EnergyService.Application.Features.Processes.Commands.UpdateProcess;
using EnergyService.Application.Features.Processes.Commands.DeleteProcess;
using EnergyService.Application.Features.Readings.Commands.InsertReading;
using EnergyService.Application.Features.ProcessAccess.Commands.UpdateProcessAccess;
using EnergyService.Application.Features.ProcessMail.Commands.ConfigureMailId;
using MediatR;

namespace EnergyService.API.GraphQL.Mutations;

public class EnergyMutation
{
    public async Task<EcProcessDto> CreateProcess([Service] IMediator mediator, CreateProcessCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<EcProcessDto> UpdateProcess([Service] IMediator mediator, UpdateProcessCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> DeleteProcess([Service] IMediator mediator, int id, CancellationToken ct)
        => await mediator.Send(new DeleteProcessCommand(id), ct);

    public async Task<EcReadingDto> InsertReading([Service] IMediator mediator, InsertReadingCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<EcProcessAccessDto> UpdateProcessAccess([Service] IMediator mediator, UpdateProcessAccessCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<EcProcessMailIdDto> ConfigureMailId([Service] IMediator mediator, ConfigureMailIdCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);
}
