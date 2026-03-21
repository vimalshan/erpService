using MediatR;
using TaskServices.Application.Features.TaskMails.Commands;

namespace TaskServices.API.GraphQL;

public class TaskMailMutation
{
    public async Task<decimal> CreateTaskMail([Service] IMediator mediator, decimal mid, decimal sysId, CancellationToken cancellationToken)
    {
        return await mediator.Send(new CreateTaskMailCommand(mid, sysId), cancellationToken);
    }

    public async Task<bool> UpdateTaskMail([Service] IMediator mediator, decimal mid, decimal sysId, CancellationToken cancellationToken)
    {
        await mediator.Send(new UpdateTaskMailCommand(mid, sysId), cancellationToken);
        return true;
    }

    public async Task<bool> DeleteTaskMail([Service] IMediator mediator, decimal mid, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteTaskMailCommand(mid), cancellationToken);
        return true;
    }
}
