using AutoMapper;
using MediatR;
using MasterService.Application.DTOs;
using MasterService.Domain.Entities;
using MasterService.Domain.Interfaces;

namespace MasterService.Application.Features.Skills.Commands;

public sealed class CreateSkillCommandHandler(ISkillRepository repository, IMapper mapper)
    : IRequestHandler<CreateSkillCommand, SkillDto>
{
    public async Task<SkillDto> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
    {
        if (await repository.ExistsAsync(request.SkillCode, cancellationToken))
            throw new InvalidOperationException($"Skill with code {request.SkillCode} already exists.");

        var skill = Skill.Create(request.SkillCode, request.SkillName, request.SkillType,
            request.WeightNum, request.Remark, request.EffectiveDate);

        await repository.AddAsync(skill, cancellationToken);
        return mapper.Map<SkillDto>(skill);
    }
}

public sealed class UpdateSkillCommandHandler(ISkillRepository repository, IMapper mapper)
    : IRequestHandler<UpdateSkillCommand, SkillDto>
{
    public async Task<SkillDto> Handle(UpdateSkillCommand request, CancellationToken cancellationToken)
    {
        var skill = await repository.GetByCodeAsync(request.SkillCode, cancellationToken)
            ?? throw new KeyNotFoundException($"Skill {request.SkillCode} not found.");

        skill.Update(request.SkillName, request.SkillType, request.WeightNum, request.Remark);
        await repository.UpdateAsync(skill, cancellationToken);
        return mapper.Map<SkillDto>(skill);
    }
}

public sealed class CloseSkillCommandHandler(ISkillRepository repository)
    : IRequestHandler<CloseSkillCommand, Unit>
{
    public async Task<Unit> Handle(CloseSkillCommand request, CancellationToken cancellationToken)
    {
        var skill = await repository.GetByCodeAsync(request.SkillCode, cancellationToken)
            ?? throw new KeyNotFoundException($"Skill {request.SkillCode} not found.");

        skill.Close();
        await repository.UpdateAsync(skill, cancellationToken);
        return Unit.Value;
    }
}
