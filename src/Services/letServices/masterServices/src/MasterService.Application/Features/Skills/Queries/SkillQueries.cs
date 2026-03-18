using MediatR;
using MasterService.Application.DTOs;

namespace MasterService.Application.Features.Skills.Queries;

public record GetSkillsQuery(char? SkillType = null) : IRequest<IEnumerable<SkillDto>>;
public record GetSkillByCodeQuery(long SkillCode) : IRequest<SkillDto?>;
