using LovService.Application.Commands.LovType;
using LovService.Application.Commands.LovMaster;
using LovService.Application.Commands.ItemData;
using LovService.Application.DTOs;
using MediatR;

namespace LovService.API.GraphQL.Mutations;

[MutationType]
public class LovMutation
{
    // LovType mutations
    public async Task<long> CreateLovTypeAsync(long lovTypeId, string lovTypeName,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateLovTypeCommand(lovTypeId, lovTypeName), ct);

    public async Task<bool> UpdateLovTypeAsync(long lovTypeId, string lovTypeName,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new UpdateLovTypeCommand(lovTypeId, lovTypeName), ct);

    public async Task<bool> DeleteLovTypeAsync(long lovTypeId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteLovTypeCommand(lovTypeId), ct);

    // LovMaster mutations
    public async Task<long> CreateLovMasterAsync(long lovId, long lovTypeId, string lovName, long updatedBy,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateLovMasterCommand(lovId, lovTypeId, lovName, updatedBy), ct);

    public async Task<bool> UpdateLovMasterAsync(long lovId, string lovName, long updatedBy,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new UpdateLovMasterCommand(lovId, lovName, updatedBy), ct);

    public async Task<bool> DeleteLovMasterAsync(long lovId,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new DeleteLovMasterCommand(lovId), ct);

    // ItemData mutations
    public async Task<int> CreateItemDataAsync(CreateItemDataRequest input,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new CreateItemDataCommand(input.CatName, input.ItemName, input.Make, input.Uom, input.Price), ct);

    public async Task<bool> UpdateItemDataAsync(int id, UpdateItemDataRequest input,
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new UpdateItemDataCommand(id, input.CatName, input.ItemName, input.Make, input.Uom, input.Price), ct);
}
