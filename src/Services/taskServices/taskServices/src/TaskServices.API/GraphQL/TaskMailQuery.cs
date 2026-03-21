using MediatR;
using TaskServices.Application.DTOs;
using TaskServices.Application.Features.TaskMails.Queries;

namespace TaskServices.API.GraphQL;

public class TaskMailQuery
{
    public async Task<IReadOnlyList<TaskMailDto>> GetTaskMails([Service] IMediator mediator, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAllTaskMailsQuery(), cancellationToken);
    }

    public async Task<TaskMailDto?> GetTaskMailById([Service] IMediator mediator, decimal mid, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetTaskMailByIdQuery(mid), cancellationToken);
    }

    public async Task<IReadOnlyList<TaskMailDto>> GetTaskMailsByUser([Service] IMediator mediator, decimal sysId, CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetTaskMailsBySystemUserQuery(sysId), cancellationToken);
    }
}
