using MediatR;
using ProjectService.Application.DTOs;
using ProjectService.Application.Queries;

namespace ProjectService.API.GraphQL;

public class ProjectQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IReadOnlyList<ProjectMainDto>> GetProjects([Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetAllProjectsQuery(), cancellationToken);

    public async Task<ProjectMainDto?> GetProjectById([Service] IMediator mediator, long id, CancellationToken cancellationToken)
        => await mediator.Send(new GetProjectByIdQuery(id), cancellationToken);

    public async Task<ProjectMainDto?> GetProjectWithDetails([Service] IMediator mediator, long id, CancellationToken cancellationToken)
        => await mediator.Send(new GetProjectWithDetailsQuery(id), cancellationToken);

    public async Task<IReadOnlyList<ProjectMainDto>> GetProjectsByStatus([Service] IMediator mediator, string status, CancellationToken cancellationToken)
        => await mediator.Send(new GetProjectsByStatusQuery(status[0]), cancellationToken);

    public async Task<IReadOnlyList<ProjectMemberDto>> GetProjectMembers([Service] IMediator mediator, long projectId, CancellationToken cancellationToken)
        => await mediator.Send(new GetProjectMembersQuery(projectId), cancellationToken);

    public async Task<IReadOnlyList<ProjectTypeMasterDto>> GetProjectTypes([Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetAllProjectTypesQuery(), cancellationToken);

    public async Task<IReadOnlyList<ProjectLocationDto>> GetLocations([Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetAllLocationsQuery(), cancellationToken);

    public async Task<IReadOnlyList<ProjectProcessDto>> GetProcesses([Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetAllProcessesQuery(), cancellationToken);

    public async Task<IReadOnlyList<ProjectFunctionDto>> GetFunctions([Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetAllFunctionsQuery(), cancellationToken);

    public async Task<IReadOnlyList<ProjectCategoryDto>> GetCategories([Service] IMediator mediator, CancellationToken cancellationToken)
        => await mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
}
