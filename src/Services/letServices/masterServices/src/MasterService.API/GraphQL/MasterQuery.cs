using MasterService.Application.DTOs;
using MasterService.Application.Features.Categories.Queries;
using MasterService.Application.Features.Jobs.Queries;
using MasterService.Application.Features.Skills.Queries;
using MasterService.Application.Features.Trainings.Queries;
using MediatR;

namespace MasterService.API.GraphQL;

public class MasterQuery
{
    public async Task<IEnumerable<SkillDto>> GetSkillsAsync(
        [Service] IMediator mediator, char? skillType = null, CancellationToken ct = default)
        => await mediator.Send(new GetSkillsQuery(skillType), ct);

    public async Task<SkillDto?> GetSkillAsync(
        [Service] IMediator mediator, long skillCode, CancellationToken ct = default)
        => await mediator.Send(new GetSkillByCodeQuery(skillCode), ct);

    public async Task<IEnumerable<TrainingProviderDto>> GetTrainingsAsync(
        [Service] IMediator mediator, CancellationToken ct = default)
        => await mediator.Send(new GetTrainingsQuery(), ct);

    public async Task<IEnumerable<JobMasterDto>> GetJobsAsync(
        [Service] IMediator mediator, string? categoryCode = null, CancellationToken ct = default)
        => await mediator.Send(new GetJobsQuery(categoryCode), ct);

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(
        [Service] IMediator mediator, CancellationToken ct = default)
        => await mediator.Send(new GetCategoriesQuery(), ct);
}
