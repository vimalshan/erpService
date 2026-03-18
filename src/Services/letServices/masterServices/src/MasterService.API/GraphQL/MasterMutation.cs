using MasterService.Application.DTOs;
using MasterService.Application.Features.Skills.Commands;
using MasterService.Application.Features.Trainings.Commands;
using MasterService.Application.Features.Jobs.Commands;
using MediatR;

namespace MasterService.API.GraphQL;

public class MasterMutation
{
    public async Task<SkillDto> CreateSkillAsync(
        [Service] IMediator mediator,
        long skillCode, string skillName, char skillType,
        decimal? weightNum = null, string? remark = null,
        CancellationToken ct = default)
        => await mediator.Send(new CreateSkillCommand(skillCode, skillName, skillType, weightNum, remark, null), ct);

    public async Task<SkillDto> UpdateSkillAsync(
        [Service] IMediator mediator,
        long skillCode, string skillName, char skillType,
        decimal? weightNum = null, string? remark = null,
        CancellationToken ct = default)
        => await mediator.Send(new UpdateSkillCommand(skillCode, skillName, skillType, weightNum, remark), ct);

    public async Task<bool> CloseSkillAsync(
        [Service] IMediator mediator, long skillCode, CancellationToken ct = default)
    {
        await mediator.Send(new CloseSkillCommand(skillCode), ct);
        return true;
    }

    public async Task<TrainingProviderDto> CreateTrainingAsync(
        [Service] IMediator mediator,
        long trainingCode, string trainingName,
        string? address1 = null, string? contactName = null, string? phoneNum = null,
        long? groupCode = null, CancellationToken ct = default)
        => await mediator.Send(new CreateTrainingCommand(trainingCode, trainingName, address1, contactName, phoneNum, groupCode), ct);

    public async Task<JobMasterDto> CreateJobAsync(
        [Service] IMediator mediator,
        long jobCode, string jobName, string categoryCode,
        CancellationToken ct = default)
        => await mediator.Send(new CreateJobCommand(jobCode, jobName, categoryCode, null), ct);
}
