using AutoMapper;
using MediatR;
using MasterService.Application.DTOs;
using MasterService.Domain.Interfaces;

namespace MasterService.Application.Features.Skills.Queries;

public sealed class GetSkillsQueryHandler(ISkillRepository repository, IMapper mapper)
    : IRequestHandler<GetSkillsQuery, IEnumerable<SkillDto>>
{
    public async Task<IEnumerable<SkillDto>> Handle(GetSkillsQuery request, CancellationToken cancellationToken)
    {
        var skills = await repository.GetAllAsync(request.SkillType, cancellationToken);
        return mapper.Map<IEnumerable<SkillDto>>(skills);
    }
}

public sealed class GetSkillByCodeQueryHandler(ISkillRepository repository, IMapper mapper)
    : IRequestHandler<GetSkillByCodeQuery, SkillDto?>
{
    public async Task<SkillDto?> Handle(GetSkillByCodeQuery request, CancellationToken cancellationToken)
    {
        var skill = await repository.GetByCodeAsync(request.SkillCode, cancellationToken);
        return skill is null ? null : mapper.Map<SkillDto>(skill);
    }
}
