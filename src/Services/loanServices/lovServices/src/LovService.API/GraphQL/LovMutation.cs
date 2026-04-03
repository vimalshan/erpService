using MediatR;
using LovService.Application.Features.LovMaster.Commands;
using LovService.Application.Features.LovTypeMast.Commands;
using LovService.Application.DTOs;

namespace LovService.API.GraphQL;

public class LovMutation
{
    public async Task<LovTypeMastDto> CreateLovType([Service] IMediator mediator, CreateLovTypeCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<LovMasterDto> CreateLovMaster([Service] IMediator mediator, CreateLovMasterCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<LovMasterDto> UpdateLovMaster([Service] IMediator mediator, UpdateLovMasterCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> DeleteLovMaster([Service] IMediator mediator, long lovId, CancellationToken ct)
        => await mediator.Send(new DeleteLovMasterCommand(lovId), ct);
}
