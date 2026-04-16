using ActionService.Application.Commands;
using ActionService.Application.DTOs;
using MediatR;

namespace ActionService.GraphQL.Mutations;

public class Mutation
{
    public async Task<ActionDto> CreateAction([Service] IMediator mediator, CreateActionDto input)
        => await mediator.Send(new CreateActionCommand(input));

    public async Task<ActionDto> UpdateAction([Service] IMediator mediator, UpdateActionDto input)
        => await mediator.Send(new UpdateActionCommand(input));

    public async Task<bool> DeleteAction([Service] IMediator mediator, int id)
        => await mediator.Send(new DeleteActionCommand(id));

    public async Task<bool> CompleteAction([Service] IMediator mediator, int id)
        => await mediator.Send(new CompleteActionCommand(id));
}
