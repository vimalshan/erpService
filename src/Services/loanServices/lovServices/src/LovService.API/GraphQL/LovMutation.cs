using MediatR;
using LovService.Application.Features.LovMaster.Commands;
using LovService.Application.Features.LovTypeMast.Commands;
using LovService.Application.DTOs;

namespace LovService.API.GraphQL;

public class LovMutation(IMediator mediator)
{
    public async Task<LovTypeMastDto> CreateLovType(CreateLovTypeCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<LovMasterDto> CreateLovMaster(CreateLovMasterCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<LovMasterDto> UpdateLovMaster(UpdateLovMasterCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> DeleteLovMaster(long lovId, CancellationToken ct)
        => await mediator.Send(new DeleteLovMasterCommand(lovId), ct);
}
