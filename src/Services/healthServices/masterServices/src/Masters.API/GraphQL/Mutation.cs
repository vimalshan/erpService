using MediatR;
using Masters.Application.Commands;
using Masters.Application.DTOs;

namespace Masters.API.GraphQL;

public class Mutation
{
    public async Task<LovTypeMasterDto> CreateLovTypeMaster(
        string lovTypeCode,
        string lovTypeName,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CreateLovTypeMasterCommand(lovTypeCode, lovTypeName);
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<LovTypeMasterDto> UpdateLovTypeMaster(
        string lovTypeCode,
        string lovTypeName,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new UpdateLovTypeMasterCommand(lovTypeCode, lovTypeName);
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<bool> DeleteLovTypeMaster(
        string lovTypeCode,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new DeleteLovTypeMasterCommand(lovTypeCode);
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<LovMasterDto> CreateLovMaster(
        long lovId,
        string lovType,
        string lovName,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CreateLovMasterCommand(lovId, lovType, lovName);
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<LovMasterDto> UpdateLovMaster(
        long lovId,
        string lovName,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new UpdateLovMasterCommand(lovId, lovName);
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<bool> DeleteLovMaster(
        long lovId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new DeleteLovMasterCommand(lovId);
        return await mediator.Send(command, cancellationToken);
    }
}
