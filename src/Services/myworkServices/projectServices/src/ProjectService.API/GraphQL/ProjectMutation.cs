using MediatR;
using ProjectService.Application.Commands;
using ProjectService.Application.DTOs;

namespace ProjectService.API.GraphQL;

public class ProjectMutation
{
    public async Task<ProjectMainDto> CreateProject([Service] IMediator mediator, CreateProjectCommand input, CancellationToken cancellationToken)
        => await mediator.Send(input, cancellationToken);

    public async Task<ProjectMainDto> UpdateProject([Service] IMediator mediator, UpdateProjectCommand input, CancellationToken cancellationToken)
        => await mediator.Send(input, cancellationToken);

    public async Task<bool> DeleteProject([Service] IMediator mediator, long projId, CancellationToken cancellationToken)
        => await mediator.Send(new DeleteProjectCommand(projId), cancellationToken);

    public async Task<ProjectMainDto> ChangeProjectStatus([Service] IMediator mediator, long projId, string newStatus, CancellationToken cancellationToken)
        => await mediator.Send(new ChangeProjectStatusCommand(projId, newStatus[0]), cancellationToken);

    public async Task<ProjectMainDto> CloseProject([Service] IMediator mediator, long projId, CancellationToken cancellationToken)
        => await mediator.Send(new CloseProjectCommand(projId), cancellationToken);

    public async Task<ProjectMemberDto> AddProjectMember([Service] IMediator mediator, long projId, long funcId, long empSysId, CancellationToken cancellationToken)
        => await mediator.Send(new AddProjectMemberCommand(projId, funcId, empSysId), cancellationToken);

    public async Task<bool> RemoveProjectMember([Service] IMediator mediator, long memberId, CancellationToken cancellationToken)
        => await mediator.Send(new RemoveProjectMemberCommand(memberId), cancellationToken);

    public async Task<ProjectHoldDto> HoldProject([Service] IMediator mediator, long projId, string reason, long updatedBy, CancellationToken cancellationToken)
        => await mediator.Send(new HoldProjectCommand(projId, reason, updatedBy), cancellationToken);
}
